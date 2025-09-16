using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        float baseShare = SeedManager.CurrentDifficulty switch
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
            targetPurpleVotes = nationalVotes - targetOrangeVotes;
        }

        RandomizeStates(targetOrangeVotes, targetPurpleVotes);
    }

    private void RandomizeStates(int targetOrangeVotes, int targetPurpleVotes)
    {
        int orangeRemaining = targetOrangeVotes;
        int purpleRemaining = targetPurpleVotes;

        // Shuffle states so distribution isn't predictable
        foreach (var state in states.OrderBy(x => Random.value))
        {
            // Fraction of remaining national pool
            float orangeRatio = (float)orangeRemaining / (orangeRemaining + purpleRemaining + 1); // +1 avoids div/0
            float randomFactor = Random.Range(-0.15f, 0.15f);
            float finalRatio = Mathf.Clamp01(orangeRatio + randomFactor);

            int orangeVotes = Mathf.RoundToInt(state.stateVotes * finalRatio);
            orangeVotes = Mathf.Clamp(orangeVotes, 0, state.stateVotes);

            int purpleVotes = state.stateVotes - orangeVotes;

            // Save as fractions (0..1)
            state.orangePercent = (state.stateVotes > 0) ? (orangeVotes / (float)state.stateVotes) : 0f;
            state.purplePercent = 1f - state.orangePercent;

            state.CalculateStateColor();

            orangeRemaining -= orangeVotes;
            purpleRemaining -= purpleVotes;
        }

        // If rounding left leftover/shortfall in the national pool, patch it deterministically:
        int totalAssignedOrange = states.Sum(s => Mathf.RoundToInt(s.orangePercent * s.stateVotes));
        int nationalTotal = states.Sum(s => s.stateVotes);
        int diff = targetOrangeVotes - totalAssignedOrange;

        if (diff != 0)
            FixRoundingDiff(diff);
    }

    private void FixRoundingDiff(int diff)
    {
        var list = new List<(StateSetup state, float remainder)>();
        foreach (var s in states)
        {
            float exactOrange = s.orangePercent * s.stateVotes;
            int floored = Mathf.FloorToInt(exactOrange);
            float remainder = exactOrange - floored;
            list.Add((s, remainder));
        }

        if (diff > 0)
        {
            foreach (var tuple in list.OrderByDescending(x => x.remainder).Take(diff))
            {
                var s = tuple.state;
                // increase integer orange votes by 1, then update percent
                int orangeVotes = Mathf.RoundToInt(s.orangePercent * s.stateVotes) + 1;
                orangeVotes = Mathf.Clamp(orangeVotes, 0, s.stateVotes);
                s.orangePercent = orangeVotes / (float)s.stateVotes;
                s.purplePercent = 1f - s.orangePercent;
                s.CalculateStateColor();
            }
        }
        else if (diff < 0)
        {
            int toRemove = -diff;
            foreach (var tuple in list.OrderBy(x => x.remainder).Take(toRemove))
            {
                var s = tuple.state;
                int orangeVotes = Mathf.RoundToInt(s.orangePercent * s.stateVotes) - 1;
                orangeVotes = Mathf.Clamp(orangeVotes, 0, s.stateVotes);
                s.orangePercent = orangeVotes / (float)s.stateVotes;
                s.purplePercent = 1f - s.orangePercent;
                s.CalculateStateColor();
            }
        }
    }

    public void CountVotes()
    {
        orangeVT = 0;
        purpleVT = 0;

        int nationalTotal = states.Sum(s => s.stateVotes);

        // First compute per-state integer votes using rounding
        var perStateOrange = new List<int>(states.Length);
        float sumExact = 0f;
        foreach (var s in states)
        {
            float exactOrange = Mathf.Clamp01(s.orangePercent) * s.stateVotes;
            sumExact += exactOrange;
            perStateOrange.Add(Mathf.FloorToInt(exactOrange)); // floor for now
        }

        // Largest remainder to match rounded total nearest to sumExact
        int targetOrangeSum = Mathf.RoundToInt(sumExact);
        int currentOrangeSum = perStateOrange.Sum();
        int remainderToAssign = targetOrangeSum - currentOrangeSum;

        // compute remainders
        var remainders = new List<(int idx, float rem)>();
        for (int i = 0; i < states.Length; i++)
        {
            float exact = Mathf.Clamp01(states[i].orangePercent) * states[i].stateVotes;
            float rem = exact - Mathf.FloorToInt(exact);
            remainders.Add((i, rem));
        }

        if (remainderToAssign > 0)
        {
            foreach (var item in remainders.OrderByDescending(x => x.rem).Take(remainderToAssign))
                perStateOrange[item.idx] += 1;
        }
        else if (remainderToAssign < 0)
        {
            foreach (var item in remainders.OrderBy(x => x.rem).Take(-remainderToAssign))
                perStateOrange[item.idx] = Mathf.Max(0, perStateOrange[item.idx] - 1);
        }

        // final tally
        for (int i = 0; i < states.Length; i++)
        {
            int orangeForState = Mathf.Clamp(perStateOrange[i], 0, states[i].stateVotes);
            int purpleForState = states[i].stateVotes - orangeForState;
            orangeVT += orangeForState;
            purpleVT += purpleForState;
        }

        Debug.Log($"CountVotes -> Orange: {orangeVT}, Purple: {purpleVT}, TotalStatesVotes: {nationalTotal}");
    }
}
