using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ActionDatabase", menuName = "Campaign/Default Action Database")]
public class DefaultActionDatabase : ActionDatabase
{
    private void OnEnable()
    {
        if (actions.Count > 0) return;

        actions = new List<CampaignAction>
        {
            new CampaignAction
            {
                actionName = "Fundraiser",
                description = "Organize a fundraising event to generate campaign funds.",
                category = EffectCategory.Money,
                baseCost = 25000,
                baseEffect = 35000,
                successChance = 1f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Finance, type = ModifierType.Effect, value = 0.25f },
                    new StaffModifier { role = CampaignRole.Field, type = ModifierType.Effect, value = 0.05f },
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.Effect, value = 0.05f }
                }
            },

            new CampaignAction
            {
                actionName = "Corporate Luncheon",
                description = "Exclusive event for high donors; risky if PR is low.",
                category = EffectCategory.Money,
                baseCost = 20000,
                baseEffect = 20000,
                successChance = 0.9f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Finance, type = ModifierType.Effect, value = 0.15f },
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.SuccessChance, value = 0.05f }
                }
            },

            new CampaignAction
            {
                actionName = "Grassroots Drive",
                description = "Community donation drive to boost small-scale contributions.",
                category = EffectCategory.Money,
                baseCost = 10000,
                baseEffect = 12000,
                successChance = 1f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Field, type = ModifierType.Effect, value = 0.05f },
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.Effect, value = 0.05f }
                }
            },

            new CampaignAction
            {
                actionName = "Parade / Rally",
                description = "Public rally to boost visibility and voter support.",
                category = EffectCategory.VoterSway,
                baseCost = 30000,
                baseEffect = 0.04f, // 4% voter sway
                successChance = 0.95f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Field, type = ModifierType.Effect, value = 0.10f },
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.Effect, value = 0.15f }
                }
            },

            new CampaignAction
            {
                actionName = "Town Hall Meeting",
                description = "Local meeting to address issues and engage voters directly.",
                category = EffectCategory.VoterSway,
                baseCost = 15000,
                baseEffect = 0.03f,
                successChance = 1f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Field, type = ModifierType.Effect, value = 0.05f },
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.Effect, value = 0.02f }
                }
            },

            new CampaignAction
            {
                actionName = "Rumor Campaign",
                description = "Spread rumors to sway voters; risky but potentially effective.",
                category = EffectCategory.VoterSway,
                baseCost = 35000,
                baseEffect = 0.05f,
                successChance = 0.85f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.Effect, value = 0.15f }
                }
            },

            new CampaignAction
            {
                actionName = "Humanitarian Aid",
                description = "Donate to disaster victims to improve public perception.",
                category = EffectCategory.PR,
                baseCost = 25000,
                baseEffect = 0.04f, // 4% PR boost
                successChance = 1f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Field, type = ModifierType.Effect, value = 0.05f },
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.Effect, value = 0.10f }
                }
            },

            new CampaignAction
            {
                actionName = "Community Clean-Up",
                description = "Small local volunteer event to boost goodwill and PR.",
                category = EffectCategory.PR,
                baseCost = 10000,
                baseEffect = 0.02f,
                successChance = 1f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Field, type = ModifierType.Effect, value = 0.03f },
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.Effect, value = 0.05f }
                }
            },

            new CampaignAction
            {
                actionName = "Opposition Research",
                description = "Gather dirt on rivals; boosts next press event and efficiency.",
                category = EffectCategory.InternalPrep,
                baseCost = 25000,
                baseEffect = 0.02f, // 2% efficiency gain
                successChance = 0.95f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Finance, type = ModifierType.Cost, value = -0.1f },
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.Effect, value = 0.1f }
                }
            },

            new CampaignAction
            {
                actionName = "Internal Audit",
                description = "Review finances and prep internal operations; improves efficiency and budget.",
                category = EffectCategory.InternalPrep,
                baseCost = 0,
                baseEffect = 10000, // bonus money next week
                successChance = 1f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Finance, type = ModifierType.Effect, value = 0.15f }
                }
            }
        };
    }
}
