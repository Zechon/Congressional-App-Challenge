using UnityEngine;
using TMPro;

public class ConfirmationPanel : MonoBehaviour
{
    public static ConfirmationPanel instance;

    [Header("UI References")]
    public TMP_Text confirmText;

    private StateSetup selectedState;
    private string actionName;
    private int actionCost;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Show(StateSetup state, string action, int cost)
    {
        selectedState = state;
        actionName = action;
        actionCost = cost;

        confirmText.text = $"Plan {action} in {state.stateName} for {cost} budget?";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        selectedState = null;
        actionName = "";
        actionCost = 0;
    }

    // UI Button: Confirm
    public void OnConfirm()
    {
        GamePhaseManager.instance.ConfirmAction(selectedState.stateName, actionName, actionCost);
        Hide();
    }

    // UI Button: Cancel
    public void OnCancel()
    {
        Hide();

        // Optionally reopen action panel for same state
        ActionPanel.instance.Show(selectedState);
    }
}

