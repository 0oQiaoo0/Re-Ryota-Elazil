using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class GameSceneInteraction : MonoBehaviour,IMouseInterface
    {
        public int talk_id;

        public bool showTips = true;

        public string tipDesc = "";

        public UnityEvent ActionEndEvent;

        [NonSerialized]
        public int todayShowCount = -1;
        
        public int showCount = -1;

        public int dayCache = -1;
        
        public bool mouseEnterHookCameraMove = false;
        
        public void OnEnter()
        {
            if(mouseEnterHookCameraMove)
                Main.Get().MouseOpter.DisableCameraMove();
            if (Main.Get().GlobalSetting.cursor == null)
                return;
            
            Cursor.SetCursor(Main.Get().GlobalSetting.cursor,Vector2.zero,CursorMode.Auto);
            if (showTips)
            {
                Main.Get().UIManager.ShowTipsItem(this.gameObject,tipDesc);
            }
               
        }

        private void OnDrawGizmos()
        {
            
        }


        public void OnExit()
        {
            if(mouseEnterHookCameraMove)
                Main.Get().MouseOpter.EnableCameraMove();
            Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
            if(showTips)
                Main.Get().UIManager.CloseTipsItem();
        }

        public void OnDown()
        {
        }

        protected virtual void OnEnd()
        {
            ActionEndEvent?.Invoke();
        }

        private void OnCloseDialog()
        {
            OnEnd();
        }

        protected virtual int GetTalkId()
        {
            return talk_id;
        }

        private void TryResetToDayShowCount()
        {
            if (showCount == -1)
            {
                todayShowCount = showCount;
                return;
            }
             
            if (dayCache != Main.Get().GameProcessController.curDay)
            {
                todayShowCount = showCount;
                dayCache = Main.Get().GameProcessController.curDay;
            }
        }

        public void OnUp()
        {
            TryResetToDayShowCount();
            
            if (todayShowCount == -1 || todayShowCount > 0)
            {
                if (todayShowCount > 0)
                    todayShowCount--;

                if (GetTalkId() > 0)
                    Main.Get().UIManager.ShowDialog(GetTalkId(), OnCloseDialog);
                else
                    OnEnd();
            }
            else
            {
                OnEnd();
            }

        }

    }
}