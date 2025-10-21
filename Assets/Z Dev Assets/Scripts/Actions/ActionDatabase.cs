using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionDatabase", menuName = "Campaign/Action Database")]
public class ActionDatabase : ScriptableObject
{

    [System.Serializable]
    public class StaffModifier
    {
        public string staffName;          
        public CampaignRole role;          
        public ModifierType type;          
        public float value;                
    }

    public enum ModifierType
    {
        Cost,
        Effect,
        SuccessChance
    }

    public enum EffectCategory
    {
        Money,          // Fundraising or finance gain
        VoterSway,      // Shifts votes or public support
        InternalPrep,   // Internal buffs, strategic preparation
        PR,             // Public relations or media impact
        Logistics,      // Improves efficiency or cost reductions
    }

    [System.Serializable]
    public class CampaignAction
    {
        [Header("General Info")]
        public string actionName;
        [TextArea] public string description;
        public EffectCategory category;

        [Header("Base Values")]
        public int baseCost;
        public float baseEffect;
        [Range(0f, 1f)] public float successChance = 1f;

        [Header("Staff Modifiers")]
        public List<StaffModifier> modifiers = new List<StaffModifier>();
    }

    public List<CampaignAction> actions = new List<CampaignAction>();

    public CampaignAction GetAction(string name)
    {
        return actions.Find(a => a.actionName == name);
    }

    public List<CampaignAction> GetActionsByCategory(EffectCategory category)
    {
        return actions.FindAll(a => a.category == category);
    }

    public List<CampaignAction> GetAllActions()
    {
        return new List<CampaignAction>(actions);
    }
}
