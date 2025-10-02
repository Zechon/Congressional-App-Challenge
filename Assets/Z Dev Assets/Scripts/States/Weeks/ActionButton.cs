using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButton : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    private string actionName;
    private int actionCost;

    public void Setup(string name, int cost)
    {
        actionName = name;
        actionCost = cost;
        label.text = $"{name} ({cost})";
    }

    public void OnClick()
    {
        ActionPanel.instance.OnSelectAction(actionName, actionCost);
    }
}
