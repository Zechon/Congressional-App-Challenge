using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;

public class StateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private StateSetup s;
    public string stateName;

    [Header("Animation Settings")]
    public float hoverScale = 1.1f;
    public float bounceSpeed = 5f;

    private Vector3 originalScale;
    private bool isHovered = false;
    private bool isSelected = false;

    void Start()
    {
        originalScale = transform.localScale;
        s = GetComponent<StateSetup>();
        stateName = s.stateName;
    }

    void Update()
    {
        if (isHovered && !isSelected)
        {
            float bounce = Mathf.Sin(Time.time * bounceSpeed) * 0.05f;
            transform.localScale = originalScale * hoverScale + new Vector3(bounce, bounce, 0f);
        }
        else if (!isSelected)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 8f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            isHovered = true;
            StateUIManager.Instance.ShowStateName(stateName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            isHovered = false;
            StateUIManager.Instance.HideStateName();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;

        if (isSelected)
        {
            StateUIManager.Instance.SelectState(this);
        }
        else
        {
            StateUIManager.Instance.DeselectState();
        }
    }

    public void ForceDeselect()
    {
        isSelected = false;
        isHovered = false;
        transform.localScale = originalScale;
    }
}

