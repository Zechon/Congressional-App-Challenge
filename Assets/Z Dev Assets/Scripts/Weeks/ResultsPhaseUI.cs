using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using DG.Tweening;

public class ResultsPhaseUI : MonoBehaviour
{
    public static ResultsPhaseUI instance;

    [Header("UI References")]
    public GameObject resultsPanel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Button nextButton;

    [Header("Stuff to Disable")]
    public GameObject ActionPanel;
    public GameObject InfoPanel;

    private Queue<PlannedAction> queuedResults = new();
    private System.Action onComplete;

    void Awake()
    {
        instance = this;
        resultsPanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextResult);
    }

    public void Show(List<PlannedAction> actions, System.Action onFinished)
    {
        ActionPanel.SetActive(false);
        InfoPanel.SetActive(false);

        if (actions.Count == 0)
        {
            onFinished?.Invoke();
            return;
        }

        queuedResults.Clear();
        foreach (var act in actions) queuedResults.Enqueue(act);

        resultsPanel.SetActive(true);
        onComplete = onFinished;
        ShowNextResult();
    }

    void ShowNextResult()
    {
        if (queuedResults.Count == 0)
        {
            resultsPanel.SetActive(false);
            onComplete?.Invoke();
            return;
        }

        var act = queuedResults.Dequeue();

        titleText.text = $"{act.actionName} — {act.stateName}";
        descriptionText.text = $"Spent ${act.cost}K during Week {act.week}.";

        StateSetup state = StateUIManager.Instance.MapLayer
            .GetComponentsInChildren<StateSetup>()
            .FirstOrDefault(s => s.stateName == act.stateName);

        if (state != null)
        {
            if (GameData.Party == "Orange")
            {
                state.orangePercent = Mathf.Clamp01(state.orangePercent + act.effect);
                state.purplePercent = 1f - state.orangePercent;
            }
            else
            {
                state.purplePercent = Mathf.Clamp01(state.purplePercent + act.effect);
                state.orangePercent = 1f - state.purplePercent;
            }

            state.CalculateStateColor();
        }

        Debug.Log($"Applied results for {act.actionName} in {act.stateName}");
    }
}
