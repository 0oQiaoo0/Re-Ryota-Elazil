using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("成员")]
    public List<Document> Documents;
    public List<Folder> Folders;
    public TextMeshProUGUI timeUI;
    public GameObject NextGameGO;

    [Header("时间")]
    public float SetTime;
    private float RemainTime;

    [Header("状态")]
    public bool isStarted;
    public bool isDragging;
    private int RemainDocumentNum;

    private void Awake()
    {
        timeUI.text = SetTime.ToString("0.00");

        RemainTime = SetTime;
        
        isStarted = false;
        isDragging = false;
        RemainDocumentNum = Documents.Count;
    }
    public void GameReset()
    {
        timeUI.text = SetTime.ToString("0.00");

        RemainTime = SetTime;
        
        isStarted = false;
        isDragging = false;
        RemainDocumentNum = Documents.Count;

        foreach (var document in Documents)
        {
            document.DocumentReset();
        }
        foreach (var folder in Folders)
        {
            folder.FolderReset();
        }
    }
    private void Update()
    {
        if (isStarted)
        {
            RemainTime -= Time.deltaTime;
            if (RemainTime <= 0)
            {
                timeUI.text = "0.00";
                GameReset();
            }
            else timeUI.text = RemainTime.ToString("0.00");
        }
    }
 
    

    public void StartGhost()
    {
        isStarted = true;
        foreach (var document in Documents)
        {
            if(document.isGhost) document.gameObject.SetActive(true);
        }
    }

    public void ResetGhost()
    {
        foreach(var document in Documents)
        {
            if (document.isGhost)
            {
                document.GhostReset();
            }
        }
        --RemainDocumentNum;
        if (RemainDocumentNum <= 0)
        {
            NextGame();
        }
    }

    public void NextGame()
    {
        isStarted = false;
        if (NextGameGO)
        {
            NextGameGO.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            //TODO:记录已完成游戏
            // transform.parent.parent.gameObject.SetActive(false);
            Game.Main.Get()?.GameProcessController.FinishActivities(Game.DayActivities.Game);
            UI.ComputerDesktopWindow.Instance.CloseWindows( WindowDefine.GameWindow);
        }
    }
}
