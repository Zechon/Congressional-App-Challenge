using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhaseManager : MonoBehaviour
{
    public static GamePhaseManager instance;

    [Header("Game Settings")]
    public int totalWeeks = 4;
    public int currentWeek = 1;
    public int playerBudget = 100; // this is a placeholder budget amt, idk what to actually use yet.

    [Header("Animations")]
    public Animator anim;

    [Header("Tracking")]
    public List<PlannedAction> plannedActions = new List<PlannedAction>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            anim.SetInteger("Week", 1);
        }
        else Destroy(gameObject);
    }

    public void ConfirmAction(string stateName, string actionName, int cost)
    {
        if (playerBudget < cost)
        {
            Debug.LogWarning("Not Enough Budget!");
            return;
        }

        playerBudget -= cost;

        var action = new PlannedAction(stateName, actionName, cost, currentWeek);
        plannedActions.Add(action);

        Debug.Log($"[WEEK {currentWeek}] Planned {actionName} in {stateName}. (Cost: {cost})");

        if (currentWeek < totalWeeks)
        {
            currentWeek++;
            Debug.Log($"Now moving to Week {currentWeek}.");
        }
        else
        {
            EndPlanningPhase();
        }
    }

    private void EndPlanningPhase()
    {
        Debug.Log("All Weeks Planned! Moving to results.");
        // TODO: THE ACTUAL RESULTS PHASE
    }
}
