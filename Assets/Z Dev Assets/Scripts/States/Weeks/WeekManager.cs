using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekManager : MonoBehaviour
{
    public static WeekManager Instance;
    public List<WeekSlot> weekSlots = new List<WeekSlot>();

    public GameObject weekSlotPrefab;
    public Transform weekSlotContainer;

    void Start()
    {
        foreach (var slot in weekSlots)
        {
            var slotObj = Instantiate(weekSlotPrefab, weekSlotContainer);
            slotObj.GetComponent<WeekSlotUI>().Init(slot);
        }
    }
    private void Awake()
    {
        Instance = this;

        weekSlots.Add(new WeekSlot { weekName = "Week 1" });
        weekSlots.Add(new WeekSlot { weekName = "Week 2" });
        weekSlots.Add(new WeekSlot { weekName = "Week 3" });
        weekSlots.Add(new WeekSlot { weekName = "Press Conference", isLocked = true });
    }
}
