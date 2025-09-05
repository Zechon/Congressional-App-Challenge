using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CampaignRole { Finance, Communications, Field }

[System.Serializable]
public class CampaignStaff 
{
    public string staffName;
    public CampaignRole role;
    public int stat1; // "Connections", "Message Control", or "Motivation"
    public int skill;
    public int cost;
    public string description;
}
