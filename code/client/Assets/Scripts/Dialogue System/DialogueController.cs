using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [Header("Basic Elements")]
    public TextMeshProUGUI DialogueText;
    public GameObject SendButton;
    //private TextMeshProUGUI SendButtonText;
    public GameObject[] SelectButtons = new GameObject[2];
    private TextMeshProUGUI[] SelectButtonTexts = new TextMeshProUGUI[2];
    public Sprite SelectedButton;
    public Sprite UnselectedButton;
    [Header("Data")]
    private int day = 0;
    private DialogueData_SO currentData;
    private int currentIndex;
    [Header("State")]
    /// <summary>
    /// The mode about select.
    /// Case:
    /// -1: Dialogue Completed
    /// 0: Can't Select
    /// 1: Can select and There is an option
    /// 2: Can select and there is two options 
    /// </summary>
    private int selectMode;
    /// <summary>
    /// The state about select.
    /// Case:
    /// -1: No buttons are selected.
    /// 0: Button "SelectButtons[0]" is selected.
    /// 1: Button "SelectButtons[1]" is selected.
    /// </summary>
    private int selectState;
    [Header("Setting")]
    public float typingSpeed;
    public float minInterval;
    public float intervalFactor;
    private float intervalTime;
    private float passedTime;
    private void Awake()
    {
        //SendButtonText = SendButton.GetComponentInChildren<TextMeshProUGUI>();
        SelectButtonTexts[0] = SelectButtons[0].GetComponentInChildren<TextMeshProUGUI>();
        SelectButtonTexts[1] = SelectButtons[1].GetComponentInChildren<TextMeshProUGUI>();

        //SendButtonText.text = "";
        SelectButtonTexts[0].text = "";
        SelectButtonTexts[1].text = "";

        SelectButtons[0].GetComponent<Image>().sprite = SelectedButton;
        SelectButtons[1].GetComponent<Image>().sprite = SelectedButton;
        SendButton.GetComponent<Image>().sprite = SelectedButton;

        LoadToday();
    }
    private void Update()
    {
        if (selectMode == 0)
        {
            UpdateDialogue();
        }
    }
    public void UpdateDialogue()
    {
        if(currentIndex >= currentData.dialoguePieces.Count)
        {
            //TODO:标记已完成
          
            if (selectMode != -1)
            {
                Game.Main.Get()?.GameProcessController.FinishActivities(Game.DayActivities.Dialog);
                Game.Main.Get()?.GameProcessController.TryGameOver();
            }
            
            selectMode = -1;
            return;
        }
        var piece = currentData.dialoguePieces[currentIndex];
        intervalTime = Mathf.Max(intervalFactor * piece.text.Length, minInterval);
        if (passedTime > intervalTime)
        {
            passedTime = 0;

            if (piece.character == DialoguePiece.Character.Elazil)
            {
                DialogueText.text += "<color=red>Elazil\n";
                DialogueText.text += "<color=white>" + piece.text + "\n";
                DialogueText.text += "\n";
                NextPiece();
            }
            else
            {
                ShowChoose();
            }
        }
        passedTime += Time.deltaTime;
    }

    public void NextPiece()
    {
        switch (currentData.dialoguePieces[currentIndex].options.Count)
        {
            case 0: currentIndex++; break;//下一条
            case 1: currentIndex = currentData.dialoguePieces[currentIndex].options[0]; break;//跳转
            case 2: ShowChooses(); break;//选择
            default: Debug.Log("Options more than 2"); break;
        }
    }
    public void NextPiece(int idx)//0 or 1
    {
        currentIndex = currentData.dialoguePieces[currentIndex].options[idx];
    }

    public void ShowChoose()
    {
        selectMode = 1;
        selectState = -1;
        //SendButtonText.text = "发送";
        SelectButtonTexts[1].text = currentData.dialoguePieces[currentIndex].text;
        SelectButtons[1].GetComponent<Image>().sprite = UnselectedButton;
    }
    public void ShowChooses()
    {
        selectMode = 2;
        selectState = -1;
        //SendButtonText.text = "发送";
        SelectButtonTexts[0].text = currentData.dialoguePieces[currentData.dialoguePieces[currentIndex].options[0]].text;
        SelectButtons[0].GetComponent<Image>().sprite = UnselectedButton;
        SelectButtonTexts[1].text = currentData.dialoguePieces[currentData.dialoguePieces[currentIndex].options[1]].text;
        SelectButtons[1].GetComponent<Image>().sprite = UnselectedButton;
    }

    public void ClickButton(int index)
    {
        if (selectMode==0) return;
        else if(selectMode == 1)
        {
            if (index == 1)
            {
                if (selectState == index)
                {
                    selectState = -1;
                    SelectButtons[index].GetComponent<Image>().sprite = UnselectedButton;
                    SendButton.GetComponent<Image>().sprite = SelectedButton;
                }
                else
                {
                    selectState = index;
                    SelectButtons[index].GetComponent<Image>().sprite = SelectedButton;
                    SendButton.GetComponent<Image>().sprite = UnselectedButton;
                }
            }
        }
        else
        {
            if (selectState == index)
            {
                selectState = -1;
                SelectButtons[index].GetComponent<Image>().sprite = UnselectedButton;
                SendButton.GetComponent<Image>().sprite = SelectedButton;
            }
            else
            {
                selectState = index;
                SelectButtons[index].GetComponent<Image>().sprite = SelectedButton;
                SelectButtons[1 - index].GetComponent<Image>().sprite = UnselectedButton;
                SendButton.GetComponent<Image>().sprite = UnselectedButton;
            }
        }
    }
    public void ClickSend() 
    {
        if (selectMode == 0 || selectState == -1) return;

        if(currentData.dialoguePieces[currentIndex].options.Count == 2)
        {
            if (selectMode == 2)
                NextPiece(selectState);
        }

        DialogueText.text += "<color=yellow>Ryota\n";
        DialogueText.text += "<color=white>";

        selectMode = -1;
        selectState = -1;

        StartCoroutine(SetText());

        //SendButtonText.text = "";
        SelectButtonTexts[0].text = "";
        SelectButtonTexts[1].text = "";
        SelectButtons[0].GetComponent<Image>().sprite = SelectedButton;
        SelectButtons[1].GetComponent<Image>().sprite = SelectedButton;
        SendButton.GetComponent<Image>().sprite = SelectedButton;
    }

    IEnumerator SetText()
    {
        foreach(var i in currentData.dialoguePieces[currentIndex].text)
        {
            DialogueText.text += i;
            yield return new WaitForSeconds(typingSpeed);
        }
        DialogueText.text += "\n\n";

        selectMode = 0;
        passedTime = 0;
        NextPiece();
    }
    
    public void LoadToday()
    {
        ++day;
        if(Game.Main.Get() != null)
            day = Game.Main.Get().GameProcessController.curDay;
        
        currentData = Resources.Load<DialogueData_SO>("Dialogue Data/Day " + day);
        DialogueText.text = "";
        currentIndex = 0;
        selectMode = 0;
        selectState = -1;
        intervalTime = 0;
        passedTime = 0;
    }
}
