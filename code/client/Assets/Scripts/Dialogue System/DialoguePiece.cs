using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[System.Serializable]
public class DialoguePiece
{
    public DialoguePiece(Character character,string text)
    {
        this.character = character;
        this.text = text;
    }
    public enum Character
    {
        Elazil,
        Ryota
    }
    public Character character;
    [TextArea]
    public string text;

    public List<int> options = new List<int>();
}
