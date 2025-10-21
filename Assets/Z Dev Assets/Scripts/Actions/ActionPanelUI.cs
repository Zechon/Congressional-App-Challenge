using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ActionPanelUI : MonoBehaviour
{
    public static ActionPanelUI instance;

    [Header("UI References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject actionButtonPrefab;

    [Header("Action + Staff Databases")]
    [SerializeField] private ActionDatabase actionDatabase;

    private StateSetup currentState;
    private List<ActionDatabase.CampaignAction> currentActions = new List<ActionDatabase.CampaignAction>();

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);

        List<StaffData> hires = GameData.HiredStaff;
    }

    public void Show(StateSetup state)
    {
        currentState = state;
        gameObject.SetActive(true);

        if (actionDatabase != null)
        {
            currentActions = actionDatabase.actions;
            PopulateActions();
        }
        else
        {
            Debug.LogWarning("No ActionDatabase assigned to ActionPanelUI!");
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        currentState = null;
    }

    private void PopulateActions()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var action in currentActions)
        {
            GameObject btnObj = Instantiate(actionButtonPrefab, contentParent);
            btnObj.transform.localScale = Vector3.one;

            var buttonUI = btnObj.GetComponent<ActionButtonUI>();

            buttonUI.Setup(action);
        }

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void OnSelectAction(string actionName, int actionCost)
    {
        if (currentState == null)
        {
            Debug.LogWarning("Tried to select an action, but no state is currently active.");
            return;
        }

        var action = actionDatabase.GetAction(actionName);
        if (action == null)
        {
            Debug.LogWarning($"Action {actionName} not found in database!");
            return;
        }

        Debug.Log($"Selected action: {actionName} (${actionCost}) in {currentState.stateName}");

        if (ConfirmationPanel.instance != null)
            ConfirmationPanel.instance.Show(currentState, actionName, actionCost);

        Hide();
    }
}
