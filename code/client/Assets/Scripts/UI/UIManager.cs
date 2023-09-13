using System;
using Game;
using UnityEngine;

namespace UI
{
    public class UIManager
    {
        public TipsItem tipsItem = null;
        
        public void ShowTipsItem(GameObject go, string tipDesc)
        {
            if (tipsItem == null)
                return;
            
            tipsItem.LockGameObject(go);
            tipsItem.Open(tipDesc);
        }
        
        public void CloseTipsItem()
        {
            if (tipsItem == null)
                return;

            tipsItem.Close();
        }

        public void ShowDialog(int talkID,Action closeEvent = null,bool ignoreMouseOpter = false)
        {
            Main.Get().TalkDialog.ShowDialog(talkID,closeEvent,ignoreMouseOpter);
        }
        
        
    }
}