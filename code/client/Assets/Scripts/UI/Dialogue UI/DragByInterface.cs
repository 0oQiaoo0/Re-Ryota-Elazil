using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragByInterface : MonoBehaviour,IDragHandler
{
    public GameObject draggedObject;

    public void OnDrag(PointerEventData eventData)
    {
        draggedObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }


}
