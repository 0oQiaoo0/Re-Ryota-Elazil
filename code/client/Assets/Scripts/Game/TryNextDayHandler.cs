using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TryNextDayHandler : GameSceneInteraction
    {
        public int nextDayTalkId;

        protected override void OnEnd()
        {
            base.OnEnd();

            if (Game.Main.Get().GameProcessController.TodayFinishAll())
            {
                Game.Main.Get().GameProcessController.TryToNextDay();
            }
        }

        protected override int GetTalkId()
        {
            if (Game.Main.Get().GameProcessController.TodayFinishAll())
            {
                return nextDayTalkId;
            }
            else
            {
                return base.GetTalkId();
            }
        }

    }
}