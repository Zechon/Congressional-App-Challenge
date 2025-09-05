using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class StaffSelector : MonoBehaviour
{
    public bool guaranteeLevel5 = false;     
    [Range(0f, 1f)] public float chanceOfLevel5 = 0.3f; 

    void Start()
    {
        var result = GetStaffChoices(3);

        //foreach (var role in result.Keys)
        //{
        //    Debug.Log($"Final {role} choices:");
        //    foreach (var staff in result[role])
        //    {
        //        Debug.Log($"- {staff.staffName} (Cost: {staff.cost}, Skill: {staff.skill})");
        //    }
        //}
    }

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

                // Debug
                //string roleList = string.Join(", ", randomChoices.Select(s => $"{s.staffName} (Cost: {s.cost}, Skill: {s.skill})"));
                //Debug.Log($"{role} choices: {roleList}");
            }

            // Step 2: Inject a guaranteed or chance-based level 5 if enabled
            if (guaranteeLevel5 || Random.value < chanceOfLevel5)
            {
                // Pick a random role to inject into
                CampaignRole targetRole = (CampaignRole)System.Enum.GetValues(typeof(CampaignRole)).GetValue(Random.Range(0, 3));

                // Find all level 5 staff in that role
                var level5Pool = allStaff.Where(s => s.role == targetRole && s.skill == 5).ToList();

                if (level5Pool.Count > 0)
                {
                    // Replace one of the existing random choices with a level 5 staffer
                    int replaceIndex = Random.Range(0, choices[targetRole].Count);
                    var injected = level5Pool[Random.Range(0, level5Pool.Count)];
                    choices[targetRole][replaceIndex] = injected;

                    Debug.Log($"⭐ Injected guaranteed Level 5 into {targetRole}: {injected.staffName}");
                }
            }

            // Step 3: Check if at least one combo is within 8–10
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

                        if (totalCost >= 8 && totalCost <= 10)
                            validCombos.Add($"{f.staffName} + {c.staffName} + {fl.staffName} = {totalCost}");
                    }
                }
            }

            if (validCombos.Count > 0)
            {
                valid = true;
                Debug.Log($"✅ Found {validCombos.Count} valid combo(s) in the 8–10 range:");
            }
        }

        if (!valid)
            Debug.LogWarning("Could not generate valid staff choices within safety counter.");

        return choices;
    }

}
