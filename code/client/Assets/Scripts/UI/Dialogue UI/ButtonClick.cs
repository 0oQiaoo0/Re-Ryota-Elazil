using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite selectButton;
    public Sprite unselectButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().sprite = selectButton;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().sprite = unselectButton;
    }
}
