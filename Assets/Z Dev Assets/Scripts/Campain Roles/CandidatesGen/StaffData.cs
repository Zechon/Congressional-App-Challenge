using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

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

    public void AssignColorsIfNeeded()
    {
        // Assign skin tone if unassigned
        if (assignedSkin == default)
            assignedSkin = GetRandomSkinColor();

        // Base clothing color for harmony
        if (assignedShirt == default && assignedHat == default && assignedMisc1 == default && assignedMisc2 == default)
        {
            Color baseColor = Random.ColorHSV(0f, 1f, 0.6f, 0.9f, 0.7f, 1f);

            assignedShirt = baseColor;
            assignedHat = ShiftHue(baseColor, Random.Range(-0.1f, 0.1f));
            assignedMisc1 = ShiftHue(baseColor, Random.Range(0.2f, 0.4f));
            assignedMisc2 = ShiftHue(baseColor, Random.Range(0.4f, 0.6f));
        }
    }

    private Color GetRandomSkinColor()
    {
        Color[] skinTones = new Color[]
        {
            new Color(0.98f, 0.8f, 0.6f),
            new Color(0.9f, 0.7f, 0.5f),
            new Color(0.75f, 0.55f, 0.35f),
            new Color(0.6f, 0.4f, 0.25f),
            new Color(0.4f, 0.25f, 0.15f)
        };
        return skinTones[Random.Range(0, skinTones.Length)];
    }

    private Color ShiftHue(Color original, float shiftAmount)
    {
        Color.RGBToHSV(original, out float h, out float s, out float v);
        h = Mathf.Repeat(h + shiftAmount, 1f);
        return Color.HSVToRGB(h, s, v);
    }
}