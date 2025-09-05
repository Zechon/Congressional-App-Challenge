using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStaff", menuName = "Campaign/Staff")]
public class StaffData : ScriptableObject
{
    [Header("Stats")]
    public CampaignRole role;
    public string staffName;
    [TextArea] public string description;
    [Range(1, 5)] public int stat1;
    [Range(1, 5)] public int skill;
    public int cost;

    [Header("Prefab")]
    public GameObject mannequinPrefab;

    [Header("Clothing Options")]
    public Sprite[] shirtOptions;
    public Sprite[] hatOptions;
    public Sprite[] misc1Options;
    public Sprite[] misc2Options;

    [Header("Runtime Clothing")]
    [HideInInspector] public Sprite selectedShirt;
    [HideInInspector] public Sprite selectedHat;
    [HideInInspector] public Sprite selectedMisc1;
    [HideInInspector] public Sprite selectedMisc2;

    [HideInInspector] public Color assignedSkin;
    [HideInInspector] public Color assignedShirt;
    [HideInInspector] public Color assignedHat;
    [HideInInspector] public Color assignedMisc1;
    [HideInInspector] public Color assignedMisc2;

    public void AssignClothingIfNeeded()
    {
        if (selectedShirt == null && shirtOptions.Length > 0)
            selectedShirt = shirtOptions[Random.Range(0, shirtOptions.Length)];

        if (selectedMisc1 == null && misc1Options.Length > 0)
            selectedMisc1 = misc1Options[Random.Range(0, misc1Options.Length)];

        if (selectedMisc2 == null && misc2Options.Length > 0)
            selectedMisc2 = misc2Options[Random.Range(0, misc2Options.Length)];

        if (selectedHat == null && hatOptions.Length > 0)
            selectedHat = hatOptions[Random.Range(0, hatOptions.Length)];
    }
}