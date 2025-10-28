using UnityEngine;
using System.Text;
using TMPro;

public class OpponentTestController : MonoBehaviour
{
    public DefaultActionDatabase defaultActionDatabase;
    public Difficulty testDifficulty = Difficulty.Normal;
    public int monthsToSimulate = 3;

    public TMP_Text reportText; // Or TMP_Text if using TextMeshPro

    private void Start()
    {
        SeedManager.GenerateSeed(testDifficulty);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Generated seed: {SeedManager.Seed} | Difficulty: {SeedManager.CurrentDifficulty}");
        sb.AppendLine();

        for (int month = 1; month <= monthsToSimulate; month++)
        {
            var report = OpponentMonthSummary.GenerateMonthReport(month, defaultActionDatabase);

            sb.AppendLine($"===== Month {report.monthNumber} Opponent Report =====");
            foreach (var action in report.actions)
                sb.AppendLine($"• {action}");
            sb.AppendLine($"Debate: {report.debateResult}");
            sb.AppendLine("=====================================");
            sb.AppendLine();
        }

        reportText.text = sb.ToString();
    }
}
