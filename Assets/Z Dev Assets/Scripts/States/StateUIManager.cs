using UnityEngine;
using TMPro;
using DG.Tweening;

public class StateUIManager : MonoBehaviour
{
    public static StateUIManager Instance;

    [Header("UI References")]
    public TMP_Text selectedNameText;
    public TMP_Text selectedEcoText;
    public GameObject selectedPanel;
    public TMP_Text prcntText;
    public TMP_Text ttlVotes;

    [Header("Selection Settings")]
    public float selectedScale = 1.15f;
    public float selectedBounceMagnitude = 0.03f;
    public float selectedBounceSpeed = 0.7f;

    [Header("Camera/Layer")]
    public Camera cam;
    public LayerMask stateLayer2D;

    [Header("Layers")]
    [SerializeField] private Transform mapLayer;
    [SerializeField] private Transform floatingLayer;

    private StateSetup currentSelected;

    private Vector3 originalScaleSelected;
    private int originalSiblingSelected;
    private Transform originalParentSelected;

    private Tween selectTween;

    void Awake()
    {
        Instance = this;
        selectedEcoText.text = "";
        prcntText.text = "";
        ttlVotes.text = "";
        selectedPanel.SetActive(false);
    }

    void Update()
    {
        HandleClick();
    }

    void HandleClick()
    {
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

        prcntText.text = state.currentColor switch
        {
            PartyColor.Purple => $"{state.PurpleRatioClamped() * 100f:0.#}% P",
            PartyColor.Orange => $"{state.OrangeRatioClamped() * 100f:0.#}% O",
            PartyColor.Brown => GameData.Party == "Orange"
                ? $"{state.OrangeRatioClamped() * 100f:0.#}% O"
                : $"{state.PurpleRatioClamped() * 100f:0.#}% P",
            _ => ""
        };

        ttlVotes.text = state.stateVotes.ToString();

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

        selectedPanel.SetActive(true);
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

        state.transform.SetParent(originalParentSelected, true);
        state.transform.SetSiblingIndex(originalSiblingSelected);
    }
}
