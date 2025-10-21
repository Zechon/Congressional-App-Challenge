using System.Collections.Generic;
using UnityEngine;

public class GamePhaseManager : MonoBehaviour
{
    public static GamePhaseManager instance;

    [Header("Game Settings")]
    public int totalWeeks = 4;
    public int totalMonths = 8;

    [Header("Runtime Tracking")]
    public int currentWeek = 1;
    public int currentMonth = 1;
    public int playerBudget = 100;

    [Header("Animations / UI")]
    public Animator anim;

    [Header("Tracking")]
    public List<PlannedAction> plannedActions = new List<PlannedAction>();

    private bool inResultsPhase = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (anim != null)
                anim.SetInteger("Week", currentWeek);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ConfirmAction(string stateName, string actionName, int cost)
    {
        if (inResultsPhase)
        {
            Debug.LogWarning("Cannot plan actions during Results Phase!");
            return;
        }

        if (playerBudget < cost)
        {
            Debug.LogWarning("Not Enough Budget!");
            return;
        }

        playerBudget -= cost;

        var action = new PlannedAction(stateName, actionName, cost, currentWeek);
        plannedActions.Add(action);

        Debug.Log($"[Month {currentMonth} | Week {currentWeek}] Planned {actionName} in {stateName}. (Cost: {cost})");

        // Move to next week or phase
        if (currentWeek < totalWeeks)
        {
            currentWeek++;
            anim?.SetInteger("Week", currentWeek);

            if (currentWeek == 4)
            {
                Debug.Log("Triggering Press Conference!");
                PressConferenceManager.instance?.StartConference();
            }
        }
        else
        {
            EndPlanningPhase();
        }
    }

    private void EndPlanningPhase()
    {
        Debug.Log($"Month {currentMonth} complete! Moving to results slideshow.");
        inResultsPhase = true;
        ResultsPhaseUI.instance?.Show(plannedActions, OnResultsComplete);
    }

    private void OnResultsComplete()
    {
        inResultsPhase = false;
        plannedActions.Clear();

        if (currentMonth < totalMonths)
        {
            currentMonth++;
            currentWeek = 1;
            anim?.SetInteger("Week", currentWeek);
            Debug.Log($"--- Beginning Month {currentMonth} ---");
        }
        else
        {
            Debug.Log("All 8 months complete — election results phase!");
            // TODO: final election results phase
        }
    }
}
