using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CandidateCarousel : MonoBehaviour
{
    [Header("Carousel Settings")]
    public Transform candidateParent;
    public GameObject candidatePrefab;
    public float slideDuration = 0.5f;
    public float spacing = 6f;
    public float fadeDuration = 0.3f;

    [Header("Candidate Settings")]
    public float candidateScale = 2f;

    [Header("Stats Display")]
    public Text nameText;
    public Text descriptionText;
    public Text stat1Text;
    public Text skillText;
    public Text costText;

    private List<StaffData> candidateDataList = new List<StaffData>();
    private List<GameObject> activeCandidates = new List<GameObject>(); // only current + previous/next
    private int currentIndex = 0;

    void Start()
    {
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
        // Clear previous
        foreach (var obj in activeCandidates)
            Destroy(obj);
        activeCandidates.Clear();

        // Spawn current candidate
        SpawnCandidateAtIndex(currentIndex, Vector3.zero, 1f);

        // Spawn next candidate just offscreen to the right
        int nextIndex = (currentIndex + 1) % candidateDataList.Count;
        SpawnCandidateAtIndex(nextIndex, new Vector3(spacing, 0, 0), 0f);
    }

    private GameObject SpawnCandidateAtIndex(int index, Vector3 localPos, float alpha)
    {
        StaffData data = candidateDataList[index];
        GameObject candidate = Instantiate(candidatePrefab, candidateParent);
        candidate.transform.localPosition = localPos;
        candidate.transform.localScale = Vector3.one * candidateScale;

        CandidateGenerator gen = candidate.GetComponent<CandidateGenerator>();
        gen.SetupCandidate(data);

        CanvasGroup cg = candidate.GetComponent<CanvasGroup>();
        if (cg == null) cg = candidate.AddComponent<CanvasGroup>();
        cg.alpha = alpha;

        activeCandidates.Add(candidate);
        return candidate;
    }

    void Update()
    {
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
        // direction: 1 = right scroll (next), -1 = left scroll (previous)
        StaffData newData = candidateDataList[newIndex];

        // Spawn the incoming candidate
        Vector3 startOffset = new Vector3(direction * spacing, 0, 0);
        GameObject newCandidate = SpawnCandidateAtIndex(newIndex, startOffset, 0f);

        // Animate old candidate out
        GameObject oldCandidate = activeCandidates[0];
        oldCandidate.GetComponent<CanvasGroup>().DOFade(0f, fadeDuration);

        // Animate new candidate in
        newCandidate.GetComponent<CanvasGroup>().DOFade(1f, fadeDuration);
        oldCandidate.transform.DOLocalMove(-startOffset, slideDuration).SetEase(Ease.OutBack);
        newCandidate.transform.DOLocalMove(Vector3.zero, slideDuration).SetEase(Ease.OutBack);

        // Update activeCandidates
        Destroy(oldCandidate, slideDuration + 0.05f);
        activeCandidates.Clear();
        activeCandidates.Add(newCandidate);

        // Update stats display
        UpdateStatsDisplay(newData);

        currentIndex = newIndex;
    }

    private void UpdateStatsDisplay(StaffData data)
    {
        if (nameText) nameText.text = data.staffName;
        if (descriptionText) descriptionText.text = data.description;
        if (stat1Text) stat1Text.text = $"Stat1: {data.stat1}";
        if (skillText) skillText.text = $"Skill: {data.skill}";
        if (costText) costText.text = $"Cost: {data.cost}";
    }
}
