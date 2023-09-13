using System.Collections.Generic;

namespace Game
{
    public class GameSceneInteractionByEveryDay : GameSceneInteraction
    {
        public List<int> talkIds = new();

        protected override int GetTalkId()
        {
            int talkIndex = Game.Main.Get().GameProcessController.curDay - 1;

            if (talkIndex >= 0 && talkIndex < talkIds.Count)
                return talkIds[talkIndex];
            
            return base.GetTalkId();
        }
    }
}