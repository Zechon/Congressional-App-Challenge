using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStaff", menuName = "Campaign/Staff")]
public class StaffData : ScriptableObject
{
    [Header("Stats")]
    public CampaignRole role; // Finance, Field, Communications
    public string staffName;
    [TextArea] public string description;
    [Range(1, 5)] public int stat1;  // Connections / Message Control / Motivation
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

    [Header("Temp")]
    [HideInInspector] public Sprite tempShirt;
    [HideInInspector] public Sprite tempHat;
    [HideInInspector] public Sprite tempMisc1;
    [HideInInspector] public Sprite tempMisc2;

    [HideInInspector] public Color tempSkin;
    [HideInInspector] public Color tempShirtColor;
    [HideInInspector] public Color tempHatColor;
    [HideInInspector] public Color tempMisc1Color;
    [HideInInspector] public Color tempMisc2Color;

    [HideInInspector] public bool hasTempLook = false;


    private enum ColorSchemeType { Monochrome, Complementary, Analogous, Triadic, Random }

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
        // Only assign skin if not already assigned
        if (assignedSkin == default)
            assignedSkin = GetRandomSkinColor();

        // Only assign clothing colors if none assigned yet
        if (assignedShirt == default && assignedHat == default && assignedMisc1 == default && assignedMisc2 == default)
        {
            Color baseColor = GetBaseColorForCategory(role);
            ColorSchemeType scheme = PickRandomScheme();

            switch (scheme)
            {
                case ColorSchemeType.Monochrome:
                    assignedShirt = baseColor;
                    assignedHat = TintOrShadeForClothingType(baseColor, 0.85f);   // <-- used here
                    assignedMisc1 = TintOrShadeForClothingType(baseColor, 0.7f);  // <-- used here
                    assignedMisc2 = TintOrShadeForClothingType(baseColor, 0.5f);  // <-- misc2 darker
                    break;

                case ColorSchemeType.Complementary:
                    assignedShirt = baseColor;
                    assignedHat = TintOrShadeForClothingType(ShiftHue(baseColor, 0.08f), 0.9f);
                    assignedMisc1 = TintOrShadeForClothingType(ShiftHue(baseColor, -0.08f), 1.05f);
                    assignedMisc2 = TintOrShadeForClothingType(Shade(ShiftHue(baseColor, 0.5f), 0.5f), 0.5f);
                    break;

                case ColorSchemeType.Analogous:
                    assignedShirt = baseColor;
                    assignedHat = TintOrShadeForClothingType(ShiftHue(baseColor, 0.08f), 0.9f);
                    assignedMisc1 = TintOrShadeForClothingType(ShiftHue(baseColor, 0.16f), 1.05f);
                    assignedMisc2 = TintOrShadeForClothingType(Shade(ShiftHue(baseColor, 0.24f), 0.5f), 0.5f);
                    break;

                case ColorSchemeType.Triadic:
                    assignedShirt = baseColor;
                    assignedHat = TintOrShadeForClothingType(ShiftHue(baseColor, 0.33f), 0.9f);
                    assignedMisc1 = TintOrShadeForClothingType(ShiftHue(baseColor, 0.66f), 1.05f);
                    assignedMisc2 = TintOrShadeForClothingType(Shade(ShiftHue(baseColor, 0.5f), 0.5f), 0.5f);
                    break;

                case ColorSchemeType.Random:
                    assignedShirt = GetRandomClothingColor();
                    assignedHat = TintOrShadeForClothingType(GetRandomClothingColor(), 0.9f);
                    assignedMisc1 = TintOrShadeForClothingType(GetRandomClothingColor(), 1.05f);
                    assignedMisc2 = TintOrShadeForClothingType(Shade(GetRandomClothingColor(), 0.5f), 0.5f);
                    break;
            }
        }
    }


    private Color GetBaseColorForCategory(CampaignRole role)
    {
        switch (role)
        {
            case CampaignRole.Finance:
                return Random.ColorHSV(0f, 1f, 0.4f, 0.7f, 0.5f, 0.8f); // muted colors
            case CampaignRole.Field:
                return Random.ColorHSV(0f, 1f, 0.6f, 0.9f, 0.6f, 0.9f); // brighter
            case CampaignRole.Communications:
                return Random.ColorHSV(0f, 1f, 0.5f, 0.8f, 0.5f, 0.85f);
            default:
                return Random.ColorHSV(0f, 1f, 0.5f, 0.8f, 0.5f, 0.85f);
        }
    }

    private Color GetRandomClothingColor()
    {
        // Use a moderate saturation and brightness for realistic clothes
        return Random.ColorHSV(0f, 1f, 0.4f, 0.9f, 0.4f, 0.9f);
    }

    private Color ShiftHue(Color original, float shiftAmount)
    {
        Color.RGBToHSV(original, out float h, out float s, out float v);
        h = Mathf.Repeat(h + shiftAmount, 1f);

        // Clamp s/v to realistic range
        s = Mathf.Clamp(s, 0.3f, 0.9f);
        v = Mathf.Clamp(v, 0.3f, 0.9f);
        return Color.HSVToRGB(h, s, v);
    }

    private Color Shade(Color original, float factor)
    {
        return new Color(
            Mathf.Clamp01(original.r * factor),
            Mathf.Clamp01(original.g * factor),
            Mathf.Clamp01(original.b * factor)
        );
    }

    private Color Tint(Color original, float factor)
    {
        return new Color(
            Mathf.Clamp01(original.r * factor + (1 - factor)),
            Mathf.Clamp01(original.g * factor + (1 - factor)),
            Mathf.Clamp01(original.b * factor + (1 - factor))
        );
    }


    private ColorSchemeType PickRandomScheme()
    {
        float roll = Random.value;
        if (roll < 0.4f) return ColorSchemeType.Monochrome;
        else if (roll < 0.65f) return ColorSchemeType.Complementary;
        else if (roll < 0.85f) return ColorSchemeType.Analogous;
        else return ColorSchemeType.Triadic;
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

    private Color TintOrShadeForClothingType(Color color, float factor)
    {
        return factor > 1f ? Tint(color, factor) : Shade(color, factor);
    }

    public void ApplyTempLook()
    {
        if (!hasTempLook)
        {
            tempShirt = selectedShirt;
            tempHat = selectedHat;
            tempMisc1 = selectedMisc1;
            tempMisc2 = selectedMisc2;

            tempShirtColor = assignedShirt;
            tempHatColor = assignedHat;
            tempMisc1Color = assignedMisc1;
            tempMisc2Color = assignedMisc2;
            tempSkin = assignedSkin;

            hasTempLook = true;
        }
    }

    // Reset temp values (e.g., on scene reload)
    public void ResetTempLook()
    {
        tempShirt = null;
        tempHat = null;
        tempMisc1 = null;
        tempMisc2 = null;

        tempShirtColor = default;
        tempHatColor = default;
        tempMisc1Color = default;
        tempMisc2Color = default;
        tempSkin = default;

        hasTempLook = false;
    }

}
