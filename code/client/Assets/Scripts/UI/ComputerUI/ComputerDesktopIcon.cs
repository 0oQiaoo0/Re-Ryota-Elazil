using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ComputerDesktopIcon : MonoBehaviour
    {

        private Button button;

        private Action<ComputerDesktopIcon> clickHandler;

        public WindowDefine ComputerWindow;
        
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            clickHandler.Invoke(this);
        }


        public void AddClickEvent(Action<ComputerDesktopIcon> onDesktopIconClick)
        {
            clickHandler = onDesktopIconClick;
        }
        
    }
}