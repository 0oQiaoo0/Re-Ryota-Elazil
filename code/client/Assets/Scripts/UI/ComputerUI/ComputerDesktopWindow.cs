using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EventSystem = UnityEngine.EventSystems.EventSystem;

namespace UI
{
    public class ComputerDesktopWindow : UIWindow
    {

        public List<ComputerDesktopIcon> DesktopIcons = new();

        public static ComputerDesktopWindow Instance { get; private set; }

        private Dictionary<WindowDefine,DestktopWindowToken> windows = new();

        public Transform windowParent;

        public Transform windowHandlerParent;

        public ComputerDesktopWindowHandler handlerTemp;

        private List<DestktopWindowToken> topWindows = new();

        private DestktopWindowToken focusToken;

        public TextMeshProUGUI dayText;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            this.GetComponentsInChildren(DesktopIcons);

            foreach (var computerDesktopIcon in DesktopIcons)
            {
                computerDesktopIcon.AddClickEvent(OnDesktopIconClick);
            }
        }

        
        public void OnDesktopIconClick(ComputerDesktopIcon computerDesktopIcon)
        {
            // 打开对应的界面   
            OpenWindows(computerDesktopIcon.ComputerWindow);
        }

        public void OnDesktopHandlerIconClick(ComputerDesktopWindowHandler computerDesktopIcon)
        {
            var token = topWindows.Find((a) => { return a.Handler == computerDesktopIcon; });
            
            if (focusToken != token || token.Window.WindowSize == ComputerWindow.WindowSizeType.Min)
            {
                ActiveFocus(token);
            }
            else
            {   
                token.Window.SetMinWindow();
                DiscardFocus(token);
            }
          
        }
        
        private bool IsFocusTokenOrShowIn(DestktopWindowToken token)
        {
            return focusToken == token || token.Window.WindowSize != ComputerWindow.WindowSizeType.Min;
        }

        public void DiscardFocus(DestktopWindowToken token)
        {
            if (topWindows.Count >= 2)
            {
                for (var i = topWindows.Count - 1; i >= 0; i--)
                {
                    if (topWindows[i] != token)
                    {
                        ActiveFocus(topWindows[i],false);
                        break;
                    }
                }
            }
            else
            {
                token.Handler.SetSelectState(false);
                focusToken = null;
            }
        }

        public void ActiveFocus(DestktopWindowToken token)
        {
            ActiveFocus(token, true);
        }
        
        private void ActiveFocus(DestktopWindowToken token,bool isOpen)
        {
            if (focusToken != token)
            {
                topWindows.Remove(token);
                for (var i = 0; i < topWindows.Count; i++)
                    topWindows[i].Handler.SetSelectState(false);

                topWindows.Add(token);
                token.Handler.SetSelectState(true);
         

                focusToken = token;
            }
            
            if (isOpen)
            {
                token.WindowGo.transform.SetAsLastSibling();

                if (token.Window.WindowSize == ComputerWindow.WindowSizeType.Min)
                {
                    token.Window.OnOpen();
                }
            }
        }
        
        // 自顶向下,
        // 焦点 在最顶上,如果有
        // 焦点上,缩放后,切换焦点
        // 

        public void OpenWindows<T>(WindowDefine sourceWindow,T arg1)
        {
            var windowToken = InternalOpenWindows(sourceWindow);
            ActiveFocus(windowToken);
            windowToken.WindowGo.GetComponent<IWindowParams<T>>()?.OnInitArg(arg1);
            windowToken.Window.OnOpen();
        }
        
        public void OpenWindows(WindowDefine sourceWindow)
        {
            var windowToken = InternalOpenWindows(sourceWindow);
            ActiveFocus(windowToken);
            windowToken.Window.OnOpen();
        }

        private DestktopWindowToken InternalOpenWindows(WindowDefine sourceWindow)
        {
            if (!windows.TryGetValue(sourceWindow, out DestktopWindowToken windowToken))
            {
                GameObject sourceGo = Resources.Load($"UI/{sourceWindow.ToString()}" ) as GameObject;
                if (sourceGo == null)
                {
                    Debug.LogError($"Can not find {sourceWindow} by Asset/Resource/UI.");
                    return null;
                }
                
                windowToken = new();
                windowToken.WindowGo = GameObject.Instantiate(sourceGo, windowParent, false);
                windowToken.WindowName = sourceWindow;
                windowToken.Window =  windowToken.WindowGo.gameObject.GetComponentInChildren<ComputerWindow>();
                windowToken.Handler = GameObject.Instantiate(handlerTemp, windowHandlerParent, false);
                
                windowToken.Handler.gameObject.SetActive(true);
                windowToken.Handler.AddClickEvent(OnDesktopHandlerIconClick);
                windowToken.Handler.token = windowToken;
                windowToken.Handler.name = sourceWindow.GetType().Name;
                
                windowToken.WindowGo.name = sourceWindow.GetType().Name;
                windowToken.Window.token = windowToken;
                windowToken.Window.AddHookClick(ActiveFocus);
               
                // windowToken.Window.gameObject.SetActive(true);
               
                windows.Add(sourceWindow,windowToken);   
            }
            
            return windowToken;
        }


        public void CloseWindows(WindowDefine sourceWindow)
        {
            if (windows.TryGetValue(sourceWindow, out DestktopWindowToken windowToken))
            {
                CloseWindows(windowToken);
            }
        }

        public void CloseAllWindows()
        {
            var allWindows = windows.Select((a) => a.Key).ToArray();
            for (var i = 0; i < allWindows.Length; i++)
            {
                CloseWindows(allWindows[i]);
            }
            
        }
        
        public void CloseWindows(DestktopWindowToken token)
        {
            if (windows.TryGetValue(token.WindowName, out DestktopWindowToken windowToken))
            {
                DiscardFocus(windowToken);
                topWindows.Remove(windowToken);
                windowToken.Window.OnClose();
                windowToken.Handler.OnClose();
                GameObject.Destroy(windowToken.WindowGo);
                GameObject.Destroy(windowToken.Handler.gameObject);
                windows.Remove(token.WindowName);
            }
            
        }

        private PointerEventData sampleEventData = new (EventSystem.current);
        private List<RaycastResult> raycastResults = new();
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (sampleEventData == null)
                    sampleEventData = new PointerEventData(EventSystem.current);
                sampleEventData.position = Input.mousePosition;
                EventSystem.current.RaycastAll(sampleEventData,raycastResults);
                if (raycastResults.Count > 0)
                { 
                    WindowLogic windowLogic =  raycastResults[0].gameObject.GetComponentInParent<WindowLogic>();
                    if (windowLogic != null)
                    {
                        ComputerWindow computerWindow = windowLogic.ComputerWindow;
                        if (computerWindow != null)
                        {
                            computerWindow.ActiveFocus();
                        }
                    }
                }
            }

            int day = Main.Get().GameProcessController.curDay;
            dayText.text = $"第{day}天";
        }
        
        
        
    }
    
    public class DestktopWindowToken
    {
        public ComputerWindow Window;
        public ComputerDesktopWindowHandler Handler;
        public GameObject WindowGo;
        public WindowDefine WindowName;
    }
}