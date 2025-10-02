using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannedAction
{
    public string stateName;
    public string actionName;
    public int cost;
    public int weekNumber;

    public PlannedAction(string stateName, string actionName, int cost, int weekNumber)
    {
        this.stateName = stateName;
        this.actionName = actionName;
        this.cost = cost;
        this.weekNumber = weekNumber;
    }
}
