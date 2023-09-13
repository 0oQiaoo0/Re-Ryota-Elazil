using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Renders
{
    public class PostProcessExRenderObjects : ScriptableRendererFeature
    {
        private PostProcessExPass _processExPass;

        public RenderPassEvent PassEvent;

        public Material material;

        public LayerMask cameraLayer;

        public bool IsEnable = false;

        public override void Create()
        {
            _processExPass = new( PassEvent,material);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if(IsEnable && (cameraLayer.value & (1 << renderingData.cameraData.camera.gameObject.layer)) !=0 )
                renderer.EnqueuePass(_processExPass);
        }
        
        

    }
}
