using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CandidateGenerator : MonoBehaviour
{
    public Image bodyRender;
    public Image shirtRender;
    public Image hatRender;
    public Image misc1Render;
    public Image misc2Render;

    // Temporary session cache for all candidates
    private static Dictionary<StaffData, TempLook> tempLooks = new Dictionary<StaffData, TempLook>();

    private struct TempLook
    {
        public Sprite shirtSprite, hatSprite, misc1Sprite, misc2Sprite;
        public Color skin, shirt, hat, misc1, misc2;
    }

    // Called whenever a candidate prefab spawns (preview mode)
    public void PreviewCandidate(StaffData data)
    {
        // If temp look exists, use it
        if (!data.hasTempLook)
        {
            data.AssignClothingIfNeeded();
            data.AssignColorsIfNeeded();
            data.ApplyTempLook(); // save the temporary session colors/clothes
        }

        bodyRender.color = data.tempSkin;
        if (data.tempShirt != null) shirtRender.sprite = data.tempShirt;
        if (data.tempHat != null) hatRender.sprite = data.tempHat;
        if (data.tempMisc1 != null) misc1Render.sprite = data.tempMisc1;
        if (data.tempMisc2 != null) misc2Render.sprite = data.tempMisc2;

        shirtRender.color = data.tempShirtColor;
        hatRender.color = data.tempHatColor;
        misc1Render.color = data.tempMisc1Color;
        misc2Render.color = data.tempMisc2Color;
    }


    // Called when a candidate is selected / confirmed
    public void SetupCandidate(StaffData data)
    {
        if (tempLooks.TryGetValue(data, out TempLook look))
        {
            // Save final selection to StaffData
            data.selectedShirt = look.shirtSprite;
            data.selectedHat = look.hatSprite;
            data.selectedMisc1 = look.misc1Sprite;
            data.selectedMisc2 = look.misc2Sprite;

            data.assignedSkin = look.skin;
            data.assignedShirt = look.shirt;
            data.assignedHat = look.hat;
            data.assignedMisc1 = look.misc1;
            data.assignedMisc2 = look.misc2;

            ApplyLook(look);
        }
        else
        {
            // fallback: preview if no cached look
            PreviewCandidate(data);
        }
    }

    private void ApplyLook(TempLook look)
    {
        bodyRender.color = look.skin;
        shirtRender.sprite = look.shirtSprite;
        shirtRender.color = look.shirt;
        hatRender.sprite = look.hatSprite;
        hatRender.color = look.hat;
        misc1Render.sprite = look.misc1Sprite;
        misc1Render.color = look.misc1;
        misc2Render.sprite = look.misc2Sprite;
        misc2Render.color = look.misc2;
    }

    // Clear temporary session data (call when scene ends)
    public static void ClearTempLooks()
    {
        tempLooks.Clear();
    }
}
