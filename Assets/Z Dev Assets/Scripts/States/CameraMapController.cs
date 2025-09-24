using UnityEngine;
using DG.Tweening;

public class CameraMapController : MonoBehaviour
{
    public Camera cam;

    [Header("Zoom Settings")]
    public float zoomStep = 1f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomDuration = 0.3f;

    [Header("Pan Settings")]
    public float edgeSize = 50f;
    public float panSpeed = 10f;
    public float panDuration = 0.3f;

    [Header("Map Bounds")]
    public Vector2 mapMin; // bottom-left world corner
    public Vector2 mapMax; // top-right world corner

    private Vector3 targetPos;
    private float targetZoom;

    private Tweener zoomTween;
    private Tweener panTween;

    void Start()
    {
        if (!cam) cam = Camera.main;
        targetPos = cam.transform.position;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleZoom();
        HandleEdgePan();
    }

    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newZoom = Mathf.Clamp(targetZoom - scroll * zoomStep, minZoom, maxZoom);

            // World point under cursor
            Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(Input.mousePosition);

            targetZoom = newZoom;

            Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 offset = mouseWorldBefore - mouseWorldAfter;
            targetPos += offset;

            ClampTarget();

            // Tween zoom only once
            zoomTween?.Kill();
            zoomTween = cam.DOOrthoSize(targetZoom, zoomDuration).SetEase(Ease.OutCubic);

            // Tween pan only once
            panTween?.Kill();
            panTween = cam.transform.DOMove(targetPos, zoomDuration).SetEase(Ease.OutCubic);
        }
    }

    void HandleEdgePan()
    {
        Vector3 move = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;

        // Horizontal pan (proximity-based)
        if (mousePos.x < edgeSize) // left
        {
            float factor = 1f - (mousePos.x / edgeSize); // closer to edge = faster
            move.x -= panSpeed * factor * Time.deltaTime;
        }
        else if (mousePos.x > Screen.width - edgeSize) // right
        {
            float factor = (mousePos.x - (Screen.width - edgeSize)) / edgeSize;
            move.x += panSpeed * factor * Time.deltaTime;
        }

        // Vertical pan (proximity-based)
        if (mousePos.y < edgeSize) // bottom
        {
            float factor = 1f - (mousePos.y / edgeSize);
            move.y -= panSpeed * factor * Time.deltaTime;
        }
        else if (mousePos.y > Screen.height - edgeSize) // top
        {
            float factor = (mousePos.y - (Screen.height - edgeSize)) / edgeSize;
            move.y += panSpeed * factor * Time.deltaTime;
        }

        if (move != Vector3.zero)
        {
            targetPos += move;
            ClampTarget();

            // Start or update tween
            if (panTween == null || !panTween.IsActive() || !panTween.IsPlaying())
            {
                panTween?.Kill();
                panTween = cam.transform.DOMove(targetPos, panDuration).SetEase(Ease.OutCubic);
            }
            else
            {
                panTween.ChangeEndValue(targetPos, panDuration, true);
            }
        }
    }


    void ClampTarget()
    {
        float vertExtent = targetZoom;
        float horzExtent = vertExtent * cam.aspect;

        float minX = mapMin.x + horzExtent;
        float maxX = mapMax.x - horzExtent;
        float minY = mapMin.y + vertExtent;
        float maxY = mapMax.y - vertExtent;

        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
    }
}
