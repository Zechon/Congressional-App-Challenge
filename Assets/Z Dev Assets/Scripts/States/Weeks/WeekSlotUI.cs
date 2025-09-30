using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeekSlotUI : MonoBehaviour
{
    public TMP_Text weekLabel;
    public TMP_Text actionText;
    public Button assignButton;

    public GameObject tablet;
    public Animator anim;

    private WeekSlot linkedSlot;

    private void Start()
    {
        anim = tablet.GetComponent<Animator>();
        anim.SetInteger("Week", 1);
    }

    public void Init(WeekSlot slot)
    {
        linkedSlot = slot;
        weekLabel.text = slot.weekName;
        Refresh();

        assignButton.onClick.AddListener(() => OnAssignClicked());
    }

    private void Refresh()
    {
        if (linkedSlot.isLocked)
        {
            actionText.text = "Locked (Press Conference)";
            assignButton.interactable = false;
        }
        else if (linkedSlot.assignedAction != null)
        {
            actionText.text = $"{linkedSlot.assignedAction} in {linkedSlot.assignedState.stateName}";
        }
        else
        {
            actionText.text = "No Action";
        }
    }

    private void OnAssignClicked()
    {
        // TODO: Open action selection UI, pass this slot as target
        Debug.Log($"Assigning action for {linkedSlot.weekName}");
    }
}
