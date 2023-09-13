using System;
using UnityEngine;

namespace UI
{
    public class TestLogic : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
            {
                ComputerDesktopWindow.Instance?.OpenWindows(WindowDefine.PhotoWindow,"补充素材/聊天发送的图片/第一日/羽毛");
            }
            
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
            {
                Game.Main.Get().GameProcessController.ForceToNextDay();
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M))
            {
                Game.Main.Get().GameProcessController.FinishAll();
            }
            
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M))
            {
                Game.Main.Get().GameProcessController.TryGameOver();
            }

        }
    }
}