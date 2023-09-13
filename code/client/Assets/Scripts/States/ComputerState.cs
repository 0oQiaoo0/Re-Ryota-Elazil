namespace Game
{
    public class ComputerState : StateBase
    {
        public override StateType type { get; } = StateType.Computer;

        public override void OnEnter()
        {
            base.OnEnter();
            Main.CameraOpter.EnableComputerCamera();
            Main.CloseBtn.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Main.CloseBtn.SetActive(false);
        }
        
        
    }

}