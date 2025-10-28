using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance;

    public int totalScore;
    public int currentPhaseScore;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ApplyDebateMultiplier(float multiplier)
    {
        int newScore = Mathf.RoundToInt(currentPhaseScore * multiplier);
        totalScore += newScore;
        Debug.Log($"Debate finished! Multiplier: {multiplier}x | Phase Score: {currentPhaseScore} | Added: {newScore}");
    }
}
