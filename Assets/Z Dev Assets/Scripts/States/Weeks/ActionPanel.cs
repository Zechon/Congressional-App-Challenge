using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    public static ActionPanel instance;

    private StateSetup currentState;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Show(StateSetup state)
    {
        currentState = state;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        currentState = null;
    }

    public void OnSelectAction(string actionName, int cost)
    {
        if (currentState != null) return;

        ConfirmationPanel.instance.Show(currentState, actionName, cost);

        Hide();
    }
}
