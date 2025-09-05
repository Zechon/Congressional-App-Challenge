using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandidateGenerator : MonoBehaviour
{
    public SpriteRenderer bodyRender;
    public SpriteRenderer shirtRender;
    public SpriteRenderer hatRender;
    public SpriteRenderer misc1Render;
    public SpriteRenderer misc2Render;

    public void SetupCandidate(StaffData data)
    {
        if (data.shirtOptions.Length > 0)
            shirtRender.sprite = data.shirtOptions[Random.Range(0, data.shirtOptions.Length)];

        if (data.hatOptions.Length > 0)
            hatRender.sprite = data.hatOptions[Random.Range(0, data.hatOptions.Length)];

        if (data.misc1Options.Length > 0)
            misc1Render.sprite = data.misc1Options[Random.Range(0, data.misc1Options.Length)];

        if (data.misc2Options.Length > 0)
            misc2Render.sprite = data.misc2Options[Random.Range(0, data.misc2Options.Length)];
    }
}
