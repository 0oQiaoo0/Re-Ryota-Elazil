using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailClick : MonoBehaviour
{
    MailPiece mailPiece;
    GameObject MaillistInterface;
    GameObject MailDetailsInterface;
    Transform MailDetails;

    private void Start()
    {
        mailPiece = GetComponent<Mail>().mailPiece;
        Transform mailWindowTransform = gameObject.GetComponentInParent<UI.MailWindowRoot>().transform;
        MaillistInterface = mailWindowTransform.Find("Maillist Interface").gameObject;
        MailDetailsInterface = mailWindowTransform.Find("Mail Details Interface").gameObject;
        MailDetails = MailDetailsInterface.transform.Find("Maillist").Find("Viewport").Find("Details");
    }

    public void ClickMail()
    {
        MailDetailsInterface.SetActive(true);

        //Status
        Image statusImage = transform.Find("Status").GetComponent<Image>();
        Color tmpColor = statusImage.color;
        tmpColor.a = 0f;
        statusImage.color = tmpColor;

        //HeadPortrait
        Transform headPortrait = MailDetails.Find("Sender").Find("HeadPortrait");
        if (mailPiece.mailType == MailPiece.MailType.Forward)
            headPortrait.GetComponent<LayoutElement>().preferredWidth = 0;
        else
        {
            headPortrait.GetComponent<LayoutElement>().preferredWidth = 80;
            headPortrait.GetComponent<Image>().sprite = mailPiece.headPortrait;
        }

        //SenderText
        TextMeshProUGUI senderText = MailDetails.Find("Sender").Find("SenderText").GetComponent<TextMeshProUGUI>();
        if (mailPiece.mailType == MailPiece.MailType.Forward)
            senderText.text = "转发\n";
        else
            senderText.text = mailPiece.senderName + "\n";

        senderText.text += "<color=white>" + mailPiece.senderEmail;

        //Title & Email
        TextMeshProUGUI titleAndEmail = MailDetails.Find("Title&Email").GetComponent<TextMeshProUGUI>();
        titleAndEmail.text = mailPiece.title;
        if (mailPiece.mailType == MailPiece.MailType.Direct)
            titleAndEmail.text += "\n<size=30>发给:" + mailPiece.targetEmail;

        //Content
        TextMeshProUGUI content = MailDetails.Find("Content").GetComponent<TextMeshProUGUI>();
        content.text = mailPiece.content;

        MaillistInterface.SetActive(false);
    }
}
