using System.Collections.Generic;
using UnityEngine;
using static ActionDatabase;

[System.Serializable]
public class OpponentMonthReport
{
    public int monthNumber;
    public List<string> actions;
    public string debateResult;
}

public static class OpponentMonthSummary
{
    // Example pool of states – you can swap this for your real state data later.
    private static readonly List<string> StateList = new List<string>
    {
        "California", "Texas", "Florida", "New York", "Pennsylvania", "Ohio", "Michigan",
        "Georgia", "North Carolina", "Arizona", "Wisconsin", "Virginia", "Colorado",
        "Minnesota", "Nevada", "New Jersey", "Tennessee", "Indiana", "Illinois", "Iowa"
    };

    public static OpponentMonthReport GenerateMonthReport(int monthNumber, DefaultActionDatabase database)
    {
        System.Random rng = SeedManager.GetSubRng(monthNumber + 10); // Unique per-month RNG
        List<string> actions = new List<string>();
        var availableActions = database.actions;

        int numActions = SeedManager.CurrentDifficulty switch
        {
            Difficulty.Easy => rng.Next(1, 3),
            Difficulty.Normal => rng.Next(2, 4),
            Difficulty.Hard => rng.Next(3, 5),
            _ => 2
        };

        for (int i = 0; i < numActions; i++)
        {
            CampaignAction chosen = availableActions[rng.Next(availableActions.Count)];
            string state = PickState(rng);
            string result = GenerateActionSummary(chosen, rng, state);
            actions.Add(result);
        }

        string debateSummary = GenerateDebateSummary(monthNumber, rng);

        return new OpponentMonthReport
        {
            monthNumber = monthNumber,
            actions = actions,
            debateResult = debateSummary
        };
    }

    private static string PickState(System.Random rng)
    {
        // You could later replace this with a StateManager or a weighted pick
        return StateList[rng.Next(StateList.Count)];
    }

    private static string GenerateActionSummary(CampaignAction action, System.Random rng, string state)
    {
        bool success = rng.NextDouble() <= action.successChance;

        if (!success)
            return $"Their attempt at a {action.actionName.ToLower()} in {state} failed to gain traction.";

        switch (action.category)
        {
            case EffectCategory.Money:
                return $"Held a {action.actionName} in {state}, boosting their campaign funds.";
            case EffectCategory.VoterSway:
                return $"Ran a {action.actionName} in {state}, gaining small voter support.";
            case EffectCategory.PR:
                return $"Completed a {action.actionName} in {state}, improving their public image.";
            case EffectCategory.InternalPrep:
                return $"Spent time on {action.actionName} operations in {state}, improving campaign efficiency.";
            default:
                return $"Performed a {action.actionName} in {state}.";
        }
    }

    private static string GenerateDebateSummary(int monthNumber, System.Random rng)
    {
        float roll = (float)rng.NextDouble();
        switch (SeedManager.CurrentDifficulty)
        {
            case Difficulty.Easy:
                if (roll > 0.7f) return "They stumbled during the debate, losing some public confidence.";
                if (roll > 0.4f) return "They performed adequately, but the audience favored you.";
                return "They had a surprisingly strong showing despite early missteps.";
            case Difficulty.Normal:
                if (roll > 0.6f) return "Their debate performance was solid, earning moderate support.";
                if (roll > 0.3f) return "They held their ground, neither gaining nor losing much momentum.";
                return "They were sharp and persuasive, slightly swaying undecided voters.";
            case Difficulty.Hard:
                if (roll > 0.6f) return "They delivered a confident, strategic debate performance.";
                if (roll > 0.3f) return "They were aggressive and well-prepared, gaining voter confidence.";
                return "They dominated the debate, leaving you on the defensive.";
            default:
                return "Their debate went unnoticed by most voters.";
        }
    }
}
