using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PhotoTextClick : TextClick
{
    private void Start()
    {
        LinkID_Path.Add("id_01", "�����ز�/���췢�͵�ͼƬ/��һ��/��ë");
        LinkID_Path.Add("id_02", "�����ز�/���췢�͵�ͼƬ/�ڶ���/Ů���Ͷ��ѵĺ���");
        LinkID_Path.Add("id_03", "�����ز�/���췢�͵�ͼƬ/�ڶ���/���");
        LinkID_Path.Add("id_04", "�����ز�/���췢�͵�ͼƬ/������/������");

        LinkID_Created.Add("id_01", false);
        LinkID_Created.Add("id_02", false);
        LinkID_Created.Add("id_03", false);
        LinkID_Created.Add("id_04", false);
        
        RegisterLinkEvent("id_01", (string desc) => OnOpenPhotolick ("�����ز�/���췢�͵�ͼƬ/��һ��/��ë"));
        RegisterLinkEvent("id_02", (string desc) => OnOpenPhotolick ("�����ز�/���췢�͵�ͼƬ/�ڶ���/Ů���Ͷ��ѵĺ���"));
        RegisterLinkEvent("id_03", (string desc) => OnOpenPhotolick ("�����ز�/���췢�͵�ͼƬ/�ڶ���/���"));
        RegisterLinkEvent("id_04", (string desc) => OnOpenPhotolick ("�����ز�/���췢�͵�ͼƬ/������/������"));
    }
    
    private void OnOpenPhotolick(string descPath)
    {
        ComputerDesktopWindow.Instance?.OpenWindows(WindowDefine.PhotoWindow,descPath);
    }
    
    public override void CreateWindow(string path)
    {
        GameObject instance = (GameObject)Instantiate(Resources.Load("Prefabs/DialogueUI/Photo Window"), canvas.transform.Find("Background"));

        instance.transform.Find("ǳ��ɫ�ײ�").Find("Photo").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
    }
}
    
