using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 cachedScale;
    TextMeshProUGUI buttonText;

    void Start()
    {
        cachedScale = transform.localScale;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        if (buttonText != null)
        {
            buttonText.color = Color.white;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = cachedScale;
        if (buttonText != null)
        {
            buttonText.color = Color.black;
        }
    }
}
