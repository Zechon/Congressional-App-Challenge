using UnityEngine;

public enum PartyColor { Brown, Orange, Purple }

[DisallowMultipleComponent]
public class StateSetup : MonoBehaviour
{
    [Header("State Info")]
    public string stateName;
    public int stateVotes;
    public int economyLvl;

    [Header("Party Split % (0..1)")]
    [Range(0f, 1f)] public float orangePercent;
    [Range(0f, 1f)] public float purplePercent;

    [HideInInspector] public PartyColor currentColor;

    // Use 0.6f because percent is stored as 0..1
    public void CalculateStateColor()
    {
        if (orangePercent >= 0.6f) currentColor = PartyColor.Orange;
        else if (purplePercent >= 0.6f) currentColor = PartyColor.Purple;
        else currentColor = PartyColor.Brown;
    }

    // helpers
    public float OrangeRatioClamped() => Mathf.Clamp01(orangePercent);
    public float PurpleRatioClamped() => Mathf.Clamp01(purplePercent);
}
