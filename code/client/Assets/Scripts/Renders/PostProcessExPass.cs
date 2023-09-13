using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Renders
{
    public class PostProcessExPass : ScriptableRenderPass
    {
        private Material postMaterial;

        private RenderTargetHandle m_CameraColorAttachment;

        private RenderTargetHandle m_CameraTransparentColorAttachment;
        
        private ProfilingSampler _profilingSampler = new("PostProcessExPass ProfilingScope");
        public PostProcessExPass(RenderPassEvent passEvent, Material material)
        {
            renderPassEvent = passEvent;
            postMaterial = material;
            
            m_CameraColorAttachment.Init("_CameraColorTexture");
            m_CameraTransparentColorAttachment.Init("_CameraTransparentColorTexture");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);
            
            var des = cameraTextureDescriptor;
            des.depthBufferBits = 0;
            cmd.GetTemporaryRT(m_CameraTransparentColorAttachment.id, des, FilterMode.Point);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            
            CommandBuffer commandBuffer = CommandBufferPool.Get("PostProcessExPass");
            using (new ProfilingScope(commandBuffer, _profilingSampler))
            {
                // m_CameraColorAttachment = RenderTargetHandle.CameraTarget;
                // postMaterial.SetTexture("",m_CameraColorAttachment.Identifier());
                commandBuffer.SetGlobalTexture("_MainTex",m_CameraColorAttachment.id);

                // Blit(commandBuffer,ref renderingData,postMaterial,0);
                
                // postMaterial.SetTexture("",m_CameraColorAttachment.Identifier());
                commandBuffer.Blit(m_CameraColorAttachment.id, m_CameraTransparentColorAttachment.Identifier(), postMaterial);
                commandBuffer.Blit(m_CameraTransparentColorAttachment.Identifier(), m_CameraColorAttachment.id);
                
            
                // SetupBloom(cmd, GetSource(), m_Materials.uber);
            }
               
            // commandBuffer.Blit();
            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
            
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            base.FrameCleanup(cmd);
            
            if(m_CameraTransparentColorAttachment != RenderTargetHandle.CameraTarget)
                cmd.ReleaseTemporaryRT(m_CameraTransparentColorAttachment.id);
        }
    }
}