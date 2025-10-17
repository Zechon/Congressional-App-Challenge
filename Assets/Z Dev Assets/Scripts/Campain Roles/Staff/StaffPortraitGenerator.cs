using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StaffPortraitGenerator : MonoBehaviour
{
    [Header("UI Capture Settings")]
    [SerializeField] private Canvas sourceCanvas;
    [SerializeField] private Vector2 renderResolution = new Vector2(512, 512);

    public Sprite GeneratePortrait(GameObject uiPrefab, string staffName, RectTransform rect)
    {
        if (uiPrefab == null || rect == null)
        {
            Debug.LogError("Invalid UI prefab or RectTransform for portrait capture!");
            return null;
        }

        bool wasActive = uiPrefab.activeSelf;
        uiPrefab.SetActive(true);

        RenderTexture rt = new RenderTexture((int)renderResolution.x, (int)renderResolution.y, 0, RenderTextureFormat.ARGB32);
        rt.Create();

        GameObject camObj = new GameObject("TempUICaptureCamera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0);
        cam.targetTexture = rt;

        Canvas canvas = sourceCanvas ?? uiPrefab.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found for UI portrait capture!");
            Destroy(camObj);
            return null;
        }

        var originalRenderMode = canvas.renderMode;
        var originalCamera = canvas.worldCamera;

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        Canvas.ForceUpdateCanvases();

        cam.Render();

        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);

        float x = bottomLeft.x;
        float y = bottomLeft.y;
        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        Rect cropRect = new Rect(
            Mathf.Clamp(x, 0, rt.width),
            Mathf.Clamp(y, 0, rt.height),
            Mathf.Clamp(width, 0, rt.width - x),
            Mathf.Clamp(height, 0, rt.height - y)
        );

        RenderTexture.active = rt;
        Texture2D fullTex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        fullTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        fullTex.Apply();

        Texture2D croppedTex = new Texture2D((int)cropRect.width, (int)cropRect.height, TextureFormat.RGBA32, false);
        Color[] pixels = fullTex.GetPixels((int)cropRect.x, (int)cropRect.y, (int)cropRect.width, (int)cropRect.height);
        croppedTex.SetPixels(pixels);
        croppedTex.Apply();

        RenderTexture.active = null;

        canvas.renderMode = originalRenderMode;
        canvas.worldCamera = originalCamera;

        Destroy(camObj);
        rt.Release();
        uiPrefab.SetActive(wasActive);

        string folderPath = Application.dataPath + "/SavedPortraits/";
        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);

        string fileName = staffName.Replace(" ", "_") + ".png";
        string fullPath = folderPath + fileName;

        byte[] pngData = croppedTex.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, pngData);
        Debug.Log($"Saved cropped portrait: {fullPath}");

        return Sprite.Create(croppedTex, new Rect(0, 0, croppedTex.width, croppedTex.height), new Vector2(0.5f, 0.5f), 100f);
    }


    public static Sprite LoadPortrait(string staffName)
    {
        string path = Path.Combine(Application.persistentDataPath, "StaffPortraits", staffName.Replace(" ", "_") + ".png");

        if (!File.Exists(path))
        {
            Debug.LogWarning("Portrait not found for " + staffName);
            return null;
        }

        byte[] fileData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(fileData);

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
    }
}
