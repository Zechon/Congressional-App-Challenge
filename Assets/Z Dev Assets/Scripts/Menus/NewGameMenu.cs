using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class NewGameMenu : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown difficultyDropdown;
    public TMP_InputField seedInput;
    public Button start;

    [Header("Other")]
    [SerializeField] private string NextSceneName;
    [SerializeField] private GameObject fadeOverlay;
    [SerializeField] private float fadeDuration = 0.3f;

    void Start()
    {
        StartCoroutine(FadeIn());
        start.onClick.AddListener(OnStartGame);
    }

    private IEnumerator FadeIn()
    {
        fadeOverlay.GetComponent<CanvasGroup>().DOFade(0f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        fadeOverlay.SetActive(false);
    }

    void OnStartGame()
    {
        string input = seedInput.text.Trim();

        if (string.IsNullOrEmpty(input))
        {
            Difficulty chosenDifficulty = (Difficulty)(difficultyDropdown.value + 1);
            SeedManager.GenerateSeed(chosenDifficulty);
        }
        else
        {
            if (int.TryParse(input, out int seed))
            {
                SeedManager.UseSeed(seed);
            }
            else
            {
                Debug.LogWarning("Invalid seed entered! Generating a random one instead.");
                Difficulty chosenDifficulty = (Difficulty)(difficultyDropdown.value + 1);
                SeedManager.GenerateSeed(chosenDifficulty);
            }
        }

        Debug.Log($"Starting game with seed {SeedManager.Seed} and difficulty {SeedManager.CurrentDifficulty}");

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        GameData.RunSeed = SeedManager.Seed;

        yield return new WaitForSeconds(2);

        fadeOverlay.SetActive(true);
        fadeOverlay.GetComponent<CanvasGroup>().DOFade(1f, fadeDuration);

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene(NextSceneName);

        StartCoroutine(LoadSceneFully(NextSceneName));
    }

    IEnumerator LoadSceneFully(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            yield return null;
        }

        op.allowSceneActivation = true;

        yield return null;

        Debug.Log("Scene is fully loaded and initialized!");
    }
}

