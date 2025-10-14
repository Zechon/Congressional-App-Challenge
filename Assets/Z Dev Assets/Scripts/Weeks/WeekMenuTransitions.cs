using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeekMenuTransitions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Other References")]
    public GameObject Tab;
    private Animator tabAnim;
    public GameObject ActPnl;

    private Image btnImg;
    private Animator btnAnim;

    private void Start()
    {
        tabAnim = Tab.GetComponent<Animator>();
        btnImg = GetComponent<Image>();
        btnAnim = GetComponent<Animator>();
    }

    public void StateMenuRiseFall()
    {
        if (tabAnim.GetBool("Rising") == false)
        {
            tabAnim.SetBool("Rising", true);
            btnAnim.SetTrigger("Switch");
        }

        else
        {
            tabAnim.SetBool("Rising", false);
            btnAnim.SetTrigger("Switch");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btnAnim.GetBool("Hover") == false)
            btnAnim.SetBool("Hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (btnAnim.GetBool("Hover") == true)
            btnAnim.SetBool("Hover", false);
    }
}
