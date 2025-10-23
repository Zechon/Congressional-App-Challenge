using UnityEngine;
using UnityEngine.UI;

public enum PartyColor { Brown, Orange, Purple }

[DisallowMultipleComponent]
public class StateSetup : MonoBehaviour
{
    Color orange = new Color(1.0f, 0.64f, 0.0f, 1.0f);
    Color purple = new Color(0.627f, 0.125f, 0.941f, 1.0f);
    Color brown = new Color(0.588f, 0.294f, 0.0f);

    [Header("State Info")]
    public string stateName;
    public int stateVotes;
    public int economyLvl;

    [Header("Party Split % (0..1)")]
    [Range(0f, 1f)] public float orangePercent;
    [Range(0f, 1f)] public float purplePercent;

    [Header("Nearby States")]
    public GameObject near1;
    public GameObject near2;
    public GameObject near3;
    public GameObject near4;
    public GameObject near5;
    public GameObject near6;
    public GameObject near7;
    public GameObject near8;

    [Header("Other Setup")]
    public Image spr;

    [HideInInspector] public PartyColor currentColor;

    public void Start()
    {
        spr = GetComponent<Image>();
    }

    public void CalculateStateColor()
    {
        if (orangePercent >= 0.6f)
        {
            spr.color = orange;
            currentColor = PartyColor.Orange;
        }
        else if (purplePercent >= 0.6f)
        {
            spr.color = purple;
            currentColor = PartyColor.Purple;
        }
        else
        {
            spr.color = brown;
            currentColor = PartyColor.Brown;
        }
    }

    public float OrangeRatioClamped() => Mathf.Clamp01(orangePercent);
    public float PurpleRatioClamped() => Mathf.Clamp01(purplePercent);
}
