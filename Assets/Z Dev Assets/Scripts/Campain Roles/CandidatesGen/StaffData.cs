using System.Collections;
using System.Collections.Generic;
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

    public void AssignClothingIfNeeded(System.Random rng)
    {
        if (shirtOptions != null && selectedShirt == null && shirtOptions.Length > 0)
            selectedShirt = shirtOptions[rng.Next(shirtOptions.Length)];

        if (misc1Options != null && selectedMisc1 == null && misc1Options.Length > 0)
            selectedMisc1 = misc1Options[rng.Next(misc1Options.Length)];

        if (misc2Options != null && selectedMisc2 == null && misc2Options.Length > 0)
            selectedMisc2 = misc2Options[rng.Next(misc2Options.Length)];

        if (hatOptions != null && selectedHat == null && hatOptions.Length > 0)
            selectedHat = hatOptions[rng.Next(hatOptions.Length)];
    }

    public void AssignColorsIfNeeded(System.Random rng)
    {
        if (assignedSkin == default)
            assignedSkin = GetRandomSkinColor(rng);

        if (assignedShirt == default && assignedHat == default && assignedMisc1 == default && assignedMisc2 == default)
        {
            Color baseColor = GetBaseColorForCategory(role, rng);
            ColorSchemeType scheme = PickRandomScheme(rng);

            switch (scheme)
            {
                case ColorSchemeType.Monochrome:
                    assignedShirt = baseColor;
                    assignedHat = TintOrShadeForClothingType(baseColor, 0.85f);
                    assignedMisc1 = TintOrShadeForClothingType(baseColor, 0.7f);
                    assignedMisc2 = TintOrShadeForClothingType(baseColor, 0.5f);
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
                    assignedShirt = GetRandomClothingColor(rng);
                    assignedHat = TintOrShadeForClothingType(GetRandomClothingColor(rng), 0.9f);
                    assignedMisc1 = TintOrShadeForClothingType(GetRandomClothingColor(rng), 1.05f);
                    assignedMisc2 = TintOrShadeForClothingType(Shade(GetRandomClothingColor(rng), 0.5f), 0.5f);
                    break;
            }
        }
    }

    private Color GetBaseColorForCategory(CampaignRole role, System.Random rng)
    {
        float h = (float)rng.NextDouble();
        float s, v;

        switch (role)
        {
            case CampaignRole.Finance:
                s = 0.4f + (float)rng.NextDouble() * 0.3f;
                v = 0.5f + (float)rng.NextDouble() * 0.3f;
                break;
            case CampaignRole.Field:
                s = 0.6f + (float)rng.NextDouble() * 0.3f;
                v = 0.6f + (float)rng.NextDouble() * 0.3f;
                break;
            case CampaignRole.Communications:
                s = 0.5f + (float)rng.NextDouble() * 0.3f;
                v = 0.5f + (float)rng.NextDouble() * 0.35f;
                break;
            default:
                s = 0.5f + (float)rng.NextDouble() * 0.3f;
                v = 0.5f + (float)rng.NextDouble() * 0.35f;
                break;
        }

        return Color.HSVToRGB(h, s, v);
    }

    private Color GetRandomClothingColor(System.Random rng)
    {
        float h = (float)rng.NextDouble();
        float s = 0.4f + (float)rng.NextDouble() * 0.5f;
        float v = 0.4f + (float)rng.NextDouble() * 0.5f;
        return Color.HSVToRGB(h, s, v);
    }

    private ColorSchemeType PickRandomScheme(System.Random rng)
    {
        float roll = (float)rng.NextDouble();
        if (roll < 0.4f) return ColorSchemeType.Monochrome;
        else if (roll < 0.65f) return ColorSchemeType.Complementary;
        else if (roll < 0.85f) return ColorSchemeType.Analogous;
        else return ColorSchemeType.Triadic;
    }

    private Color GetRandomSkinColor(System.Random rng)
    {
        Color[] skinTones = new Color[]
        {
            new Color(0.98f, 0.8f, 0.6f),
            new Color(0.9f, 0.7f, 0.5f),
            new Color(0.75f, 0.55f, 0.35f),
            new Color(0.6f, 0.4f, 0.25f),
            new Color(0.4f, 0.25f, 0.15f)
        };
        return skinTones[rng.Next(skinTones.Length)];
    }

    private Color ShiftHue(Color original, float shiftAmount)
    {
        Color.RGBToHSV(original, out float h, out float s, out float v);
        h = Mathf.Repeat(h + shiftAmount, 1f);
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
