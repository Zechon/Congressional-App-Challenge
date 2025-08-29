using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class Ideal_Determination : MonoBehaviour
{
    [Header("Values")]
    private int choicesRemaining = 4;
    private int unweighedIdeals = 0;
    public float weighedIdeals = 0f;
    public bool OrangeLeaning = false;

    [Header("Components")]
    [SerializeField] private GameObject warning;

    void Start()
    {
        //components
        warning.SetActive(false);

        //set variables
        weighedIdeals = 0f;
        OrangeLeaning = false;
        unweighedIdeals = 0;
        choicesRemaining = 4;
    }

    public void RemainingIdls()
    {
        if (choicesRemaining <= 0) { warning.SetActive(true); }
        else { warning.SetActive(false); }
    }

    public void weighIdls()
    {
        switch (unweighedIdeals)
        {
            case -4:
                OrangeLeaning = true;
                weighedIdeals = 1f;
                break;

            case -3:
                OrangeLeaning = true;
                weighedIdeals = .75f;
                break;

            case 0:
                //ChooseColor();
                weighedIdeals = .5f;
                break;

            case 3:
                OrangeLeaning = false;
                weighedIdeals = .75f;
                break;

            case 4:
                OrangeLeaning = false;
                weighedIdeals = 1f;
                break;
        }
    }

    //[HideInInspector]
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