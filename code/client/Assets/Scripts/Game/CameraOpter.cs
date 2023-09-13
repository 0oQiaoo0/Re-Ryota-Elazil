using System;
using Cinemachine;
using UnityEngine;

namespace Game
{
    public class CameraOpter : MonoBehaviour
    {

        public CinemachineVirtualCamera gameVirtualCamera;
        public CinemachineVirtualCamera computerVirtualCamera;
        public CinemachineBlendListCamera blendVitualCameras;

        private CinemachineConfiner cinemachineConfiner;

        public Vector2 moveDirection = Vector2.zero;

        public Vector2 moveSpeed = Vector2.one;

        private void Awake()
        {
            EnableGameCamera();
        }

        public void EnableGameCamera()
        {
            EnableVirtualCamera(gameVirtualCamera);
        }
        
        
        public void EnableComputerCamera()
        {
            EnableVirtualCamera(computerVirtualCamera);
        }

        private void EnableVirtualCamera(CinemachineVirtualCamera cinemachineVirtualCamera)
        {
            blendVitualCameras.m_Instructions = new CinemachineBlendListCamera.Instruction[1]
            {
                new CinemachineBlendListCamera.Instruction()
                {
                     m_VirtualCamera = cinemachineVirtualCamera,
                     m_Blend = new CinemachineBlendDefinition( CinemachineBlendDefinition.Style.EaseInOut,1f),
                     m_Hold = 9999f,
                }
                
            };

        }

        public void MoveGameCamera(Vector2 direction)
        {
            moveDirection = direction;
        }

        private void LateUpdate()
        {
            var gameTrans = gameVirtualCamera.transform;
            Vector3 rightMovePos = gameTrans.right * (moveDirection.x * Time.deltaTime * moveSpeed.x);
            Vector3 upMovePos = gameTrans.up * (moveDirection.y * Time.deltaTime * moveSpeed.y);
            gameVirtualCamera.ForceCameraPosition(Main.Get().GameCamera.transform.position + upMovePos + rightMovePos,gameTrans.rotation);
            moveDirection = Vector2.zero;
        }
    }
}