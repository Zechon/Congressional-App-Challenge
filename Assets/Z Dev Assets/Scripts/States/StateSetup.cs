using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartyColor { Brown, Orange, Purple }

public class StateSetup : MonoBehaviour
{
    [Header("State Info")]
    public string stateName;
    public int stateVotes;
    public int economyLvl;

    [Header("Party Split %")]
    [Range(0, 1)] public float orangePercent;
    [Range(0, 1)] public float purplePercent;

    [HideInInspector] public PartyColor currentColor;

    public void CalculateStateColor()
    {
        if (orangePercent >= 60) { currentColor = PartyColor.Orange; }
        else if (purplePercent >= 60) { currentColor = PartyColor.Purple; }
        else { currentColor = PartyColor.Brown; }
    }
}
