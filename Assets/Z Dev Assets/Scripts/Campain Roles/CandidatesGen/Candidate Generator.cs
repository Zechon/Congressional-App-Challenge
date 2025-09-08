using System.Collections;
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

    public void SetupCandidate(StaffData data)
    {
        data.AssignClothingIfNeeded();
        data.AssignColorsIfNeeded();

        if (data.selectedShirt != null) shirtRender.sprite = data.selectedShirt;
        if (data.selectedHat != null) hatRender.sprite = data.selectedHat;
        if (data.selectedMisc1 != null) misc1Render.sprite = data.selectedMisc1;
        if (data.selectedMisc2 != null) misc2Render.sprite = data.selectedMisc2;

        bodyRender.color = data.assignedSkin;
        shirtRender.color = data.assignedShirt;
        hatRender.color = data.assignedHat;
        misc1Render.color = data.assignedMisc1;
        misc2Render.color = data.assignedMisc2;
    }
}
