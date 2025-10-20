using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollableActionPanelTest : MonoBehaviour
{
    void Start()
    {
        // Create a Scroll View root object
        GameObject scrollGO = new GameObject("ActionPanel", typeof(RectTransform), typeof(ScrollRect), typeof(Image));
        scrollGO.transform.SetParent(transform, false);
        RectTransform scrollRT = scrollGO.GetComponent<RectTransform>();
        scrollRT.anchorMin = new Vector2(0.5f, 0.5f);
        scrollRT.anchorMax = new Vector2(0.5f, 0.5f);
        scrollRT.pivot = new Vector2(0.5f, 0.5f);
        scrollRT.sizeDelta = new Vector2(400, 500);
        scrollGO.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.4f);

        // Viewport
        GameObject viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(RectMask2D));
        viewport.transform.SetParent(scrollGO.transform, false);
        var viewportRT = viewport.GetComponent<RectTransform>();
        viewportRT.anchorMin = Vector2.zero;
        viewportRT.anchorMax = Vector2.one;
        viewportRT.offsetMin = viewportRT.offsetMax = Vector2.zero;
        viewport.GetComponent<Image>().color = new Color(0, 0, 0, 0); // transparent

        // Content
        GameObject content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
        content.transform.SetParent(viewport.transform, false);
        var contentRT = content.GetComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0, 1);
        contentRT.anchorMax = new Vector2(1, 1);
        contentRT.pivot = new Vector2(0.5f, 1);
        contentRT.anchoredPosition = Vector2.zero;

        var vlg = content.GetComponent<VerticalLayoutGroup>();
        vlg.spacing = 50;
        vlg.padding = new RectOffset(10, 10, 20, 20); //top/bottom padding fixes clipping
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.childControlHeight = true;
        vlg.childControlWidth = true;


        var csf = content.GetComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        // Scrollbar
        GameObject scrollbarGO = new GameObject("Scrollbar Vertical", typeof(RectTransform), typeof(Image), typeof(Scrollbar));
        scrollbarGO.transform.SetParent(scrollGO.transform, false);
        RectTransform sbRT = scrollbarGO.GetComponent<RectTransform>();
        sbRT.anchorMin = new Vector2(1, 0);
        sbRT.anchorMax = new Vector2(1, 1);
        sbRT.pivot = new Vector2(1, 1);
        sbRT.sizeDelta = new Vector2(20, 0);
        scrollbarGO.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);

        Scrollbar sb = scrollbarGO.GetComponent<Scrollbar>();
        sb.direction = Scrollbar.Direction.BottomToTop;

        // ScrollRect setup
        ScrollRect sr = scrollGO.GetComponent<ScrollRect>();
        sr.content = contentRT;
        sr.viewport = viewportRT;
        sr.verticalScrollbar = sb;
        sr.horizontal = false;
        sr.vertical = true;
        sr.movementType = ScrollRect.MovementType.Clamped;
        sr.scrollSensitivity = 40f;

        // Scrollbar handle
        GameObject handle = new GameObject("Handle", typeof(RectTransform), typeof(Image));
        handle.transform.SetParent(scrollbarGO.transform, false);
        handle.GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
        RectTransform handleRT = handle.GetComponent<RectTransform>();
        handleRT.anchorMin = Vector2.zero;
        handleRT.anchorMax = Vector2.one;
        handleRT.offsetMin = handleRT.offsetMax = Vector2.zero;
        sb.handleRect = handleRT;
        sb.targetGraphic = handle.GetComponent<Image>();

        // Generate 20 action buttons
        for (int i = 0; i < 20; i++)
        {
            GameObject btnGO = new GameObject($"ActionButton_{i + 1}", typeof(RectTransform), typeof(Image), typeof(Button));
            btnGO.transform.SetParent(content.transform, false);
            var btnRT = btnGO.GetComponent<RectTransform>();
            btnRT.sizeDelta = new Vector2(0, 80);
            btnGO.GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value, 0.8f);

            // Add label
            GameObject labelGO = new GameObject("Label", typeof(TextMeshProUGUI));
            labelGO.transform.SetParent(btnGO.transform, false);
            var label = labelGO.GetComponent<TextMeshProUGUI>();
            label.text = $"Action {i + 1}";
            label.fontSize = 24;
            label.alignment = TextAlignmentOptions.Center;
            RectTransform labelRT = labelGO.GetComponent<RectTransform>();
            labelRT.anchorMin = Vector2.zero;
            labelRT.anchorMax = Vector2.one;
            labelRT.offsetMin = labelRT.offsetMax = Vector2.zero;
        }

        Debug.Log("Scrollable Action Panel created successfully!");
    }
}
