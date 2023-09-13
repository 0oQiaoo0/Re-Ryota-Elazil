using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ImageDrop : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
    {
        public Action<Vector3> onDropEvent;
        public RectTransform rectTransform;
        public Vector3 lastPos;
        public Vector2 lastPos2;

        public bool convertWorldOrLocal = true;

        public bool isDrag = false;
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }


        public void SetRectransformAndConvertType(RectTransform rect,bool worldOrLocal)
        {
            rectTransform = rect;
            convertWorldOrLocal = worldOrLocal;
        }

        private void ConvertPos(PointerEventData eventData)
        {
            eventData.position = UIRenderGraphicRaycasterProxy.ConvertPosition(eventData.position);
            // var UIRenderGraphicRaycasterProxy = Game.Main.Get().UIRenderGraphicRaycasterProxy;
            // if (UIRenderGraphicRaycasterProxy != null && UIRenderGraphicRaycasterProxy.enabled)
            // {
            //     eventData.position += UIRenderGraphicRaycasterProxy.uiOffset;
            //     eventData.position *= UIRenderGraphicRaycasterProxy.uiScaler;
            // }

        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!eventData.pointerCurrentRaycast.isValid)
                return;
            isDrag = true;
            var sourcePos = eventData.position;
            ConvertPos(eventData);  
            if (convertWorldOrLocal)
            {
                Vector3 uiPos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position,
                    eventData.enterEventCamera, out uiPos))
                    lastPos = uiPos;
            }
            else
            {      
                Vector2 uiPos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position,
                eventData.enterEventCamera, out uiPos))
                    lastPos2 = uiPos;
                    
            }
            eventData.position = sourcePos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!eventData.pointerCurrentRaycast.isValid)
                return;
            if (!isDrag)
                return;
            var sourcePos = eventData.position;
            ConvertPos(eventData);  
            if (convertWorldOrLocal)
            {
                Vector3 uiPos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position,
                    eventData.enterEventCamera, out uiPos))
                {
                    onDropEvent.Invoke(uiPos - lastPos);
                    lastPos = uiPos;
                }
            }
            else
            {
                Vector2 uiPos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position,
                    eventData.enterEventCamera, out uiPos))
                {
                    onDropEvent.Invoke(uiPos - lastPos2);
                    lastPos2 = uiPos;
                }
            }
            eventData.position = sourcePos;
        }
    }
}