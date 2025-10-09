using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionDatabase", menuName = "Campaign/ActionDatabase")]
public class ActionDatabase : ScriptableObject
{
    [System.Serializable]
    public class StaffModifier
    {
        public string staffName;           
        public ModifierType type;          
        public float value;                
    }

    public enum ModifierType { Cost, Effect, Success }

    [System.Serializable]
    public class CampaignAction
    {
        public string actionName;          
        public int baseCost;               
        public int baseEffect;             
        [Range(0f, 1f)] public float successChance; 
        public List<StaffModifier> modifiers = new List<StaffModifier>();
        [TextArea] public string description;
    }

    public List<CampaignAction> actions = new List<CampaignAction>();

    public CampaignAction GetAction(string name)
    {
        return actions.Find(a => a.actionName == name);
    }
}