using UnityEngine;
using DG.Tweening;

public class MapController : MonoBehaviour
{
    public RectTransform mapContent;
    public RectTransform viewport; // the mask/viewport
    public Canvas canvas;

    [Header("Zoom Settings")]
    public float zoomStep = 0.1f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;
    public float zoomDuration = 0.3f;

    [Header("Pan Settings")]
    public float edgeSize = 50f;
    public float panSpeed = 500f;

    private Vector3 targetPosition;
    private float targetZoom = 1f;

    void Start()
    {
        targetPosition = mapContent.localPosition;
        targetZoom = mapContent.localScale.x;
    }

    void Update()
    {
        HandleZoom();
        HandleEdgePan();
    }

    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll == 0) return;

        // Calculate zoom
        float newZoom = Mathf.Clamp(targetZoom + scroll * zoomStep, minZoom, maxZoom);
        float scaleFactor = newZoom / targetZoom;

        // Get mouse position relative to map pivot
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapContent, Input.mousePosition, canvas.worldCamera, out localMousePos);

        // Adjust map position so the point under the cursor stays
        targetPosition = mapContent.localPosition + (Vector3)(localMousePos * (1 - scaleFactor));

        targetZoom = newZoom;
        ClampPosition();

        mapContent
            .DOScale(Vector3.one * targetZoom, zoomDuration)
            .SetEase(Ease.OutCubic);
        mapContent
            .DOLocalMove(targetPosition, zoomDuration)
            .SetEase(Ease.OutCubic);
    }

    void HandleEdgePan()
    {
        Vector3 pan = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x <= edgeSize)
            pan.x += panSpeed * Time.deltaTime;
        else if (mousePos.x >= Screen.width - edgeSize)
            pan.x -= panSpeed * Time.deltaTime;

        if (mousePos.y <= edgeSize)
            pan.y += panSpeed * Time.deltaTime;
        else if (mousePos.y >= Screen.height - edgeSize)
            pan.y -= panSpeed * Time.deltaTime;

        if (pan != Vector3.zero)
        {
            targetPosition += pan;
            ClampPosition();
            mapContent.DOLocalMove(targetPosition, 0.2f).SetEase(Ease.OutCubic);
        }
    }

    void ClampPosition()
    {
        // Calculate map boundaries relative to viewport
        Vector2 mapSize = mapContent.rect.size * targetZoom;
        Vector2 viewSize = viewport.rect.size;

        float xMin = -(mapSize.x - viewSize.x) / 2f;
        float xMax = (mapSize.x - viewSize.x) / 2f;
        float yMin = -(mapSize.y - viewSize.y) / 2f;
        float yMax = (mapSize.y - viewSize.y) / 2f;

        targetPosition.x = Mathf.Clamp(targetPosition.x, xMin, xMax);
        targetPosition.y = Mathf.Clamp(targetPosition.y, yMin, yMax);
    }
}
