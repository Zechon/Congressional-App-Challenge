using UnityEngine;

public class PlannedAction
{
    public string stateName;
    public string actionName;
    public int cost;
    public int week;
    public float effect;

    public PlannedAction(string state, string action, int cost, int week, float effect = 0.1f)
    {
        stateName = state;
        actionName = action;
        this.cost = cost;
        this.week = week;
        this.effect = effect;
    }
}
