using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{


    public class TalkDialog : MonoBehaviour
    {

        private TalkDialogConfig TalkDialogConfig;

        public TextMeshProUGUI desc;

        public Button nextButton;
        
        public int talkIndex = 0;

        public Action closeEvent;


        public bool ignoreMouseOpter;
        
        private void Awake()
        {
            nextButton.onClick.AddListener(ShowNextTalk);
        }

        public void ShowDialog(int id, Action ce,bool ignoreMouseOpter = true)
        {
            closeEvent = ce;
            TalkDialogConfig = Main.Get().GlobalSetting.TalkDialogConfigs.GetTalkDialogConfig(id);
            if (TalkDialogConfig == null)
                return;
            
            this.gameObject.SetActive(true);

            gameObject.SetActive(true);
            this.ignoreMouseOpter = ignoreMouseOpter;
            if(!ignoreMouseOpter)
                Main.Get().MouseOpter.DisableMouseOpter();
            
            ResetCache();
            ShowNextTalk();
        }

        private void ResetCache()
        {
            talkIndex = 0;
        }

        private void ShowNextTalk()
        {
            if (TalkDialogConfig.talks.Count <= talkIndex)
            {
                Close();
                return;
            }

            TalkContext talkContext =  TalkDialogConfig.talks[talkIndex];
            desc.text = talkContext.context;
            talkIndex++;

        }

        public void Close()
        {
            
            this.gameObject.SetActive(false);
            
            if(!this.ignoreMouseOpter)
                Main.Get().MouseOpter.EnableMouseOpterByNextFrame();
            
            closeEvent?.Invoke();
        }
        

    }
}
