using UnityEngine;

public class PressConferenceManager : MonoBehaviour
{
    public static PressConferenceManager instance;

    [Header("UI")]
    public GameObject pressPanel;
    public Animator panelAnimator;

    private void Awake()
    {
        instance = this;
        if (pressPanel != null)
            pressPanel.SetActive(false);
    }

    public void StartConference()
    {
        if (pressPanel == null)
        {
            Debug.LogWarning("PressConference panel not assigned!");
            return;
        }

        pressPanel.SetActive(true);
        panelAnimator?.SetTrigger("Show");
        Debug.Log("Press Conference started!");
    }

    public void EndConference()
    {
        panelAnimator?.SetTrigger("Hide");
        pressPanel.SetActive(false);
        Debug.Log("Press Conference ended.");
        GamePhaseManager.instance?.ConfirmAction("National", "Press Conference", 0);
    }
}
