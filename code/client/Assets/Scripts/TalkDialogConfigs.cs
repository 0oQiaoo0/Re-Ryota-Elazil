using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Assets/Resources/TalkDialogConfigs",fileName = "TalkDialogConfigs.asset")]
    public class TalkDialogConfigs : ScriptableObject,ISerializationCallbackReceiver
    {

        public List<TalkDialogConfig> DialogConfigs = new();

        private Dictionary<int, TalkDialogConfig> dict = new();

        public void OnBeforeSerialize()
        {
        }

        public TalkDialogConfig GetTalkDialogConfig(int id)
        {
            return dict.GetValue(id);
        }

        public void OnAfterDeserialize()
        {
            dict.Clear();

            foreach (var talkDialogConfig in DialogConfigs)
            {
                if(!dict.ContainsKey(talkDialogConfig.dialogId))
                    dict.Add(talkDialogConfig.dialogId,talkDialogConfig);
            }
        }
    }
    
    [Serializable]
    public class TalkDialogConfig
    {
        public string desc
        {
            get => $"{dialogId}";
        }
        public int dialogId;
        
        public List<TalkContext> talks = new ();

    }
    
    [Serializable]
    public class TalkContext
    {
        public string context;
    }
    
    
}