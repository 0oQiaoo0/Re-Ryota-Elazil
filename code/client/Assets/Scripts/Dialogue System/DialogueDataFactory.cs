using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Linq;

public class DialogueDataFactory
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Dialogue Data/Create Days")]

    public static void CreateDays()
    {
        for (int i = 1; i <= 4; i++)
        {
            CreateDay(i);
        }
    }
    public static void CreateDay(int idx)
    {
        DialogueData_SO asset = ScriptableObject.CreateInstance<DialogueData_SO>();
        string[] textTxt = File.ReadAllLines("Assets/Resources/Dialogue Data/Day " + idx + ".txt");
        Dictionary<int, int> TransDic = new Dictionary<int, int>();

        int nowInd = -1;
        foreach (string text in textTxt)
        {
            if (text.Length == 0) continue;
            if (text[0] == 'E')
            {
                ++nowInd;
                asset.dialoguePieces.Add(new DialoguePiece(DialoguePiece.Character.Elazil, text.Substring(2)));
                if (TransDic.ContainsKey(nowInd))
                {
                    asset.dialoguePieces[nowInd].options.Add(TransDic[nowInd]);
                }

            }
            else if (text[0] == 'R')
            {
                ++nowInd;
                asset.dialoguePieces.Add(new DialoguePiece(DialoguePiece.Character.Ryota, text.Substring(2)));
                if (TransDic.ContainsKey(nowInd))
                {
                    asset.dialoguePieces[nowInd].options.Add(TransDic[nowInd]);
                }
            }
            else if (text[0] == 'C')
            {
                string tmp = "";
                int i = 2;
                while (i < text.Length && text[i] != ' ')
                {
                    tmp += text[i++];
                }
                int chooseOneLength = int.Parse(tmp);
                tmp = "";
                ++i;
                while (i < text.Length && text[i] != ' ')
                {
                    tmp += text[i++];
                }
                int chooseTwoLenth = int.Parse(tmp);

                asset.dialoguePieces[nowInd].options.Add(nowInd + 1);
                asset.dialoguePieces[nowInd].options.Add(nowInd + 2);
                TransDic.Add(nowInd + 1, nowInd + 3);
                TransDic.Add(nowInd + 2, nowInd + 2 + chooseOneLength + 1);
                if (chooseOneLength > 0)
                    TransDic.Add(nowInd + 2 + chooseOneLength, nowInd + 2 + chooseOneLength + chooseTwoLenth + 1);
            }
        }
        UnityEditor.AssetDatabase.DeleteAsset("Assets/Resources/Dialogue Data/Day " + idx + ".asset");
        UnityEditor.AssetDatabase.CreateAsset(asset, "Assets/Resources/Dialogue Data/Day " + idx + ".asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
    
#endif
}
