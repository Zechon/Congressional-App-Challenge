using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActionButtonUI : MonoBehaviour
{
    [Header("Name and Category")]
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text categoryText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image categoryBackground;

    [Header("Stats")]
    [SerializeField] private Image effectImg;
    [SerializeField] private Color effectColor;
    [SerializeField] private Image costImg;
    [SerializeField] private Color costColor;
    [SerializeField] private Image successImg;
    [SerializeField] private Color successColor;
    [SerializeField] private Image timeImg;
    [SerializeField] private Color timeColor;

    [Header("Stat Text")]
    [SerializeField] private TMP_Text effectTxt;
    [SerializeField] private TMP_Text costTxt;
    [SerializeField] private TMP_Text successTxt;
    [SerializeField] private TMP_Text timeTxt;

    [Header("Staff Frames")]
    [SerializeField] private Image finFrameImg;
    [SerializeField] private Color finFrameColor;
    [SerializeField] private Image fieldFrameImg;
    [SerializeField] private Color fieldFrameColor;
    [SerializeField] private Image comFrameImg;
    [SerializeField] private Color comFrameColor;

    private ActionDatabase.CampaignAction actionData;

    public void Setup(ActionDatabase.CampaignAction action)
    {
        actionData = action;

        // Basic info
        label.text = action.actionName;
        categoryText.text = $"[{action.category}]";
        descriptionText.text = action.description;
        categoryBackground.color = GetCategoryColor(action.category);

        // Apply staff modifiers (if any)
        var result = ActionModifierUtility.ApplyModifiers(action);

        // Cost (show strikethrough if changed)
        bool costChanged = Mathf.Abs(result.modifiedCost - action.baseCost) > 0.1f;
        string costText = costChanged
            ? $"<s>${action.baseCost:N0}</s> ${result.modifiedCost:N0}"
            : $"${action.baseCost:N0}";
        costTxt.text = "Cost: " + costText;
        costImg.color = costColor;

        // Effect text based on category
        string effectText = action.category switch
        {
            ActionDatabase.EffectCategory.Money => $"Effect: +${result.modifiedEffect:N0}",
            ActionDatabase.EffectCategory.VoterSway => $"Effect: +{result.modifiedEffect * 100f:0.#}% voters",
            ActionDatabase.EffectCategory.InternalPrep => $"Effect: +${result.modifiedEffect:N0} efficiency",
            ActionDatabase.EffectCategory.PR => $"Effect: +{result.modifiedEffect * 100f:0.#}% favor",
            _ => $"Effect: {result.modifiedEffect}"
        };
        effectTxt.text = effectText;
        effectImg.color = effectColor;

        // Success chance
        successTxt.text = $"Chance: {result.modifiedSuccessChance * 100f:0}%";
        successImg.color = successColor;

        // Duration (static)
        timeTxt.text = "Time: 1 Week";
        timeImg.color = timeColor;

        // Static staff frame colors (for visual grouping)
        finFrameImg.color = finFrameColor;
        fieldFrameImg.color = fieldFrameColor;
        comFrameImg.color = comFrameColor;
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
            ActionDatabase.EffectCategory.PR => new Color(1f, 0.6f, 0.3f),
            _ => Color.white
        };
    }
}
