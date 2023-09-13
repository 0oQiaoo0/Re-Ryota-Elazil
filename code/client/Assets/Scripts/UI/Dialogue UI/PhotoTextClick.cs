using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PhotoTextClick : TextClick
{
    private void Start()
    {
        LinkID_Path.Add("id_01", "补充素材/聊天发送的图片/第一日/羽毛");
        LinkID_Path.Add("id_02", "补充素材/聊天发送的图片/第二日/女主和队友的合照");
        LinkID_Path.Add("id_03", "补充素材/聊天发送的图片/第二日/天空");
        LinkID_Path.Add("id_04", "补充素材/聊天发送的图片/第三日/虫心汤");

        LinkID_Created.Add("id_01", false);
        LinkID_Created.Add("id_02", false);
        LinkID_Created.Add("id_03", false);
        LinkID_Created.Add("id_04", false);
        
        RegisterLinkEvent("id_01", (string desc) => OnOpenPhotolick ("补充素材/聊天发送的图片/第一日/羽毛"));
        RegisterLinkEvent("id_02", (string desc) => OnOpenPhotolick ("补充素材/聊天发送的图片/第二日/女主和队友的合照"));
        RegisterLinkEvent("id_03", (string desc) => OnOpenPhotolick ("补充素材/聊天发送的图片/第二日/天空"));
        RegisterLinkEvent("id_04", (string desc) => OnOpenPhotolick ("补充素材/聊天发送的图片/第三日/虫心汤"));
    }
    
    private void OnOpenPhotolick(string descPath)
    {
        ComputerDesktopWindow.Instance?.OpenWindows(WindowDefine.PhotoWindow,descPath);
    }
    
    public override void CreateWindow(string path)
    {
        GameObject instance = (GameObject)Instantiate(Resources.Load("Prefabs/DialogueUI/Photo Window"), canvas.transform.Find("Background"));

        instance.transform.Find("浅灰色底层").Find("Photo").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
    }
}
    
