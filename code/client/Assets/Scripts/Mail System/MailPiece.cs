using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MailPiece
{
    public string title;

    public string sendTime;
    public enum MailType
    {
        Forward,
        Direct
    }
    public MailType mailType;

    public Sprite headPortrait;

    public string senderName;
    public string senderEmail;

    //title
    public string targetEmail;
    [TextArea(10,20)]
    public string content;
}
