using UnityEngine;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text modifiersText;

    private string actionName;
    private int actionCost;

    public void Setup(string name, int cost, string modifiers = "")
    {
        actionName = name;
        actionCost = cost;
        label.text = cost > 0 ? $"{name} (${cost})" : name;
        modifiersText.text = modifiers;
    }

    public void OnClick()
    {
        ActionPanelUI.instance.OnSelectAction(actionName, actionCost);
    }
}
