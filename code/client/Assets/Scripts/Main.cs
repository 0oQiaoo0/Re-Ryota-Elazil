using System;
using UI;
using UnityEngine;

namespace Game
{
    
    [DefaultExecutionOrder(-1)]
    public class Main : MonoBehaviour
    {
        private static Main _instance;

        public UIManager UIManager = new();

        public GlobalSetting GlobalSetting;

        public UIRenderGraphicRaycasterProxy UIRenderGraphicRaycasterProxy;

        public Camera GameCamera;

        public Camera ComputerUICamera;

        public Camera GameUICamera;

        public TipsItem TipsItem;

        public TalkDialog TalkDialog;

        public MouseOpter MouseOpter;

        public CameraOpter CameraOpter;
        
        public StateManager StateManager;

        public GameObject CloseBtn;

        public GameProcessController GameProcessController;
        
        
        private void Awake()
        {
            StateManager = new();
            _instance = this;
            UIManager.tipsItem = TipsItem;
            StateManager.Init(this);
            StateManager.ChangeToState(StateType.Scene);
        }

        public static Main Get()
        {
            return _instance;
        }

    }
}