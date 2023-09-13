using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class MailController : MonoBehaviour
{
    private int day;
    public Transform MailEntries;
    private void Awake()
    {
        day = 0;
        // LoadNextDay();
        // LoadNextDay();
        // LoadNextDay();
        // LoadNextDay();
    }

    private void OnEnable()
    {
        LoadToday();
    }

    public void LoadToday()
    {
        ++day;
        if( Game.Main.Get() != null)
            day = Game.Main.Get().GameProcessController.curDay;
        
        MailData_SO mailData = Resources.Load<MailData_SO>("Mail Data/Day " + day);

        foreach (var mailPiece in mailData.mailPieces)
        {
            GameObject gameObject = (GameObject)Instantiate(Resources.Load("Prefabs/MailUI/Mail Entry"), MailEntries);

            gameObject.GetComponent<Mail>().mailPiece = mailPiece;

            //From
            if (mailPiece.mailType == MailPiece.MailType.Forward)
                gameObject.transform.Find("From").GetComponent<TextMeshProUGUI>().text = "";
            else
                gameObject.transform.Find("From").GetComponent<TextMeshProUGUI>().text = mailPiece.senderName;
            //Subject
            gameObject.transform.Find("Subject").GetComponent<TextMeshProUGUI>().text = mailPiece.title;
            //Time
            gameObject.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = mailPiece.sendTime;
        }
    }
}
