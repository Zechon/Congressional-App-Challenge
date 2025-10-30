public class PlannedAction
{
    public string stateName;
    public string actionName;
    public int cost;
    public int week;

    public float effect; 

    public PlannedAction(string stateName, string actionName, int cost, int week, float effect = 0f)
    {
        this.stateName = stateName;
        this.actionName = actionName;
        this.cost = cost;
        this.week = week;
        this.effect = effect;
    }
}
