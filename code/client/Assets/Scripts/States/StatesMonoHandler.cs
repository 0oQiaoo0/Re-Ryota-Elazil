using UnityEngine;

namespace Game
{
    public class StatesMonoHandler : MonoBehaviour
    {
        public void ChangeToSceneState()
        {
            Main.Get().StateManager.ChangeToState( StateType.Scene );
        }

        public void ChangeToComputerState()
        {
            Main.Get().StateManager.ChangeToState( StateType.Computer );
        }
    }
}