using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class NewCameraMapController : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public RectTransform mapRectTransform; // the UI panel or RectMask2D

    [Header("Zoom Settings (original behavior)")]
    public float zoomStep = 1f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomDuration = 0.3f;

    [Header("Pan Settings")]
    [Tooltip("Edge region in pixels INSIDE the map rect where mouse causes edge-panning.")]
    public float edgeSize = 50f;
    [Tooltip("Pan speed is in world units/sec for edge panning (converted from screen pixels).")]
    public float panSpeed = 300f;
    public float panDuration = 0.25f;

    [Header("Behavior")]
    [Tooltip("If true, clamps the camera CENTER to the map rect. If false, clamps the view edge (original RTS style).")]
    public bool clampCameraCenterToMap = true;

    // runtime
    private Rect mapScreenRect;
    private Vector2 mapMinWorld;
    private Vector2 mapMaxWorld;

    private Vector3 targetPos;
    private float targetZoom;

    private Tweener zoomTween;
    private Tweener panTween;

    // drag
    private bool dragging;
    private Vector3 dragOriginWorld;
    private Vector3 velocityReset = Vector3.zero;

    void Start()
    {
        if (!cam) cam = Camera.main;
        if (!mapRectTransform) mapRectTransform = GetComponent<RectTransform>();

        targetPos = cam.transform.position;
        targetZoom = cam.orthographicSize;

        UpdateMapRects();
    }

    void Update()
    {
        UpdateMapRects();

        HandleZoom();     // your original zoom behaviour
        HandleEdgePan();  // edge panning inside the map rect
        HandleDrag();     // right-click drag
    }

    void UpdateMapRects()
    {
        if (!mapRectTransform) return;

        // world corners for clamping
        Vector3[] corners = new Vector3[4];
        mapRectTransform.GetWorldCorners(corners);
        mapMinWorld = corners[0];
        mapMaxWorld = corners[2];

        // screen rect (for mouse-in-map checks and edge pan inside the panel)
        Vector2 blScreen = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
        Vector2 trScreen = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);
        mapScreenRect = new Rect(blScreen.x, blScreen.y, trScreen.x - blScreen.x, trScreen.y - blScreen.y);
    }

    // === Your original zoom system (tweens + mouse pivot) ===
    void HandleZoom()
    {
        // only zoom when mouse is over the map panel
        if (!mapScreenRect.Contains(Input.mousePosition)) return;

        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newZoom = Mathf.Clamp(targetZoom - scroll * zoomStep, minZoom, maxZoom);

            // World point under cursor (before changing size)
            Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(Input.mousePosition);

            targetZoom = newZoom;

            // World point under cursor after targetZoom is applied (but we haven't set it on cam yet)
            // We can temporarily set cam.orthographicSize to compute the correct after point,
            // but the original approach simply captures ScreenToWorldPoint after changing targetZoom value
            // because DOTween will tween the ortho size. We'll compute the offset using the final targetZoom
            float savedSize = cam.orthographicSize;
            cam.orthographicSize = targetZoom;
            Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(Input.mousePosition);
            cam.orthographicSize = savedSize;

            Vector3 offset = mouseWorldBefore - mouseWorldAfter;
            targetPos += offset;

            // clamp target pos according to chosen behavior
            targetPos = ClampTarget(targetPos, targetZoom);

            // Tween zoom (only one tween at a time)
            zoomTween?.Kill();
            zoomTween = cam.DOOrthoSize(targetZoom, zoomDuration).SetEase(Ease.OutCubic);

            // Tween pan (only one tween at a time)
            panTween?.Kill();
            panTween = cam.transform.DOMove(targetPos, zoomDuration).SetEase(Ease.OutCubic)
                .OnUpdate(() =>
                {
                    // keep targetPos coherent with actual transform so other inputs behave predictably
                    targetPos = cam.transform.position;
                });
        }
    }

    // Edge pan but limited to the mapScreenRect (so the "little screen area" controls edges)
    void HandleEdgePan()
    {
        Vector3 move = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;

        // only consider edge pan if inside mapScreenRect (or you could allow outside if you prefer)
        if (!mapScreenRect.Contains(mousePos)) return;

        // Horizontal pan (proximity-based)
        if (mousePos.x < mapScreenRect.xMin + edgeSize) // left
        {
            float factor = 1f - ((mousePos.x - mapScreenRect.xMin) / edgeSize); // [0..1]
            move.x -= panSpeed * factor * Time.deltaTime;
        }
        else if (mousePos.x > mapScreenRect.xMax - edgeSize) // right
        {
            float factor = (mousePos.x - (mapScreenRect.xMax - edgeSize)) / edgeSize;
            move.x += panSpeed * factor * Time.deltaTime;
        }

        // Vertical pan (proximity-based)
        if (mousePos.y < mapScreenRect.yMin + edgeSize) // bottom
        {
            float factor = 1f - ((mousePos.y - mapScreenRect.yMin) / edgeSize);
            move.y -= panSpeed * factor * Time.deltaTime;
        }
        else if (mousePos.y > mapScreenRect.yMax - edgeSize) // top
        {
            float factor = (mousePos.y - (mapScreenRect.yMax - edgeSize)) / edgeSize;
            move.y += panSpeed * factor * Time.deltaTime;
        }

        if (move != Vector3.zero)
        {
            // move is already in world units/sec because panSpeed is world units/sec.
            targetPos += move;
            targetPos = ClampTarget(targetPos, targetZoom);

            // Start or update tween so panning is smooth and doesn't snap
            if (panTween == null || !panTween.IsActive() || !panTween.IsPlaying())
            {
                panTween?.Kill();
                panTween = cam.transform.DOMove(targetPos, panDuration).SetEase(Ease.OutCubic)
                    .OnUpdate(() => targetPos = cam.transform.position);
            }
            else
            {
                panTween.ChangeEndValue(targetPos, panDuration, true);
            }
        }
    }

    // Right-click drag for panning, with smoothing reset on release to avoid jitter
    void HandleDrag()
    {
        // Start drag on right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            if (mapScreenRect.Contains(Input.mousePosition))
            {
                dragging = true;
                dragOriginWorld = cam.ScreenToWorldPoint(Input.mousePosition);
                // immediately kill any pan tween and reset velocity-ish by setting target to current
                panTween?.Kill();
                targetPos = cam.transform.position;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            dragging = false;
            // ensure no residual tween velocities
            panTween?.Kill();
            targetPos = cam.transform.position;
        }

        if (!dragging) return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 delta = dragOriginWorld - mouseWorld;

        targetPos += delta;
        targetPos = ClampTarget(targetPos, targetZoom);

        // we set transform directly for immediate feel while dragging
        cam.transform.position = targetPos;

        // update drag origin so panning is continuous
        dragOriginWorld = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    // Clamp either by camera center or by view extents (original style)
    Vector3 ClampTarget(Vector3 proposedCenter, float orthoSize)
    {
        if (!clampCameraCenterToMap)
        {
            // original RTS style: clamp so view edge stays inside map
            float vertExtent = orthoSize;
            float horzExtent = vertExtent * cam.aspect;

            float minX = mapMinWorld.x + horzExtent;
            float maxX = mapMaxWorld.x - horzExtent;
            float minY = mapMinWorld.y + vertExtent;
            float maxY = mapMaxWorld.y - vertExtent;

            proposedCenter.x = Mathf.Clamp(proposedCenter.x, minX, maxX);
            proposedCenter.y = Mathf.Clamp(proposedCenter.y, minY, maxY);
            return proposedCenter;
        }
        else
        {
            // clamp camera center to map rect (camera origin stops at map bounds)
            proposedCenter.x = Mathf.Clamp(proposedCenter.x, mapMinWorld.x, mapMaxWorld.x);
            proposedCenter.y = Mathf.Clamp(proposedCenter.y, mapMinWorld.y, mapMaxWorld.y);
            return proposedCenter;
        }
    }

    // debugging helper to visualize the map rect when selected
    void OnDrawGizmosSelected()
    {
        if (!mapRectTransform) return;
        Vector3[] corners = new Vector3[4];
        mapRectTransform.GetWorldCorners(corners);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(corners[0], corners[1]);
        Gizmos.DrawLine(corners[1], corners[2]);
        Gizmos.DrawLine(corners[2], corners[3]);
        Gizmos.DrawLine(corners[3], corners[0]);
    }
}
