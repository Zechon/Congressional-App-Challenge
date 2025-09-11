using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CandidateCarousel : MonoBehaviour
{
    [Header("Carousel Settings")]
    public Transform candidateParent;
    public GameObject candidatePrefab;
    public float slideDuration = 0.5f;
    public float spacing = 6f;
    public float fadeDuration = 0.3f;
    public float scrollCooldown = 0.6f;

    [Header("Candidate Settings")]
    public float candidateScale = 2f;

    [Header("Stats Display")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text stat1Text;
    public TMP_Text skillText;
    public TMP_Text costText;
    public TMP_Text cateText;

    private List<StaffData> candidateDataList = new List<StaffData>();
    private List<GameObject> activeCandidates = new List<GameObject>();
    private int currentIndex = 0;

    private bool isOnCooldown = false;

    private CandidateSelectionManager selectionMgr;

    void Start()
    {
        selectionMgr = FindObjectOfType<CandidateSelectionManager>();

        StaffSelector selector = FindObjectOfType<StaffSelector>();
        if (selector != null)
        {
            Dictionary<CampaignRole, List<StaffData>> staffDict = selector.GetStaffChoices(3);

            foreach (var kvp in staffDict)
                candidateDataList.AddRange(kvp.Value);

            if (candidateDataList.Count > 0)
                SpawnInitialCandidates();
            else
                Debug.LogWarning("No candidates available for the carousel!");
        }
        else
        {
            Debug.LogWarning("StaffSelector not found in the scene!");
        }
    }

    private void SpawnInitialCandidates()
    {
        // Reset temp looks so we don't carry over colors from last run
        foreach (var candidate in candidateDataList)
            candidate.ResetTempLook();

        foreach (var obj in activeCandidates)
            Destroy(obj);
        activeCandidates.Clear();

        GameObject first = SpawnCandidateAtIndex(currentIndex, Vector3.zero, 1f);
        GameObject next = SpawnCandidateAtIndex((currentIndex + 1) % candidateDataList.Count, new Vector3(spacing, 0, 0), 0f);

        UpdateStatsDisplay(candidateDataList[currentIndex]);
    }


    private GameObject SpawnCandidateAtIndex(int index, Vector3 localPos, float alpha)
    {
        StaffData data = candidateDataList[index];
        GameObject candidate = Instantiate(candidatePrefab, candidateParent);
        candidate.transform.localPosition = localPos;
        candidate.transform.localScale = Vector3.one * candidateScale;

        CandidateGenerator gen = candidate.GetComponent<CandidateGenerator>();
        gen.PreviewCandidate(data); // preview using temp look

        CanvasGroup cg = candidate.GetComponent<CanvasGroup>();
        if (cg == null) cg = candidate.AddComponent<CanvasGroup>();
        cg.alpha = alpha;

        activeCandidates.Add(candidate);

        if (selectionMgr != null)
            selectionMgr.OnCandidateSpawned(candidate, data);

        return candidate;
    }


    void Update()
    {
        if (isOnCooldown) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextCandidate();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousCandidate();

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) PreviousCandidate();
        else if (scroll < 0f) NextCandidate();
    }

    public void NextCandidate()
    {
        int newIndex = (currentIndex + 1) % candidateDataList.Count;
        SlideToCandidate(newIndex, 1);
    }

    public void PreviousCandidate()
    {
        int newIndex = (currentIndex - 1 + candidateDataList.Count) % candidateDataList.Count;
        SlideToCandidate(newIndex, -1);
    }

    private void SlideToCandidate(int newIndex, int direction)
    {
        if (newIndex == currentIndex) return;

        StartCoroutine(CooldownRoutine());

        StaffData newData = candidateDataList[newIndex];

        Vector3 startOffset = new Vector3(direction * spacing, 0, 0);
        GameObject newCandidate = SpawnCandidateAtIndex(newIndex, startOffset, 0f);

        if (selectionMgr != null) selectionMgr.OnCandidateSpawned(newCandidate, newData);

        CanvasGroup oldCg = activeCandidates[0].GetComponent<CanvasGroup>();
        CanvasGroup newCg = newCandidate.GetComponent<CanvasGroup>();

        oldCg.DOFade(0f, fadeDuration);
        newCg.DOFade(1f, fadeDuration);

        activeCandidates[0].transform.DOLocalMove(-startOffset, slideDuration).SetEase(Ease.OutBack);
        newCandidate.transform.DOLocalMove(Vector3.zero, slideDuration).SetEase(Ease.OutBack);

        Destroy(activeCandidates[0], slideDuration + 0.05f);
        activeCandidates.Clear();
        activeCandidates.Add(newCandidate);

        UpdateStatsDisplay(newData);

        currentIndex = newIndex;
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(scrollCooldown);
        isOnCooldown = false;
    }

    private void UpdateStatsDisplay(StaffData data)
    {
        if (cateText) cateText.text = data.role.ToString();
        if (nameText) nameText.text = data.staffName;
        if (descriptionText) descriptionText.text = data.description;
        if (stat1Text) stat1Text.text = $"Stat1: {data.stat1}";
        if (skillText) skillText.text = $"Skill: {data.skill}";
        if (costText) costText.text = $"Cost: {data.cost}";
    }
    public StaffData GetCurrentCandidateData()
    {
        if (candidateDataList.Count == 0) return null;
        return candidateDataList[currentIndex];
    }

    public GameObject GetCurrentCandidateObject()
    {
        if (activeCandidates.Count == 0) return null;
        return activeCandidates[0];
    }
    public RectTransform GetCurrentCandidateRect()
    {
        if (activeCandidates.Count == 0) return null;
        return activeCandidates[0].GetComponent<RectTransform>();
    }
}
