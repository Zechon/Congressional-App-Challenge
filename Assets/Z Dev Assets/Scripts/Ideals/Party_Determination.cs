using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Rendering;


public class Party_Determination : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private string NextSceneName = "";
    public bool orange = false;

    [Header("Main Screen Components")]
    [SerializeField] private GameObject partyBox;
    [SerializeField] private TMP_Text partyText;

    [Header("Secondary Screen Components")]
    [SerializeField] private GameObject prtyChoose;
    [SerializeField] private TMP_Text youChose;
    [SerializeField] private GameObject fadeOverlay;
    [SerializeField] private float fadeDuration = 0.3f;

    void Start()
    {
        StartCoroutine(FadeIn());

        prtyChoose.SetActive(true);
        youChose.enabled = false;
        partyBox.SetActive(false);

        orange = false;
    }

    private IEnumerator FadeIn()
    {
        fadeOverlay.GetComponent<CanvasGroup>().DOFade(0f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        fadeOverlay.SetActive(false);
    }

    private IEnumerator FadeOut()
    {

        if (orange) { GameData.Party = "Orange"; }
        else { GameData.Party = "Purple"; }

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

    public void PtyOrngButton()
    {
        orange = true;
        partyBox.SetActive(true);
        partyText.text = "You've chosen the Orange Party! yay i guess";
        StartCoroutine(FadeOut());
    }

    public void PtyPurplButton()
    {
        orange = false;
        partyBox.SetActive(true);
        partyText.text = "You've chosen the Purple Party! yay i guess";
        StartCoroutine(FadeOut());
    }
}
