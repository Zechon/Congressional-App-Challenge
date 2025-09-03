using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class StaffSelector : MonoBehaviour
{
    public List<StaffData> allStaff;

    public Dictionary<CampaignRole, List<StaffData>> GetStaffChoices(int countPerRole = 3)
    {
        Dictionary<CampaignRole, List<StaffData>> choices = new Dictionary<CampaignRole, List<StaffData>>();

        bool valid = false;
        int safetyCounter = 0;

        while (!valid && safetyCounter < 100)
        {
            safetyCounter++;
            choices.Clear();

            // Step 1: Randomly select 3 staff per role
            foreach (CampaignRole role in System.Enum.GetValues(typeof(CampaignRole)))
            {
                List<StaffData> pool = allStaff.Where(s => s.role == role).ToList();
                List<StaffData> randomChoices = pool.OrderBy(x => Random.value).Take(countPerRole).ToList();
                choices[role] = randomChoices;

                //Debug
                string roleList = string.Join(", ", randomChoices.Select(s => $"{s.staffName} (Cost: {s.cost}, Skill: {s.skill})"));
                Debug.Log($"{role} choices: {roleList}");
            }

            // Step 2: Build all 27 possible combos
            List<int> comboCosts = new List<int>();
            List<string> validCombos = new List<string>();

            foreach (var f in choices[CampaignRole.Finance])
            {
                foreach (var c in choices[CampaignRole.Communications])
                {
                    foreach (var fl in choices[CampaignRole.Field])
                    {
                        int totalCost = f.cost + c.cost + fl.cost;
                        comboCosts.Add(totalCost);
                    }
                }
            }

            // Step 3: Check if any combo is within 8–9
            if (comboCosts.Any(cost => cost >= 8 && cost <= 9))
            {
                valid = true;

                //Debug
                Debug.Log($"✅ Found {validCombos.Count} valid combo(s) in the 8–10 range:");
                foreach (string combo in validCombos)
                    Debug.Log(combo);
            }
        }

        if (!valid)
            Debug.LogWarning("Could not generate valid staff choices within safety counter."); //I really hope this never happens.

        return choices;
    }
}
