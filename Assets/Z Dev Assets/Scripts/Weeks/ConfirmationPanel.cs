using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static ActionDatabase;

public class ConfirmationPanel : MonoBehaviour
{
    public static ConfirmationPanel instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI effectText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private StateSetup currentState;
    private ActionDatabase.CampaignAction currentAction;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);

        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(Hide);
    }

    public void Show(StateSetup state, string actionName)
    {
        currentState = state;
        currentAction = ActionPanelUI.instance.actionDatabase.GetAction(actionName);

        if (currentAction == null)
        {
            Debug.LogWarning($"Action {actionName} not found!");
            return;
        }

        gameObject.SetActive(true);

        // Update UI
        actionNameText.text = currentAction.actionName;
        costText.text = $"Cost: ${currentAction.baseCost}";

        string effectStr = currentAction.category switch
        {
            EffectCategory.Money => $"Gain: ${currentAction.baseEffect}",
            EffectCategory.VoterSway => $"Potential voter sway: {currentAction.baseEffect * 100f}%",
            EffectCategory.PR => $"PR Boost: {currentAction.baseEffect * 100f}%",
            _ => "Effect"
        };
        effectText.text = effectStr;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        currentState = null;
        currentAction = null;
    }

    private void OnConfirm()
    {
        if (currentAction == null) return;

        ApplyEffect(currentAction, currentState);
        Hide();
    }

    private void ApplyEffect(ActionDatabase.CampaignAction action, StateSetup state)
    {
        // Deduct cost always
        GameData.Money -= action.baseCost;

        switch (action.category)
        {
            case EffectCategory.Money:
                ApplyMoney(action);
                break;

            case EffectCategory.PR:
                ApplyPR(action);
                break;

            case EffectCategory.VoterSway:
                ApplyVoterSway(action, state);
                break;

            default:
                Debug.LogWarning($"Unhandled action category: {action.category}");
                break;
        }
    }

    private void ApplyMoney(ActionDatabase.CampaignAction action)
    {
        float finalEffect = action.baseEffect;

        foreach (var mod in action.modifiers)
        {
            if (mod.type == ModifierType.Effect)
                finalEffect += action.baseEffect * mod.value;
        }

        GameData.Money += Mathf.RoundToInt(finalEffect);
        Debug.Log($"Money action applied. New balance: ${GameData.Money}");
    }

    private void ApplyPR(ActionDatabase.CampaignAction action)
    {
        float finalEffect = action.baseEffect;

        foreach (var mod in action.modifiers)
        {
            if (mod.type == ModifierType.Effect)
                finalEffect += action.baseEffect * mod.value;
        }

        GameData.PR += finalEffect;
        Debug.Log($"PR action applied. Current PR: {GameData.PR * 100f}%");
    }

    private void ApplyVoterSway(ActionDatabase.CampaignAction action, StateSetup state)
    {
        if (state == null)
        {
            Debug.LogWarning("Voter sway action requires a valid state.");
            return;
        }

        // Success/fail check
        float roll = Random.value;
        float successModifier = roll <= action.successChance ? 1f : -0.5f;

        float voterSway = action.baseEffect;
        foreach (var mod in action.modifiers)
        {
            if (mod.type == ModifierType.Effect)
                voterSway += action.baseEffect * mod.value;
        }

        // Scale effect for large states
        float scalingFactor = 1f / Mathf.Log(state.stateVotes + 1);

        int votesGained = Mathf.RoundToInt(state.stateVotes * voterSway * GameData.PR * successModifier * scalingFactor);

        GameData.Votes += votesGained;

        Debug.Log($"Voter sway applied. Votes gained: {votesGained}, Total Votes: {GameData.Votes}");
    }
}
