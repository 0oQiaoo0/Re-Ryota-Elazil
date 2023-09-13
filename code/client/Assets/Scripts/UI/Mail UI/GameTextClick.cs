using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class GameTextClick : TextClick
{
    private void Start()
    {
        LinkID_Path.Add("game_01", "Prefabs/GameUI/Game Window Day 1");
        LinkID_Path.Add("game_02", "Prefabs/GameUI/Game Window Day 2");
        LinkID_Path.Add("game_03", "Prefabs/GameUI/Game Window Day 3");
        LinkID_Path.Add("game_04", "Prefabs/GameUI/Game Window Day 4");
        LinkID_Created.Add("game_01", false);
        LinkID_Created.Add("game_02", false);
        LinkID_Created.Add("game_03", false);
        LinkID_Created.Add("game_04", false);
        RegisterLinkEvent("game_01", (string desc) => OnOpenGameClick(1));
        RegisterLinkEvent("game_02", (string desc) => OnOpenGameClick(2));
        RegisterLinkEvent("game_03", (string desc) => OnOpenGameClick(3));
        RegisterLinkEvent("game_04", (string desc) => OnOpenGameClick(4));
    }

    private void OnOpenGameClick(int dayIndex)
    {
        ComputerDesktopWindow.Instance?.OpenWindows(WindowDefine.GameWindow);
    }
    
    public override void CreateWindow(string path)
    {
        if (ComputerDesktopWindow.Instance == null)
        {
            GameObject instance = (GameObject)Instantiate(Resources.Load(path), canvas.transform.Find("Background"));
        }
    }
}
