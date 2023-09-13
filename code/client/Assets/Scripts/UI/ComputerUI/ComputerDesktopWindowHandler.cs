using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ComputerDesktopWindowHandler : MonoBehaviour
    {

        public Button button;

        public GameObject selected;
        
        public DestktopWindowToken token;

        public Action<ComputerDesktopWindowHandler> clickHandler;

        public bool selectState;
        
        private void Awake()
        {
            button = GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
            selectState = false;
        }

        public void SetSelectState(bool state)
        {
            selectState = state;
            selected.SetActive(state);
        }
        
        public bool GetSelectState()
        {
            return selectState;
        }
        
        private void OnClick()
        {
            clickHandler?.Invoke(this);
        }

        public void AddClickEvent(Action<ComputerDesktopWindowHandler> onDesktopHandlerIconClick)
        {
            clickHandler = onDesktopHandlerIconClick;
        }

        public void OnClose()
        {
        }
    }
}