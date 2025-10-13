using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text categoryText;
    [SerializeField] private Image categoryBackground;

    private ActionDatabase.CampaignAction actionData;

    public void Setup(ActionDatabase.CampaignAction action)
    {
        actionData = action;

        categoryText.text = $"[{action.category}]";
        categoryBackground.color = GetCategoryColor(action.category);

        float modifiedCost = action.baseCost;
        StringBuilder modifierPreview = new();

        foreach (var mod in action.modifiers)
        {
            if (mod.type == ActionDatabase.ModifierType.Cost)
            {
                float costChange = action.baseCost * mod.value;
                modifiedCost += costChange;
                string sign = mod.value < 0 ? "-" : "+";
                modifierPreview.AppendLine(
                    $"({sign}{Mathf.Abs(mod.value * 100f):0}% Cost | {mod.role})"
                );
            }
        }

        string costText = action.modifiers.Count > 0
            ? $"<s>${action.baseCost}K</s> ${modifiedCost:0}K"
            : $"${action.baseCost}K";

        label.text = $"{action.actionName}\n<size=80%>{costText}</size>\n<color=#aaaaaa>{modifierPreview}</color>";
    }

    public void OnClick()
    {
        if (ActionPanelUI.instance != null)
        {
            int finalCost = Mathf.RoundToInt(actionData.baseCost);
            ActionPanelUI.instance.OnSelectAction(actionData.actionName, finalCost);
        }
    }

    private Color GetCategoryColor(ActionDatabase.EffectCategory category)
    {
        return category switch
        {
            ActionDatabase.EffectCategory.Money => new Color(0.9f, 0.8f, 0.2f),
            ActionDatabase.EffectCategory.VoterSway => new Color(0.4f, 0.8f, 1f),
            ActionDatabase.EffectCategory.InternalPrep => new Color(0.7f, 0.5f, 1f),
            ActionDatabase.EffectCategory.PR_Morale => new Color(1f, 0.6f, 0.3f),
            ActionDatabase.EffectCategory.Logistics => new Color(0.3f, 0.9f, 0.5f),
            _ => Color.white
        };
    }
}
