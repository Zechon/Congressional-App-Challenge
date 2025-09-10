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
        HashSet<StaffData> selectedStaffGlobal = new HashSet<StaffData>(); // track all picked staff

        bool valid = false;
        int safetyCounter = 0;

        while (!valid && safetyCounter < 100)
        {
            safetyCounter++;
            choices.Clear();
            selectedStaffGlobal.Clear();

            // Step 1: Randomly select staff per role without duplicates globally
            foreach (CampaignRole role in System.Enum.GetValues(typeof(CampaignRole)))
            {
                List<StaffData> pool = allStaff.Where(s => s.role == role && !selectedStaffGlobal.Contains(s)).ToList();
                List<StaffData> randomChoices = new List<StaffData>();

                int picks = Mathf.Min(countPerRole, pool.Count);
                for (int i = 0; i < picks; i++)
                {
                    int index = Random.Range(0, pool.Count);
                    randomChoices.Add(pool[index]);
                    selectedStaffGlobal.Add(pool[index]); // mark globally as picked
                    pool.RemoveAt(index); // remove to prevent picking again in this role
                }

                choices[role] = randomChoices;
            }

            // Step 2: Inject guaranteed or chance-based level 5
            if (guaranteeLevel5 || Random.value < chanceOfLevel5)
            {
                CampaignRole targetRole = (CampaignRole)System.Enum.GetValues(typeof(CampaignRole))
                                            .GetValue(Random.Range(0, System.Enum.GetValues(typeof(CampaignRole)).Length));

                var level5Pool = allStaff.Where(s => s.role == targetRole && s.skill == 5 && !selectedStaffGlobal.Contains(s)).ToList();

                if (level5Pool.Count > 0)
                {
                    int replaceIndex = Random.Range(0, choices[targetRole].Count);
                    var injected = level5Pool[Random.Range(0, level5Pool.Count)];

                    selectedStaffGlobal.Remove(choices[targetRole][replaceIndex]); // remove old pick from global set
                    choices[targetRole][replaceIndex] = injected;
                    selectedStaffGlobal.Add(injected);

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
                Debug.Log($"Found {validCombos.Count} valid combo(s) in the 8–10 range:");
            }
        }

        if (!valid)
            Debug.LogWarning("Could not generate valid staff choices within safety counter.");

        return choices;
    }


}
