using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class CustomGraphicRaycaster : UnityEngine.UI.GraphicRaycaster
    {
        public Vector2 scaler = Vector2.one;
        public Vector2 offset = Vector2.zero;

        public Vector2 outPos;
        public Vector2 eventPos;
        
        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            Vector2 sourcePos = eventData.position;
            eventPos = eventData.position;
            eventData.position = UIRenderGraphicRaycasterProxy.ConvertPosition(eventData.position);
            // var UIRenderGraphicRaycasterProxy = Game.Main.Get().UIRenderGraphicRaycasterProxy;
            // if (UIRenderGraphicRaycasterProxy != null && UIRenderGraphicRaycasterProxy.enabled)
            // {
            //     eventData.position += UIRenderGraphicRaycasterProxy.uiOffset;
            //     eventData.position *= UIRenderGraphicRaycasterProxy.uiScaler;
            // }

            outPos = eventData.position;
            base.Raycast(eventData, resultAppendList);
            eventData.position = sourcePos;
        }
        
    }
}