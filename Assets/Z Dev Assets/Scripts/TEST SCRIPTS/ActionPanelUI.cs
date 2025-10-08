using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActionPanelUI : MonoBehaviour
{
    public static ActionPanelUI instance;

    [Header("UI References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentParent; // Content inside ScrollView
    [SerializeField] private GameObject actionButtonPrefab;


    [System.Serializable]
    public class ActionOption
    {
        public string name;
        public int cost;
    }
    [Header("Available Actions")]
    [SerializeField] private List<ActionOption> availableActions = new List<ActionOption>();

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
        PopulateActions();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        currentState = null;
    }


    public void PopulateActions()
    {
        // Clear old buttons
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Spawn new buttons
        foreach (var action in availableActions)
        {
            GameObject btnObj = Instantiate(actionButtonPrefab, contentParent);
            btnObj.transform.localScale = Vector3.one; // Important to reset scale
            var buttonUI = btnObj.GetComponent<ActionButtonUI>();
            buttonUI.Setup(action.name, action.cost);
        }

        // Force layout rebuild so ScrollRect knows the content size
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f; // Scroll to top
    }

    public void OnSelectAction(string actionName, int actionCost)
    {
        if (currentState == null)
        {
            Debug.LogWarning("Tried to select an action, but no state is currently active.");
            return;
        }

        Debug.Log($"Selected action: {actionName} (${actionCost}) in {currentState.stateName}");

        // Show confirmation before applying
        if (ConfirmationPanel.instance != null)
            ConfirmationPanel.instance.Show(currentState, actionName, actionCost);
        else
            Debug.LogWarning("No ConfirmationPanel instance found.");

        Hide();
    }
}
