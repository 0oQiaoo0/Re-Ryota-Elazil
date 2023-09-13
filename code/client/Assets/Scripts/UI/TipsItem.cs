using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TipsItem : MonoBehaviour
    {
        public Vector3 scaler_1 = new Vector3(0,1,1);
        public Vector3 scaler_2 = new Vector3(1, 1, 1);

        public float scalerTimer =1f;

        // public bool EnableOpen = true;

        public GameObject lockGameObject;

        public RectTransform rectTransform;
        public RectTransform parentRectTransform;

        public TextMeshProUGUI tipText;
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            // if(EnableOpen)
                // Open();
        }

        public void Open(string tipDesc)
        {
            tipText.text = tipDesc;
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(StartScaler(scaler_1,scaler_2,scalerTimer,null));
        }

        public void Close()
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(StartScaler(scaler_2,scaler_1,scalerTimer, () => { gameObject.SetActive(false);  }));
            lockGameObject = null;
        }
        
        

        private IEnumerator StartScaler(Vector3 s,Vector3 e,float totalTime,Action callBack)
        {
            float time = 0;
            while (time <= totalTime)
            {
                transform.localScale = Vector3.Lerp(s, e,time / totalTime);
                time += Time.deltaTime;
                yield return null;
            }
            callBack?.Invoke();
        }

        private void Update()
        {
            if (lockGameObject == null)
                return;

            Vector3 pos = lockGameObject.transform.position;

            Main main = Main.Get();
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(main.GameUICamera,pos);
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPos, main.GameUICamera,
                out Vector2 localPos))
                this.transform.localPosition = localPos;
        }

        public void LockGameObject(GameObject go)
        {
            lockGameObject = go;
        }
        
    }


    public enum TipDescDefine
    {
        Computer = 0,
        BedroomDoor = 1,
        Calendar = 2,
        Balcony = 3,
        Poster = 4
    }
    
}