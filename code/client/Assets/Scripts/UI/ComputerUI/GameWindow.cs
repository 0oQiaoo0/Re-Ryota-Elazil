using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace UI
{
    public class GameWindow : WindowLogic
    {
        public List<GameObject> dayGames = new();

        public void OnEnable()
        {

            int day = Main.Get().GameProcessController.curDay;
            for (int i = 0; i < dayGames.Count; i++)
            {
                dayGames[i].SetActive(day == i + 1);
                if (day == i + 1)
                {
                    ComputerWindow.BindRectSize(dayGames[i].GetComponent<RectTransform>());
                }
            }
            
            
        }
    }
}