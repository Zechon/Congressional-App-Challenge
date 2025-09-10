using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CandidateSelectionManager : MonoBehaviour
{
    [Header("Carousel")]
    public CandidateCarousel carousel;

    [Header("UI Buttons")]
    public Button selectButton;
    public Button confirmButton;

    [Header("Currency & Budget")]
    public TMP_Text budgetText;
    public int playerBudget = 10;

    [Header("Visual Feedback")]
    public GameObject selectionHighlightPrefab;
    public Transform checkmark;
    public float highlightMoveDuration = 0.3f;

    private Dictionary<CampaignRole, StaffData> selectedCandidates = new Dictionary<CampaignRole, StaffData>();
    private Dictionary<CampaignRole, GameObject> selectedHighlights = new Dictionary<CampaignRole, GameObject>();

    void Start()
    {
        selectButton.onClick.AddListener(OnSelectPressed);
        confirmButton.onClick.AddListener(OnConfirmPressed);

        if (checkmark != null)
            checkmark.gameObject.SetActive(false);

        UpdateBudgetUI();
    }

    private void OnSelectPressed()
    {
        StaffData currentCandidate = carousel.GetCurrentCandidateData();
        if (currentCandidate == null) return;

        CampaignRole role = currentCandidate.role;

        // --- Highlight handling ---
        if (selectedHighlights.ContainsKey(role) && selectedHighlights[role] != null)
        {
            GameObject highlight = selectedHighlights[role];
            highlight.transform.SetParent(carousel.GetCurrentCandidateObject().transform);
            highlight.transform.DOLocalMove(Vector3.zero, highlightMoveDuration).SetEase(Ease.OutCubic);
        }
        else
        {
            GameObject highlight = Instantiate(selectionHighlightPrefab, carousel.GetCurrentCandidateObject().transform);
            highlight.transform.localPosition = Vector3.zero;
            highlight.transform.SetAsFirstSibling();
            selectedHighlights[role] = highlight;
        }

        // --- Update selection ---
        selectedCandidates[role] = currentCandidate;

        // --- Move the checkmark ---
        MoveCheckmarkToCurrentCandidate();

        UpdateBudgetUI();
    }

    public void UpdateCurrentCandidateVisuals()
    {
        MoveCheckmarkToCurrentCandidate();
    }

    private void OnConfirmPressed()
    {
        if (selectedCandidates.Count < System.Enum.GetValues(typeof(CampaignRole)).Length)
        {
            Debug.LogWarning("You must select one candidate per role before confirming!");
            return;
        }

        int totalCost = 0;
        foreach (var candidate in selectedCandidates.Values)
            totalCost += candidate.cost;

        if (totalCost > playerBudget)
        {
            Debug.LogWarning("Not enough budget!");
            return;
        }

        Debug.Log("Candidates confirmed! Hiring:");
        foreach (var candidate in selectedCandidates.Values)
            Debug.Log($"{candidate.staffName} ({candidate.role})");

        SaveSelectedCandidates();
    }

    private void UpdateBudgetUI()
    {
        int totalCost = 0;
        foreach (var candidate in selectedCandidates.Values)
            totalCost += candidate.cost;

        budgetText.text = $"Budget: {playerBudget - totalCost}/{playerBudget}";
    }

    private void SaveSelectedCandidates()
    {
        GameData.HiredStaff = new List<StaffData>(selectedCandidates.Values);
    }

    private void MoveCheckmarkToCurrentCandidate()
    {
        if (checkmark == null) return;

        StaffData currentCandidate = carousel.GetCurrentCandidateData();
        if (currentCandidate == null)
        {
            checkmark.gameObject.SetActive(false);
            return;
        }

        // Show checkmark only if this candidate is selected
        bool isSelected = selectedCandidates.ContainsValue(currentCandidate);

        checkmark.gameObject.SetActive(isSelected);

        if (!isSelected) return;

        // Find the anchor in the candidate prefab
        Transform anchor = null;
        foreach (Transform t in carousel.GetCurrentCandidateObject().GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("CheckmarkAnchor"))
            {
                anchor = t;
                break;
            }
        }
        if (anchor == null) anchor = carousel.GetCurrentCandidateObject().transform;

        // Tween checkmark to anchor
        checkmark.SetParent(anchor); // re-parent instantly
        checkmark.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.OutCubic); // smooth move
        checkmark.DOLocalRotate(Vector3.zero, 0.25f).SetEase(Ease.OutCubic); // optional: rotate smoothly
    }

}
