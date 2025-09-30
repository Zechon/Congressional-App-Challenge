[System.Serializable]
public class WeekSlot
{
    public string weekName;   // "Week 1", "Week 2", "Week 3", "Press Conference"
    public bool isLocked;     // true for the Press Conference
    public StateSetup assignedState;
    public string assignedAction;
}