using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
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

    [Header("Staff")]
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

        categoryText.text = $"[{action.category}]";
        categoryBackground.color = GetCategoryColor(action.category);
        descriptionText.text = action.description;

        var result = ActionModifierUtility.ApplyModifiers(action);

        bool costChanged = Mathf.Abs(result.modifiedCost - action.baseCost) > 0.1f;

        string costText = costChanged
            ? $"<s>${action.baseCost}K</s> ${result.modifiedCost:0}K"
            : $"${action.baseCost}K";

        costTxt.text = "Cost: " + costText;
        costImg.color = costColor;
        effectTxt.text = $"Effect: {result.modifiedEffect:+0;-0}K";
        effectImg.color = effectColor;
        successTxt.text = $"Chance: {result.modifiedSuccessChance * 100f:0}%";
        successImg.color = successColor;
        timeTxt.text = "Time Needed: —";
        timeImg.color = timeColor;
        finFrameImg.color = finFrameColor;
        fieldFrameImg.color = fieldFrameColor;
        comFrameImg.color = comFrameColor;


        label.text = action.actionName;
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
            ActionDatabase.EffectCategory.Logistics => new Color(0.3f, 0.9f, 0.5f),
            _ => Color.white
        };
    }
}