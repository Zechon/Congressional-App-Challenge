using UnityEngine;

public class StaffPortraitGenerator : MonoBehaviour
{
    [SerializeField] private Camera portraitCamera;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Vector3 renderPosition = new Vector3(9999, 9999, 0);

    public Sprite GeneratePortrait(GameObject staffPrefab)
    {
        GameObject temp = Instantiate(staffPrefab, renderPosition, Quaternion.identity);
        temp.layer = LayerMask.NameToLayer("Portrait");
        SetLayerRecursively(temp, LayerMask.NameToLayer("Portrait"));

        portraitCamera.targetTexture = renderTexture;
        portraitCamera.Render();

        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        Destroy(temp);

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}
