using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStaff", menuName = "Campaign/Staff")]
public class StaffData : ScriptableObject
{
    public string staffName;
    public CampaignRole role;
    [Range(1, 5)] public int stat1;
    [Range(1, 5)] public int skill;
    public int cost;
    [TextArea] public string description;
}