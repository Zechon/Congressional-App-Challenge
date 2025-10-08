using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    private string actionName;
    private int actionCost;

    public void Setup(string name, int cost)
    {
        actionName = name;
        actionCost = cost;
        label.text = cost > 0 ? $"{name} (${cost})" : name;
    }

    public void OnClick()
    {
        ActionPanel.instance.OnSelectAction(actionName, actionCost);
    }
}
