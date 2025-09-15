using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Difficulty { Easy, Normal, Hard }

public class StateManager : MonoBehaviour
{
    private StateSetup[] states;

    [Header("Difficulty")]
    public Difficulty currentDifficulty = Difficulty.Normal;

    [Header("Vote Totals")]
    public int orangeVT;
    public int purpleVT;

    void Start()
    {
        states = FindObjectsOfType<StateSetup>();

        SetupStates();
        CountVotes();
    }

    private void SetupStates()
    {
        int nationalVotes = states.Sum(s => s.stateVotes);

        float baseShare = currentDifficulty switch
        {
            Difficulty.Easy => 0.60f,
            Difficulty.Normal => 0.50f,
            Difficulty.Hard => 0.40f,
            _ => 0.50f
        };

        int targetOrangeVotes = 0;
        int targetPurpleVotes = 0;

        if (GameData.Party == "Orange")
        {
            targetOrangeVotes = Mathf.RoundToInt(nationalVotes * baseShare);
            targetPurpleVotes = nationalVotes - targetOrangeVotes;
        }
        else if (GameData.Party == "Purple")
        {
            targetPurpleVotes = Mathf.RoundToInt(nationalVotes * baseShare);
            targetOrangeVotes = nationalVotes - targetPurpleVotes;
        }
        else
        {
            Debug.LogWarning($"Unknown party string in GameData.Party: {GameData.Party}");
            targetOrangeVotes = nationalVotes / 2;
            targetPurpleVotes = nationalVotes / 2;
        }

        RandomizeStates(targetOrangeVotes, targetPurpleVotes);
    }

    private void RandomizeStates(int targetOrangeVotes, int targetPurpleVotes)
    {
        int orangeRemaining = targetOrangeVotes;
        int purpleRemaining = targetPurpleVotes;

        foreach (var state in states.OrderBy(x => Random.value))
        {
            float orangeRatio = (float)orangeRemaining / (orangeRemaining + purpleRemaining + 1);
            float randomFactor = Random.Range(-0.15f, 0.15f);
            float finalRatio = Mathf.Clamp01(orangeRatio + randomFactor);

            int orangeVotes = Mathf.RoundToInt(state.stateVotes * finalRatio);
            int purpleVotes = state.stateVotes - orangeVotes;

            state.orangePercent = Mathf.RoundToInt((orangeVotes / (float)state.stateVotes) * 100f);
            state.purplePercent = 100 - state.orangePercent;

            state.CalculateStateColor();

            orangeRemaining -= orangeVotes;
            purpleRemaining -= purpleVotes;
        }
    }

    public void CountVotes()
    {
        orangeVT = 0;
        purpleVT = 0;

        foreach (var state in states)
        {
            int orangePT = Mathf.RoundToInt(state.orangePercent * state.stateVotes);
            orangeVT += orangePT;
            purpleVT += state.stateVotes - orangePT;
        }
    }
}
