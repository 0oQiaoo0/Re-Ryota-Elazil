using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class ComputerWindow : UIWindow
    {
        public Button minButton;
        public Button maxButton;
        public Button closeButton;
        public ImageDrop moveImge;
        public ImageDrop scalerImge;
        // public ImageClickHook hookImage;
        public RectTransform winRectTransform;
        private RectTransform winParentRectTransform;
        public DestktopWindowToken token;

        public RectTransform bindRectSize;

        public Vector2 sizeDelta;

        public Vector2 TestOffset = Vector2.zero;
        
        public Vector2 minSize = new Vector2(500,300);
        public Vector2 maxSize = new Vector2(1920,1080);

        public enum WindowSizeType
        {
            Default,
            Max,
            Min
        }

        public WindowSizeType WindowSize{ get; private set; } = WindowSizeType.Default;
        
        public WindowSizeType LastWindowSize{ get; private set; } = WindowSizeType.Default;

        private Vector3 windWorldPosition;

        public float minOrMaxTimer = 0.3f;

        public bool isInited = false;
        
        protected override void Awake()
        {
            closeButton.onClick.AddListener(Close);
            maxButton.onClick.AddListener(OnMax);
            minButton.onClick.AddListener(OnMin);
            moveImge.onDropEvent = OnMove;
            scalerImge.onDropEvent = OnScalerSize;
            if (winRectTransform == null)
                winRectTransform = transform.parent.GetComponent<RectTransform>();

            winParentRectTransform = winRectTransform.parent.GetComponent<RectTransform>();
            windWorldPosition = winRectTransform.position;

            if (bindRectSize != null)
            {
                minSize = maxSize = bindRectSize.rect.size;
            }
            isInited = true;
        }

        public void BindRectSize(RectTransform rectTransform)
        {
            bindRectSize = rectTransform; 
            if (bindRectSize != null)
            {
                minSize = maxSize = bindRectSize.rect.size;
                if(isInited)
                    OnScalerSize(Vector2.zero);
            }
        }

        private void Start()
        {
            scalerImge.SetRectransformAndConvertType(winParentRectTransform,false);
            OnScalerSize(Vector2.zero);
        }

        public void OnMove(Vector3 offsetPos)
        {
            winRectTransform.position += offsetPos;
        }

        private void OnScalerSize(Vector3 localOffsetPos)
        {
            Vector2 offsetMax = winRectTransform.offsetMax;
            Vector2 offsetMin = winRectTransform.offsetMin;
            
            Vector2 size = winParentRectTransform.rect.size;
            float MinX = size.x -  offsetMin.x - minSize.x;
            float MaxX = size.x -  offsetMin.x - maxSize.x;
            float MinY = (size.y - -offsetMax.y - minSize.y);
            float MaxY = (size.y - -offsetMax.y - maxSize.y);
            
            offsetMax = offsetMax + new Vector2(localOffsetPos.x,0);
            offsetMin = offsetMin + new Vector2(0, localOffsetPos.y);

            offsetMax.x = Mathf.Clamp(offsetMax.x, -MinX, -MaxX);
            offsetMin.y = Mathf.Clamp(offsetMin.y,  MaxY,MinY); 
            
            winRectTransform.offsetMin = offsetMin;
            winRectTransform.offsetMax = offsetMax;
            TestOffset +=  new Vector2(localOffsetPos.x,localOffsetPos.y);
            sizeDelta = winRectTransform.sizeDelta;
        }
        
        public void Close()
        {
            ComputerDesktopWindow.Instance.CloseWindows(token);
        }

        public void ActiveFocus()
        {
            ComputerDesktopWindow.Instance.ActiveFocus(token);
        }
        
        private void OnMin()
        {
            SetMinWindow();
            ComputerDesktopWindow.Instance.DiscardFocus(token);
        }

        private void OnMax()
        {
            if(WindowSize == WindowSizeType.Max)
                SetDefaultWindow();
            else
                SetMaxWindow();
        }

        public void OnOpen()
        {
            if (WindowSize == WindowSizeType.Min)
            {
                SetLastDefineWindow();
            }
            
        }

        private void CheckLastWindow()
        {
            if (WindowSize == WindowSizeType.Min)
            {
                StartCoroutine(StartScaler(0,1,minOrMaxTimer));
                
                StartCoroutine(StartMove(minOrMaxTimer,false));
            }
        }
        
        public void SetMinWindow()
        {
            if (WindowSize == WindowSizeType.Min)
                return;
            
            windWorldPosition = winRectTransform.position;
            LastWindowSize = WindowSize;
            StartCoroutine(StartScaler(1,0,minOrMaxTimer));
            StartCoroutine(StartMove(minOrMaxTimer,true));
            WindowSize = WindowSizeType.Min;
        }

        public void SetDefaultWindow()
        {
            if (WindowSize == WindowSizeType.Default)
                return;
           
            LastWindowSize = WindowSize;
            winRectTransform.sizeDelta = sizeDelta;
            winRectTransform.transform.position = windWorldPosition;
            
            CheckLastWindow();
            WindowSize = WindowSizeType.Default;
        }
        
        public void SetMaxWindow()
        {
            if (WindowSize == WindowSizeType.Max)
                return;
           
            sizeDelta = winRectTransform.sizeDelta;
            windWorldPosition = winRectTransform.position;
            LastWindowSize = WindowSize;
            winRectTransform.localPosition = Vector3.zero;
            winRectTransform.offsetMax = Vector2.zero;
            winRectTransform.offsetMin = Vector2.zero;
            
            CheckLastWindow();
            WindowSize = WindowSizeType.Max;
        }

        
        
        public void SetLastDefineWindow()
        {
            switch (LastWindowSize)
            {
                case WindowSizeType.Default:
                {
                    SetDefaultWindow();
                    break;
                }
                case WindowSizeType.Min:
                {
                    SetMinWindow();
                    break;
                }
                case WindowSizeType.Max:
                {
                    SetMaxWindow();
                    break;
                }
            }
        }
        
        private IEnumerator StartScaler(float start,float end,float totalTimer)
        {
            float timer = 0;
            while (true)
            {
                timer += Time.deltaTime;    
                winRectTransform.localScale = Vector3.one * Mathf.Lerp(start,end,timer / totalTimer);
                if(timer >= totalTimer)
                    yield break;
                yield return null;
            }
        }
        
        private IEnumerator StartMove(float totalTimer,bool isRevert)
        {
            float timer = 0;
            while (true)
            {
                timer += Time.deltaTime;    
                if(isRevert)
                    winRectTransform.position = Vector3.Lerp(windWorldPosition, token.Handler.transform.position, timer / totalTimer);
                else
                    winRectTransform.position = Vector3.Lerp(token.Handler.transform.position, windWorldPosition, timer / totalTimer);
                if(timer >= totalTimer)
                    yield break;
                yield return null;
            }
        }
        
        public void OnClose()
        {
            // Handler.transform.position;
        }
        
        // 移动
        // 最小
        // 最大
        // 关闭

        public void AddHookClick(Action<DestktopWindowToken> activeFocus)
        {
            // hookImage.onClick = () => { activeFocus?.Invoke(this.token);};
        }
        
    }
    
    

}