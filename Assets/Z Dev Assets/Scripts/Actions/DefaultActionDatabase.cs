using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ActionDatabase", menuName = "Campaign/Default Balanced Action Database")]
public class DefaultActionDatabase : ActionDatabase
{
    private void OnEnable()
    {
        if (actions.Count > 0) return;

        actions = new List<CampaignAction>
        {
            // --- MONEY ---
            new CampaignAction
            {
                actionName = "Local Fundraiser",
                description = "Host a small, reliable fundraising event.",
                category = EffectCategory.Money,
                baseCost = 10000,
                baseEffect = 15000,
                successChance = 1f, // guaranteed
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Finance, type = ModifierType.Effect, value = 0.1f }
                }
            },
            new CampaignAction
            {
                actionName = "Corporate Dinner",
                description = "High-end donor event with larger returns but riskier optics.",
                category = EffectCategory.Money,
                baseCost = 20000,
                baseEffect = 35000,
                successChance = 0.8f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.SuccessChance, value = 0.05f },
                    new StaffModifier { role = CampaignRole.Finance, type = ModifierType.Effect, value = 0.15f }
                }
            },

            // --- VOTER SWAY ---
            new CampaignAction
            {
                actionName = "Neighborhood Meetup",
                description = "Hold a small, friendly event to meet voters.",
                category = EffectCategory.VoterSway,
                baseCost = 12000,
                baseEffect = 0.02f, // 2% sway
                successChance = 1f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Field, type = ModifierType.Effect, value = 0.1f }
                }
            },
            new CampaignAction
            {
                actionName = "City Rally",
                description = "Large public rally with big potential turnout but higher cost and risk.",
                category = EffectCategory.VoterSway,
                baseCost = 25000,
                baseEffect = 0.05f, // 5% sway
                successChance = 0.85f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.SuccessChance, value = 0.05f },
                    new StaffModifier { role = CampaignRole.Field, type = ModifierType.Effect, value = 0.15f }
                }
            },

            // --- PUBLIC RELATIONS ---
            new CampaignAction
            {
                actionName = "Community Clean-Up",
                description = "Volunteer event to show goodwill and earn small PR gains.",
                category = EffectCategory.PR,
                baseCost = 8000,
                baseEffect = 0.015f, // 1.5% PR boost
                successChance = 1f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Field, type = ModifierType.Effect, value = 0.05f }
                }
            },
            new CampaignAction
            {
                actionName = "Charity Gala",
                description = "Sponsor a public charity event; impressive but expensive and can backfire.",
                category = EffectCategory.PR,
                baseCost = 25000,
                baseEffect = 0.04f, // 4% PR boost
                successChance = 0.85f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.SuccessChance, value = 0.05f },
                    new StaffModifier { role = CampaignRole.Finance, type = ModifierType.Cost, value = -0.1f }
                }
            },

            // --- INTERNAL / EFFICIENCY ---
            new CampaignAction
            {
                actionName = "Team Workshop",
                description = "Basic team training session for small internal improvements.",
                category = EffectCategory.InternalPrep,
                baseCost = 5000,
                baseEffect = 5000,
                successChance = 1f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Finance, type = ModifierType.Effect, value = 0.1f }
                }
            },
            new CampaignAction
            {
                actionName = "Strategic Audit",
                description = "Comprehensive internal review that could lead to major efficiency gains.",
                category = EffectCategory.InternalPrep,
                baseCost = 10000,
                baseEffect = 15000,
                successChance = 0.8f,
                modifiers = new List<StaffModifier>
                {
                    new StaffModifier { role = CampaignRole.Finance, type = ModifierType.Effect, value = 0.2f },
                    new StaffModifier { role = CampaignRole.Communications, type = ModifierType.SuccessChance, value = 0.05f }
                }
            }
        };
    }
}
