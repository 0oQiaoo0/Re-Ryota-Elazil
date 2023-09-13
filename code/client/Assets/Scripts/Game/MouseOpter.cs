using System;
using UnityEngine;

namespace Game
{
    public class MouseOpter : MonoBehaviour
    {
        public LayerMask mouseLayer;

        private IMouseInterface lastInterface;

        private bool isEnable = true;
        private bool isEnableCamerMove = true;

        private int frame = -1;

        public void EnableMouseOpter()
        {
            isEnable = true;
        }

        public void EnableMouseOpterByNextFrame()
        {
            frame = 1;
        }
        
        public void DisableMouseOpter()
        {
            isEnable = false;
            frame = -1;
        }

        public void EnableCameraMove()
        {
            isEnableCamerMove = true;
        }
        public void DisableCameraMove()
        {
            isEnableCamerMove = false;
        }
        
        private void Update()
        {
            if (!isEnable)
            {
                if (frame == 0)
                {
                    isEnable = true;
                    frame = -1;
                }
                   
                
                if (frame > 0)
                    frame--;
                
                lastInterface?.OnExit();
                lastInterface = null;
                return;
            }

            Ray ray = Main.Get().GameCamera.ScreenPointToRay(Input.mousePosition);
            float distance = 1000f;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, mouseLayer.value))
            {
                IMouseInterface mouseInterface = hitInfo.transform.GetComponent<IMouseInterface>();
                if ( mouseInterface != null) 
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                    {
                        mouseInterface.OnDown();
                    }
                    
                    if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                    {
                        mouseInterface.OnUp();
                    }
                    
                    
                }

                if (lastInterface != mouseInterface)
                {
                    lastInterface?.OnExit();
                    mouseInterface?.OnEnter();
                }
                
                lastInterface = mouseInterface;
            }
            else
            {
                if (lastInterface != null)
                {
                    lastInterface.OnExit();
                    lastInterface = null;
                }
            }

            if (isEnableCamerMove)
            {
                Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                Vector2 direction = (Vector2)Input.mousePosition - center;
                direction.Normalize();

                Main.Get().CameraOpter.MoveGameCamera(direction);
            }
        }
        
        
        
    }
}