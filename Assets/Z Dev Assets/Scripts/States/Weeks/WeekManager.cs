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

    }
    private void Awake()
    {

    }
}
