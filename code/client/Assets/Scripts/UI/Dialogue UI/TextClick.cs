using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TextClick : MonoBehaviour , IPointerClickHandler
{
    public Dictionary<string, string> LinkID_Path = new Dictionary<string, string>();
    public Dictionary<string, bool> LinkID_Created = new Dictionary<string, bool>();
    private Dictionary<string, Action<string>> Link_Callback = new();
    private TextMeshProUGUI textMeshProUGUI;
    protected Canvas canvas;
    private Camera computerCamera;
    private void Awake()
    {
        textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
        canvas = gameObject.GetComponentInParent<Canvas>();
        // Get a reference to the camera if Canvas Render Mode is not ScreenSpace Overlay.
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            computerCamera = null;
        else
            computerCamera = canvas.worldCamera;
    }

    protected void RegisterLinkEvent(string linkText,Action<string> callBack)
    {
        Link_Callback[linkText] = callBack;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("click" + gameObject);
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshProUGUI, UIRenderGraphicRaycasterProxy.ConvertPosition(Input.mousePosition), computerCamera);
        // int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshProUGUI, Input.mousePosition, computerCamera);
        if(linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textMeshProUGUI.textInfo.linkInfo[linkIndex];

            string linkID = linkInfo.GetLinkID();

            if (!LinkID_Created[linkID])//∑¿÷π÷ÿ∏¥¥¥Ω®
            {
                LinkID_Created[linkID] = true;
                CreateWindow(LinkID_Path[linkID]);
            }

            if (Link_Callback.TryGetValue(linkID, out Action<string> click))
                click.Invoke(linkID);
        }
    }

    public abstract void CreateWindow(string path);

}
