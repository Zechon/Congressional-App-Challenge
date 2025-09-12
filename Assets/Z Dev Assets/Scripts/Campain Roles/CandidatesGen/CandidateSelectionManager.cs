using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class CandidateSelectionManager : MonoBehaviour
{
    [Header("Carousel")]
    public CandidateCarousel carousel;
    [SerializeField] private GameObject fadeOverlay;
    [SerializeField] private float fadeDuration = 0.3f;

    [Header("UI Buttons")]
    public Button selectButton;
    public Button confirmButton;

    [Header("Currency & Budget")]
    public TMP_Text budgetText;
    public int playerBudget = 10;

    [Header("Visual Feedback")]
    public float checkFadeDuration = 0.3f;

    private Dictionary<CampaignRole, StaffData> selectedCandidates = new Dictionary<CampaignRole, StaffData>();
    private int totalSpent = 0;

    void Start()
    {
        selectButton.onClick.AddListener(OnSelectPressed);
        confirmButton.onClick.AddListener(OnConfirmPressed);

        UpdateBudgetUI();
        StartCoroutine(InitializeAfterFadeIn());
    }

    private IEnumerator InitializeAfterFadeIn()
    {
        // Wait one extra frame just to be safe
        yield return null;

        // Now initialize the carousel
        if (carousel != null)
            carousel.InitializeCarousel();
        else
            Debug.LogWarning("CandidateCarousel not assigned in CandidateSelectionManager!");

        // Run the fade-in
        fadeOverlay.GetComponent<CanvasGroup>().DOFade(0f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        fadeOverlay.SetActive(false);
    }

    private void OnSelectPressed()
    {
        StaffData currentCandidate = carousel.GetCurrentCandidateData();
        GameObject currentObj = carousel.GetCurrentCandidateObject();
        if (currentCandidate == null || currentObj == null) return;

        CampaignRole role = currentCandidate.role;

        if (selectedCandidates.TryGetValue(role, out StaffData oldSelected) && oldSelected != currentCandidate)
        {
            totalSpent -= oldSelected.cost;
            SetCheckmark(oldSelected, false);
            selectedCandidates.Remove(role);
        }

        CandidateGenerator gen = currentObj.GetComponent<CandidateGenerator>();
        if (gen != null)
            gen.SetupCandidate(currentCandidate);

        selectedCandidates[role] = currentCandidate;
        totalSpent += currentCandidate.cost;

        SetCheckmark(currentCandidate, true, currentObj);

        UpdateBudgetUI();
    }

    public void OnCandidateSpawned(GameObject candidateObj, StaffData data)
    {
        if (candidateObj == null || data == null) return;

        // Enable check if this candidate is selected
        bool isSelected = selectedCandidates.TryGetValue(data.role, out StaffData sel) && sel == data;
        SetCheckmark(data, isSelected, candidateObj);
    }

    private void SetCheckmark(StaffData data, bool enable, GameObject candidateObj = null)
    {
        GameObject obj = candidateObj ?? carousel.GetCurrentCandidateObject();
        if (obj == null) return;

        Image checkImage = null;
        foreach (Transform t in obj.GetComponentsInChildren<Transform>(true))
        {
            if (t.CompareTag("candChk"))
            {
                checkImage = t.GetComponent<Image>();
                break;
            }
        }

        if (checkImage == null) return;

        checkImage.DOKill();

        if (enable)
        {
            checkImage.enabled = true;
            checkImage.color = new Color(checkImage.color.r, checkImage.color.g, checkImage.color.b, 0f);
            checkImage.DOFade(1f, checkFadeDuration).SetEase(Ease.OutBack);
        }
        else
        {
            checkImage.DOFade(0f, 0f).OnComplete(() => checkImage.enabled = false);
        }
    }

    private void OnConfirmPressed()
    {
        int roleCount = System.Enum.GetValues(typeof(CampaignRole)).Length;
        if (selectedCandidates.Count < roleCount)
        {
            Debug.LogWarning("You must select one candidate per role before confirming!");
            return;
        }

        if (totalSpent > playerBudget)
        {
            Debug.LogWarning("Not enough budget!");
            return;
        }

        Debug.Log("Candidates confirmed! Hiring:");
        foreach (var candidate in selectedCandidates.Values)
            Debug.Log($"{candidate.staffName} ({candidate.role})");

        SaveSelectedCandidates();

        fadeOverlay.SetActive(true);
        fadeOverlay.GetComponent<CanvasGroup>().DOFade(1f, fadeDuration);
    }

    private void UpdateBudgetUI()
    {
        if (budgetText != null)
            budgetText.text = $"Budget: {playerBudget - totalSpent}/{playerBudget}";
    }

    private void SaveSelectedCandidates()
    {
        GameData.HiredStaff = new List<StaffData>(selectedCandidates.Values);
    }
}
