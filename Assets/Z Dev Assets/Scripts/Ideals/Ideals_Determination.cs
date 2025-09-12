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

public class Ideal_Determination : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float buttonTimerLimit = 2f;
    [SerializeField] private string NextSceneName = "";
    private int choicesRemaining = 4;
    public int unweighedIdeals = 0;
    public int weighedIdeals = 0;
    public bool OrangeLeaning = false;

    [Header("Main Screen Components")]
    [SerializeField] private GameObject warning;
    [SerializeField] private TMP_Text warnText;
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

        //components
        warning.SetActive(false);
        prtyChoose.SetActive(false);
        youChose.enabled = false;
        partyBox.SetActive(false);

        //set variables
        weighedIdeals = 0;
        OrangeLeaning = false;
        unweighedIdeals = 0;
        choicesRemaining = 4;
    }

    private IEnumerator FadeIn()
    {
        Debug.Log("Hello");
        fadeOverlay.GetComponent<CanvasGroup>().DOFade(0f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        fadeOverlay.SetActive(false);
    }

    public void RemainingIdls()
    {
        if (choicesRemaining <= 0)
        {
            warnText.text = "You may only select four options.";
            warning.SetActive(true);
        }
        else { warning.SetActive(false); }
    }

    public void IdlConfirm()
    {
        if (choicesRemaining >= 1)
        {
            warnText.text = "You must choose four options.";
            warning.SetActive(true);
            return;
        }

        weighIdls();

        if (weighedIdeals != 50)
        {
            partyBox.SetActive(true);
            if (OrangeLeaning) { partyText.text = "The Orange Party has chosen you with a favor rating of " + weighedIdeals.ToString() + " %."; }
            else { partyText.text = "The Purple Party has chosen you with a favor rating of " + weighedIdeals.ToString() + " %."; }
            StartCoroutine(FadeOut());
        }
    }

    public void weighIdls()
    {
        switch (unweighedIdeals)
        {
            case -4:
                OrangeLeaning = true;
                weighedIdeals = 100;
                break;

            case -2:
                OrangeLeaning = true;
                weighedIdeals = 75;
                break;

            case 0:
                ChooseColor();
                weighedIdeals = 50;
                break;

            case 2:
                OrangeLeaning = false;
                weighedIdeals = 75;
                break;

            case 4:
                OrangeLeaning = false;
                weighedIdeals = 100;
                break;
        }
    }

    private void ChooseColor()
    {
        prtyChoose.SetActive(true);
    }

    public void IdlOrngButton()
    {
        OrangeLeaning = true;
        partyBox.SetActive(true);
        partyText.text = "The Orange Party has chosen you with a favor rating of " + weighedIdeals.ToString() + " %.";
        StartCoroutine(FadeOut());
    }

    public void IdlPrplButton()
    {
        OrangeLeaning = false;
        partyBox.SetActive(true);
        partyText.text = "The Purple Party has chosen you with a favor rating of " + weighedIdeals.ToString() + " %."; 
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
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

    public void IdlsCont()
    {
        SceneManager.LoadScene(NextSceneName);
    }

    [HideInInspector]
    public bool Idl1Tggl = false;
    public void Idl1Bttn()
    {
        if (!Idl1Tggl)
        {
            RemainingIdls();
            if (warning.activeSelf) { return; }

            choicesRemaining--;
            unweighedIdeals--;
            Idl1Tggl = true;
        }
        else if (Idl1Tggl)
        {
            warning.SetActive(false);

            choicesRemaining++;
            unweighedIdeals++;
            Idl1Tggl = false;
        }
    }

    [HideInInspector]
    public bool Idl2Tggl = false;
    public void Idl2Bttn()
    {
        if (!Idl2Tggl)
        {
            RemainingIdls();
            if (warning.activeSelf) { return; }

            choicesRemaining--;
            unweighedIdeals--;
            Idl2Tggl = true;
        }
        else
        {
            warning.SetActive(false);

            choicesRemaining++;
            unweighedIdeals++;
            Idl2Tggl = false;
        }
    }

    [HideInInspector]
    public bool Idl3Tggl = false;
    public void Idl3Bttn()
    {
        if (!Idl3Tggl)
        {
            RemainingIdls();
            if (warning.activeSelf) { return; }

            choicesRemaining--;
            unweighedIdeals--;
            Idl3Tggl = true;
        }
        else
        {
            warning.SetActive(false);

            choicesRemaining++;
            unweighedIdeals++;
            Idl3Tggl = false;
        }
    }

    [HideInInspector]
    public bool Idl4Tggl = false;
    public void Idl4Bttn()
    {
        if (!Idl4Tggl)
        {
            RemainingIdls();
            if (warning.activeSelf) { return; }

            choicesRemaining--;
            unweighedIdeals--;
            Idl4Tggl = true;
        }
        else
        {
            warning.SetActive(false);

            choicesRemaining++;
            unweighedIdeals++;
            Idl4Tggl = false;
        }
    }

    [HideInInspector]
    public bool Idl5Tggl = false;
    public void Idl5Bttn()
    {
        if (!Idl5Tggl)
        {
            RemainingIdls();
            if (warning.activeSelf) { return; }

            choicesRemaining--;
            unweighedIdeals++;
            Idl5Tggl = true;
        }
        else
        {
            warning.SetActive(false);

            choicesRemaining++;
            unweighedIdeals--;
            Idl5Tggl = false;
        }
    }

    [HideInInspector]
    public bool Idl6Tggl = false;
    public void Idl6Bttn()
    {
        if (!Idl6Tggl)
        {
            RemainingIdls();
            if (warning.activeSelf) { return; }

            choicesRemaining--;
            unweighedIdeals++;
            Idl6Tggl = true;
        }
        else
        {
            warning.SetActive(false);

            choicesRemaining++;
            unweighedIdeals--;
            Idl6Tggl = false;
        }
    }

    [HideInInspector]
    public bool Idl7Tggl = false;
    public void Idl7Bttn()
    {
        if (!Idl7Tggl)
        {
            RemainingIdls();
            if (warning.activeSelf) { return; }

            choicesRemaining--;
            unweighedIdeals++;
            Idl7Tggl = true;
        }
        else
        {
            warning.SetActive(false);

            choicesRemaining++;
            unweighedIdeals--;
            Idl7Tggl = false;
        }
    }

    [HideInInspector]
    public bool Idl8Tggl = false;
    public void Idl8Bttn()
    {
        if (!Idl8Tggl)
        {
            RemainingIdls();
            if (warning.activeSelf) { return; }

            choicesRemaining--;
            unweighedIdeals++;
            Idl8Tggl = true;
        }
        else
        {
            warning.SetActive(false);

            choicesRemaining++;
            unweighedIdeals--;
            Idl8Tggl = false;
        }
    }
}