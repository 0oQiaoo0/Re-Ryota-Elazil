using System;
using Game;
using UnityEngine;

namespace UI
{
    public class UIRenderGraphicRaycasterProxy : MonoBehaviour
    {
        public RenderTexture uiRender;
        
        public Camera camera;
        public float size = 5f;
        public Vector3 screenPos1;
        public Vector3 screenPos2;
        public Vector3 screenPos3;
        public Vector3 screenPos4;

        [SerializeField]
        private Vector2 uiScaler = Vector2.one;
        
        [SerializeField]
        private Vector2 uiOffset = Vector2.zero;

        private void Awake()
        {
            Main.Get().UIRenderGraphicRaycasterProxy = this;
        }

        public static Vector2 ConvertPosition(Vector2 position)
        {
            var UIRenderGraphicRaycasterProxy = Game.Main.Get().UIRenderGraphicRaycasterProxy;
            if (UIRenderGraphicRaycasterProxy != null && UIRenderGraphicRaycasterProxy.enabled)
            {
                position += UIRenderGraphicRaycasterProxy.uiOffset;
                position *= UIRenderGraphicRaycasterProxy.uiScaler;
            }

            return position;
        }

        public void Update()
        {
            if (camera == null)
                return;
            
            if (uiRender == null)
                return;

            Vector3 p1 =  this.transform.TransformPoint(new Vector3(size,0,-size));
            Vector3 p2 =  this.transform.TransformPoint(new Vector3(size,0,size));
            Vector3 p3 =  this.transform.TransformPoint(new Vector3(-size,0,-size));
            Vector3 p4 =  this.transform.TransformPoint(new Vector3(-size,0,size));
           
            screenPos1 = camera.WorldToScreenPoint(p1);
            screenPos2 = camera.WorldToScreenPoint(p2);
            screenPos3 = camera.WorldToScreenPoint(p3);
            screenPos4 = camera.WorldToScreenPoint(p4);

            Vector2 min = Vector2.Min(screenPos1, screenPos2);
            min = Vector2.Min(min, screenPos3);
            min = Vector2.Min(min, screenPos4);
            
            Vector2 max = Vector2.Max(screenPos1, screenPos2);
            max = Vector2.Max(max, screenPos3);
            max = Vector2.Max(max, screenPos4);

            Vector2 uiSize = (max - min);
            uiScaler.x = 1.0f / uiSize.x * Screen.width;
            uiScaler.y = 1.0f / uiSize.y * Screen.height;

            uiScaler.x *= uiRender.width * 1f / Screen.width;
            uiScaler.y *= uiRender.height * 1f  / Screen.height;

            uiOffset = -min;

        }
    }
}