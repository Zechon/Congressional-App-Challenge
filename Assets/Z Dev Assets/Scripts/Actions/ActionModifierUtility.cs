using System.Collections.Generic;
using UnityEngine;

public static class ActionModifierUtility
{
    public class ModifierResult
    {
        public float modifiedCost;
        public float modifiedEffect;
        public float modifiedSuccessChance;
        public List<ActionDatabase.StaffModifier> appliedModifiers = new();
    }

    public static ModifierResult ApplyModifiers(ActionDatabase.CampaignAction action)
    {
        ModifierResult result = new()
        {
            modifiedCost = action.baseCost,
            modifiedEffect = action.baseEffect,
            modifiedSuccessChance = action.successChance
        };

        foreach (var mod in action.modifiers)
        {
            switch (mod.type)
            {
                case ActionDatabase.ModifierType.Cost:
                    result.modifiedCost += action.baseCost * mod.value;
                    break;
                case ActionDatabase.ModifierType.Effect:
                    result.modifiedEffect += action.baseEffect * mod.value;
                    break;
                case ActionDatabase.ModifierType.SuccessChance:
                    result.modifiedSuccessChance += action.successChance * mod.value;
                    break;
            }

            result.appliedModifiers.Add(mod);
        }

        return result;
    }
}
