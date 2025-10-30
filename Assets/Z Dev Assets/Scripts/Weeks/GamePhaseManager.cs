using System.Collections.Generic;
using UnityEngine;

public class GamePhaseManager : MonoBehaviour
{
    public static GamePhaseManager instance;

    [Header("Game Settings")]
    public int totalWeeks = 3;   // 3 weeks of actions
    public int totalMonths = 8;

    [Header("Runtime Tracking")]
    public int currentWeek = 1;
    public int currentMonth = 1;
    public int playerBudget = 100;

    [Header("Animations / UI")]
    public Animator anim;

    [Header("Tracking")]
    public List<PlannedAction> plannedActions = new();

    private bool inResultsPhase = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        anim?.SetInteger("Week", currentWeek);
    }

    public void ConfirmAction(string stateName, string actionName, int cost)
    {
        if (inResultsPhase) return;

        if (playerBudget < cost)
        {
            Debug.LogWarning("Not enough budget!");
            return;
        }

        playerBudget -= cost;
        plannedActions.Add(new PlannedAction(stateName, actionName, cost, currentWeek, 0.1f));
        Debug.Log($"[Month {currentMonth} | Week {currentWeek}] Planned {actionName} in {stateName}.");

        if (currentWeek < totalWeeks)
        {
            currentWeek++;
            anim?.SetInteger("Week", currentWeek);
        }
        else
        {
            // After week 3
            PressConferenceManager.instance?.StartConference();
            EndPlanningPhase();
        }
    }

    void EndPlanningPhase()
    {
        Debug.Log($"Month {currentMonth} complete! Showing results.");
        inResultsPhase = true;
        ResultsPhaseUI.instance?.Show(plannedActions, OnResultsComplete);
    }

    void OnResultsComplete()
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
            Debug.Log("All months complete — final election results phase!");
            // TODO: trigger final election results UI
        }
    }
}
