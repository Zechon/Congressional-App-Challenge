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
    }

    public List<StaffData> allStaff;

    public Dictionary<CampaignRole, List<StaffData>> GetStaffChoices(int countPerRole = 3)
    {
        Dictionary<CampaignRole, List<StaffData>> choices = new Dictionary<CampaignRole, List<StaffData>>();
        HashSet<StaffData> selectedStaffGlobal = new HashSet<StaffData>();

        bool valid = false;
        int safetyCounter = 0;

        while (!valid && safetyCounter < 100)
        {
            safetyCounter++;
            choices.Clear();
            selectedStaffGlobal.Clear();

            foreach (CampaignRole role in System.Enum.GetValues(typeof(CampaignRole)))
            {
                List<StaffData> pool = allStaff.Where(s => s.role == role && !selectedStaffGlobal.Contains(s)).ToList();
                List<StaffData> randomChoices = new List<StaffData>();

                int picks = Mathf.Min(countPerRole, pool.Count);
                for (int i = 0; i < picks; i++)
                {
                    int index = SeedManager.NextInt(0, pool.Count);
                    randomChoices.Add(pool[index]);
                    selectedStaffGlobal.Add(pool[index]);
                    pool.RemoveAt(index);
                }

                choices[role] = randomChoices;
            }

            // Level 5 injection
            if (guaranteeLevel5 || SeedManager.NextFloat() < chanceOfLevel5)
            {
                CampaignRole targetRole = (CampaignRole)System.Enum.GetValues(typeof(CampaignRole))
                                            .GetValue(SeedManager.NextInt(0, System.Enum.GetValues(typeof(CampaignRole)).Length));

                var level5Pool = allStaff.Where(s => s.role == targetRole && s.skill == 5 && !selectedStaffGlobal.Contains(s)).ToList();

                if (level5Pool.Count > 0)
                {
                    int replaceIndex = SeedManager.NextInt(0, choices[targetRole].Count);
                    var injected = level5Pool[SeedManager.NextInt(0, level5Pool.Count)];

                    selectedStaffGlobal.Remove(choices[targetRole][replaceIndex]);
                    choices[targetRole][replaceIndex] = injected;
                    selectedStaffGlobal.Add(injected);

                    Debug.Log($"⭐ Injected guaranteed Level 5 into {targetRole}: {injected.staffName}");
                }
            }

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
