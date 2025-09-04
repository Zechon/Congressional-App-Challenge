using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class StaffOptionsDisplay : MonoBehaviour
{
    public StaffSelector sSelector;
    void Start()
    {
        Dictionary<CampaignRole, List<StaffData>> staffChoices = sSelector.GetStaffChoices(3);

        GameObject[] comCans = GameObject.FindGameObjectsWithTag("ComCandidateOP").OrderBy(go => go.name).ToArray();
        GameObject[] finCans = GameObject.FindGameObjectsWithTag("FinCandidateOP").OrderBy(go => go.name).ToArray();
        GameObject[] fieCans = GameObject.FindGameObjectsWithTag("FieCandidateOP").OrderBy(go => go.name).ToArray();

        UpdateCandidateUI(CampaignRole.Communications, staffChoices, comCans);
        UpdateCandidateUI(CampaignRole.Finance, staffChoices, finCans);
        UpdateCandidateUI(CampaignRole.Field, staffChoices, fieCans);
    }

    void UpdateCandidateUI(CampaignRole role, Dictionary<CampaignRole, List<StaffData>> staffChoices, GameObject[] candidateObjects)
    {
        List<StaffData> staffList = staffChoices[role];

        for (int i = 0; i < staffList.Count; i++)
        {
            StaffData candidate = staffList[i];
            GameObject obj = candidateObjects[i];

            TMP_Text nameText = obj.transform.Find("Name")?.GetComponent<TMP_Text>();
            TMP_Text descText = obj.transform.Find("Desc")?.GetComponent<TMP_Text>();
            TMP_Text stat1Text = obj.transform.Find("Stat1")?.GetComponent<TMP_Text>();
            TMP_Text skillText = obj.transform.Find("Skill")?.GetComponent<TMP_Text>();
            TMP_Text costText = obj.transform.Find("Cost")?.GetComponent<TMP_Text>();

            if (nameText != null) nameText.text = candidate.staffName;
            if (descText != null) descText.text = candidate.description;
            if (stat1Text != null) stat1Text.text = candidate.stat1.ToString();
            if (skillText != null) skillText.text = candidate.skill.ToString();
            if (costText != null) costText.text = candidate.cost.ToString();
        }
    }
}
