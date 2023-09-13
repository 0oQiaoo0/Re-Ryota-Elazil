using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleByInterface : MonoBehaviour,IDragHandler
{
    public GameObject scaledObject;
    public float minX;
    public float minY;
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 tmp = new Vector2(eventData.delta.x, -eventData.delta.y);
        if (scaledObject.GetComponent<RectTransform>().sizeDelta.x < minX && tmp.x < 0) tmp.x = 0;
        if (scaledObject.GetComponent<RectTransform>().sizeDelta.y < minY && tmp.y < 0) tmp.y = 0;
        scaledObject.GetComponent<RectTransform>().sizeDelta += tmp;
    }
}
