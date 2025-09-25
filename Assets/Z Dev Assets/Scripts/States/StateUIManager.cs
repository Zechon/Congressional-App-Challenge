using UnityEngine;
using TMPro;
using DG.Tweening;

public class StateUIManager : MonoBehaviour
{
    public static StateUIManager Instance;

    [Header("UI References")]
    public TMP_Text hoverNameText;
    public TMP_Text selectedNameText;
    public TMP_Text selectedEcoText;
    public GameObject selectedPanel;

    [Header("Selection Settings")]
    public float selectedScale = 1.15f;
    public float selectedBounceMagnitude = 0.03f;
    public float selectedBounceSpeed = 0.7f;

    [Header("Camera/Layer")]
    public Camera cam;
    public LayerMask stateLayer2D;

    [Header("Layers")]
    [SerializeField] private Transform mapLayer;      // States' normal parent (inside mask)
    [SerializeField] private Transform floatingLayer; // States go here when selected

    private StateSetup currentHovered;
    private StateSetup currentSelected;

    private Vector3 originalScaleSelected;
    private int originalSiblingSelected;
    private Transform originalParentSelected;

    private Tween selectTween;

    void Awake()
    {
        Instance = this;
        hoverNameText.text = "";
        selectedEcoText.text = "";
        selectedPanel.SetActive(false);
    }

    void Update()
    {
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorld, stateLayer2D);
        StateSetup hoveredState = hit ? hit.GetComponent<StateSetup>() : null;

        if (hoveredState != currentHovered)
        {
            currentHovered = hoveredState;

            if (currentHovered != null)
                hoverNameText.text = currentHovered.stateName;
            else
                hoverNameText.text = "";
        }
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0) && currentHovered != null)
        {
            SelectState(currentHovered);
        }
    }

    public void SelectState(StateSetup state)
    {
        // If clicking the already selected state, do nothing
        if (currentSelected == state)
            return;

        // Reset previous selection
        if (currentSelected != null)
        {
            ResetSelection(currentSelected);
        }

        currentSelected = state;

        selectedPanel.SetActive(true);
        selectedNameText.text = state.stateName;
        selectedEcoText.text = state.economyLvl switch
        {
            1 => "Hyperinflated Economy",
            2 => "Inflated",
            3 => "Stable",
            4 => "Growing Economy",
            5 => "Booming Economy",
            _ => "Unknown"
        };

        originalScaleSelected = state.transform.localScale;
        originalSiblingSelected = state.transform.GetSiblingIndex();
        originalParentSelected = state.transform.parent;

        // Reparent to floating layer so it won't get clipped
        state.transform.SetParent(floatingLayer, true);
        state.transform.SetAsLastSibling();

        selectTween?.Kill();
        selectTween = state.transform
            .DOScale(originalScaleSelected * selectedScale, 0.2f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                selectTween = state.transform
                    .DOScale(originalScaleSelected * (selectedScale + selectedBounceMagnitude), 0.5f / selectedBounceSpeed)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            });
    }


    public void DeselectState()
    {
        if (currentSelected != null)
        {
            ResetSelection(currentSelected);
            currentSelected = null;
        }

        selectedPanel.SetActive(false);
        selectedNameText.text = "";
    }

    private void ResetSelection(StateSetup state)
    {
        selectTween?.Kill();
        state.transform.DOKill();
        state.transform.DOScale(originalScaleSelected, 0.2f).SetEase(Ease.OutCubic);

        // Move it back to original parent and sibling index
        state.transform.SetParent(originalParentSelected, true);
        state.transform.SetSiblingIndex(originalSiblingSelected);
    }
}
