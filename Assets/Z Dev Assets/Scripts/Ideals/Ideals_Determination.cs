using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Ideal_Determination : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float buttonTimerLimit = 2f;
    [SerializeField] private string NextSceneName = "";
    private int choicesRemaining = 4;
    private int unweighedIdeals = 0;
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

    void Start()
    {
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
        }

        weighIdls();

        if (weighedIdeals != 50)
        {
            partyBox.SetActive(true);
            if (OrangeLeaning) { partyText.text = "The Orange Party has chosen you with a favor rating of " + weighedIdeals.ToString() + " %."; }
            else { partyText.text = "The Purple Party has chosen you with a favor rating of " + weighedIdeals.ToString() + " %."; }
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

            case -3:
                OrangeLeaning = true;
                weighedIdeals = 75;
                break;

            case 0:
                ChooseColor();
                weighedIdeals = 50;
                break;

            case 3:
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
        youChose.enabled = true;
        youChose.text = "You've chosen Orange!";
        StartCoroutine(HideAfterDelay());
    }

    public void IdlPrplButton()
    {
        OrangeLeaning = false;
        youChose.enabled = true;
        youChose.text = "You've chosen Purple!";
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(buttonTimerLimit);
        prtyChoose.SetActive(false);

        partyBox.SetActive(true);
        if (OrangeLeaning) { partyText.text = "You have chosen the Orange Party with a favor rating of " + weighedIdeals.ToString() + " %."; }
        else { partyText.text = "You have chosen the Orange Purple with a favor rating of " + weighedIdeals.ToString() + " %."; }
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