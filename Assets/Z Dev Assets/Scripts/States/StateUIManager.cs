using UnityEngine;
using TMPro;
using DG.Tweening;

public class StateUIManager : MonoBehaviour
{
    public static StateUIManager Instance;

    [Header("UI References")]
    public TMP_Text selectedNameText;
    public TMP_Text selectedEcoText;
    public TMP_Text p;
    public TMP_Text o;
    public TMP_Text t;

    [Header("Selection Settings")]
    public float selectedScale = 1.15f;
    public float selectedBounceMagnitude = 0.03f;
    public float selectedBounceSpeed = 0.7f;

    [Header("Camera/Layer")]
    public Camera cam;
    public LayerMask stateLayer2D;

    [Header("Layers")]
    [SerializeField] private Transform mapLayer;
    public Transform MapLayer => mapLayer;
    [SerializeField] private Transform floatingLayer;

    private StateSetup currentSelected;

    private Vector3 originalScaleSelected;
    private int originalSiblingSelected;
    private Transform originalParentSelected;

    private Tween selectTween;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        HandleClick();
    }

    void HandleClick()
    {
        if (ActionPanelUI.instance != null && ActionPanelUI.instance.gameObject.activeInHierarchy)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld, stateLayer2D);
            StateSetup clickedState = hit ? hit.GetComponent<StateSetup>() : null;

            if (clickedState != null)
                SelectState(clickedState);
        }
    }

    public void SelectState(StateSetup state)
    {
        if (currentSelected == state)
            return;

        if (currentSelected != null)
            ResetSelection(currentSelected);

        currentSelected = state;

        selectedNameText.text = state.stateName;
        selectedEcoText.text = state.economyLvl switch
        {
            1 => "Eco: Hyperinflated",
            2 => "Economy: Inflated",
            3 => "Economy: Stable",
            4 => "Economy: Growing",
            5 => "Economy: Booming",
            0 => "Economy Not Set",
            _ => "Economy: Unknown"
        };

        p.text = (Mathf.FloorToInt(state.stateVotes * currentSelected.purplePercent)).ToString();
        o.text = (state.stateVotes - Mathf.FloorToInt(state.stateVotes * currentSelected.purplePercent)).ToString();
        t.text = state.stateVotes.ToString();

        originalScaleSelected = state.transform.localScale;
        originalSiblingSelected = state.transform.GetSiblingIndex();
        originalParentSelected = state.transform.parent;

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

        selectedNameText.text = "";
    }

    public StateSetup GetCurrentSelectedState()
    {
        return currentSelected;
    }


    private void ResetSelection(StateSetup state)
    {
        selectTween?.Kill();
        state.transform.DOKill();
        state.transform.DOScale(originalScaleSelected, 0.2f).SetEase(Ease.OutCubic);

        state.transform.SetParent(originalParentSelected, true);
        state.transform.SetSiblingIndex(originalSiblingSelected);
    }
}
