using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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

    private Queue<PlannedAction> queuedResults = new();
    private System.Action onComplete;

    private void Awake()
    {
        instance = this;
        if (resultsPanel != null)
            resultsPanel.SetActive(false);
        nextButton.onClick.AddListener(ShowNextResult);
    }

    public void Show(List<PlannedAction> actions, System.Action onFinished)
    {
        if (actions.Count == 0)
        {
            onFinished?.Invoke();
            return;
        }

        onComplete = onFinished;
        queuedResults.Clear();
        foreach (var act in actions)
            queuedResults.Enqueue(act);

        resultsPanel.SetActive(true);
        ShowNextResult();
    }

    private void ShowNextResult()
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
            Color targetColor = state.spr.color;

            if (GameData.Party == "Orange")
            {
                state.orangePercent = Mathf.Clamp01(state.orangePercent + act.effect);
                state.purplePercent = 1f - state.orangePercent;

                // Update target color
                targetColor = state.orangePercent >= 0.6f ? new Color(1f, 0.64f, 0f) : new Color(0.588f, 0.294f, 0f);
            }
            else // Purple
            {
                state.purplePercent = Mathf.Clamp01(state.purplePercent + act.effect);
                state.orangePercent = 1f - state.purplePercent;

                targetColor = state.purplePercent >= 0.6f ? new Color(0.627f, 0.125f, 0.941f) : new Color(0.588f, 0.294f, 0f);
            }

            // Tween the color over 0.5 seconds for a smooth slideshow effect
            state.spr.DOColor(targetColor, 0.5f);
        }

        Debug.Log($"Applied results for {act.actionName} in {act.stateName}");
    }
}
