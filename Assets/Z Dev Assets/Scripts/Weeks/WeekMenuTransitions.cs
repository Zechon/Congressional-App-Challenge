using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeekMenuTransitions : MonoBehaviour
{
    [Header("References")]
    public GameObject ActPnl;
    public Animator anim;
    public GameObject WPnl;
    public Animator wAnim;

    public bool actMenu = false;

    private void Start()
    {
       anim = ActPnl.GetComponent<Animator>();
       wAnim = WPnl.GetComponent<Animator>();

       StartCoroutine(StartTimer());
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1);
        WeekSwitch();
    }

    public void ActionMenuOn()
        { 
            if (StateUIManager.Instance != null && ActionPanelUI.instance != null)
            {
                var selectedState = StateUIManager.Instance.GetCurrentSelectedState();

                if (selectedState != null)
                {
                    ActionPanelUI.instance.Show(selectedState);
                }
                else
                {
                    Debug.LogWarning("No state is currently selected — cannot open Action Panel.");
                    actMenu = false;
                    return;
                }
            }

            ActPnl.SetActive(true);
        }

    public void ActionMenuOff()
    {
        ActionPanelUI.instance.Hide();

        ActPnl.SetActive(false);
    }

    public void WeekSwitch()
    {
        wAnim.SetTrigger("Switch");
    }
}
