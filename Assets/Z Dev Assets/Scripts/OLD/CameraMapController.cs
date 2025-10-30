using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class CameraMapController : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    [Tooltip("RectTransform that defines the visible map area (RectMask2D / UI panel).")]
    public RectTransform mapRectTransform;

    [Header("Zoom Settings")]
    public float zoomStep = 1f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomDuration = 0.25f;

    [Header("Pan Settings")]
    [Tooltip("Edge region in pixels INSIDE the map rect where mouse causes panning.")]
    public float edgeSize = 32f;
    public float panSpeed = 300f;
    public float panDuration = 0.2f;

    [Header("Behavior")]
    public bool clampCameraCenterToMap = true;

    private Rect mapScreenRect;   // map rect in screen coords
    private Vector2 mapMinWorld;  // world-space corners (bottom-left)
    private Vector2 mapMaxWorld;  // world-space corners (top-right)

    private Vector3 targetPos;
    private float targetZoom;

    private Tweener zoomTween;
    private Tweener panTween;

    void Start()
    {
        if (!cam) cam = Camera.main;
        if (!mapRectTransform) mapRectTransform = GetComponent<RectTransform>();

        targetZoom = cam.orthographicSize;
        targetPos = cam.transform.position;

        UpdateMapRects();
    }

    void Update()
    {
        UpdateMapRects();

        HandleZoom();
        HandleEdgePan();
    }

    void UpdateMapRects()
    {
        if (mapRectTransform == null) return;

        Vector3[] corners = new Vector3[4];
        mapRectTransform.GetWorldCorners(corners); // 0 = bl, 2 = tr
        mapMinWorld = corners[0];
        mapMaxWorld = corners[2];

        Vector2 blScreen = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
        Vector2 trScreen = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);
        float left = blScreen.x;
        float bottom = blScreen.y;
        float width = trScreen.x - blScreen.x;
        float height = trScreen.y - blScreen.y;
        mapScreenRect = new Rect(left, bottom, width, height);
    }

    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) < 0.01f) return;

        float newZoom = Mathf.Clamp(targetZoom - scroll * zoomStep, minZoom, maxZoom);
        if (Mathf.Approximately(newZoom, targetZoom)) return;

        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, cam.transform.position.z - 0f));

        float prevZoom = targetZoom;
        targetZoom = newZoom;

        float savedSize = cam.orthographicSize;
        cam.orthographicSize = targetZoom;
        Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, cam.transform.position.z - 0f));
        cam.orthographicSize = savedSize;

        Vector3 desiredTargetPos = targetPos + (mouseWorldBefore - mouseWorldAfter);

        desiredTargetPos = ClampCameraCenter(desiredTargetPos, targetZoom);

        zoomTween?.Kill();
        zoomTween = DOTween.To(() => cam.orthographicSize, x => cam.orthographicSize = x, targetZoom, zoomDuration).SetEase(Ease.OutCubic);

        panTween?.Kill();
        panTween = cam.transform.DOMove(desiredTargetPos, zoomDuration).SetEase(Ease.OutCubic)
            .OnUpdate(() =>
            {
                targetPos = cam.transform.position;
            });

        targetPos = desiredTargetPos;
    }

    void HandleEdgePan()
    {
        Vector3 mouse = Input.mousePosition;

        if (!mapScreenRect.Contains(mouse)) return;

 
        float fx = 0f;
        if (mouse.x < mapScreenRect.xMin + edgeSize)
            fx = 1f - ((mouse.x - mapScreenRect.xMin) / edgeSize); 
        else if (mouse.x > mapScreenRect.xMax - edgeSize)
            fx = (mouse.x - (mapScreenRect.xMax - edgeSize)) / edgeSize; 

        float fy = 0f;
        if (mouse.y < mapScreenRect.yMin + edgeSize)
            fy = 1f - ((mouse.y - mapScreenRect.yMin) / edgeSize); 
        else if (mouse.y > mapScreenRect.yMax - edgeSize)
            fy = (mouse.y - (mapScreenRect.yMax - edgeSize)) / edgeSize;

        if (Mathf.Approximately(fx, 0f) && Mathf.Approximately(fy, 0f)) return;

        Vector2 screenDir = new Vector2(0f, 0f);
        if (fx > 0f)
        {
            if (mouse.x < mapScreenRect.xMin + edgeSize) screenDir.x = -1f;
            else screenDir.x = 1f;
        }
        if (fy > 0f)
        {
            if (mouse.y < mapScreenRect.yMin + edgeSize) screenDir.y = -1f;
            else screenDir.y = 1f;
        }

        float intensity = Mathf.Clamp01(Mathf.Max(fx, fy));

        float worldPerPixel = (2f * cam.orthographicSize) / Screen.height;
        Vector2 worldMovePerSecond = screenDir * panSpeed * worldPerPixel;

        Vector3 deltaWorld = (Vector3)(worldMovePerSecond * intensity * Time.deltaTime);

        targetPos += deltaWorld;
        targetPos = ClampCameraCenter(targetPos, cam.orthographicSize);

        panTween?.Kill();
        panTween = cam.transform.DOMove(targetPos, panDuration).SetEase(Ease.OutCubic)
            .OnUpdate(() =>
            {
                targetPos = cam.transform.position;
            });
    }

    Vector3 ClampCameraCenter(Vector3 proposedCenter, float orthoSize)
    {
        if (!clampCameraCenterToMap) return proposedCenter;

        float clampedX = Mathf.Clamp(proposedCenter.x, mapMinWorld.x, mapMaxWorld.x);
        float clampedY = Mathf.Clamp(proposedCenter.y, mapMinWorld.y, mapMaxWorld.y);

        return new Vector3(clampedX, clampedY, proposedCenter.z);
    }

    void OnDrawGizmosSelected()
    {
        if (mapRectTransform == null) return;
        Vector3[] corners = new Vector3[4];
        mapRectTransform.GetWorldCorners(corners);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(corners[0], corners[1]);
        Gizmos.DrawLine(corners[1], corners[2]);
        Gizmos.DrawLine(corners[2], corners[3]);
        Gizmos.DrawLine(corners[3], corners[0]);
    }
}
