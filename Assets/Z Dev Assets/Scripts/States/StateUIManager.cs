using UnityEngine;
using TMPro;

public class StateUIManager : MonoBehaviour
{
    public static StateUIManager Instance;

    [Header("UI References")]
    public TMP_Text hoverNameText;
    public TMP_Text selectedNameText;
    public GameObject selectedPanel;

    private StateButton currentSelected;

    void Awake()
    {
        Instance = this;
        hoverNameText.text = "";
        selectedPanel.SetActive(false);
    }

    public void ShowStateName(string stateName)
    {
        hoverNameText.text = stateName;
    }

    public void HideStateName()
    {
        hoverNameText.text = "";
    }

    public void SelectState(StateButton state)
    {
        if (currentSelected != null && currentSelected != state)
            currentSelected.ForceDeselect();

        currentSelected = state;

        selectedPanel.SetActive(true);
        selectedNameText.text = state.stateName;

        Debug.Log($"Selected {state.stateName}");
    }

    public void DeselectState()
    {
        if (currentSelected != null)
        {
            currentSelected.ForceDeselect();
            currentSelected = null;
        }

        selectedPanel.SetActive(false);
        selectedNameText.text = "";
    }
}
