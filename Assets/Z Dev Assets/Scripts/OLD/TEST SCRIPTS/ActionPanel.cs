using UnityEngine;

public class OLDActionPanel : MonoBehaviour
{
    public static OLDActionPanel instance;

    [Header("UI References")]
    public Transform contentParent;   // ScrollView/Viewport/Content
    public GameObject actionButtonPrefab;

    private StateSetup currentState;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Show(StateSetup state)
    {
        currentState = state;
        gameObject.SetActive(true);

        // Clear old buttons
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Example actions (replace with your actual set)
        AddAction("Ad Campaign", 50);
        AddAction("Parade", 30);
        AddAction("Fundraise", 0);
        AddAction("Humanitarian Aid", 40);
        AddAction("Debate Prep", 25);
        AddAction("Town Hall Meeting", 20);
    }

    private void AddAction(string name, int cost)
    {
        var btn = Instantiate(actionButtonPrefab, contentParent);
        //btn.GetComponent<ActionButtonUI>().Setup(name, cost);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        currentState = null;
    }

    public void OnSelectAction(string actionName, int cost)
    {
        if (currentState == null) return;

        ConfirmationPanel.instance.Show(currentState, actionName);
        Hide();
    }
}
