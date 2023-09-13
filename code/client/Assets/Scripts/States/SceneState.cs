namespace Game
{
    public class SceneState : StateBase
    {
        public override StateType type { get; } = StateType.Scene;

        public override void OnEnter()
        {
            base.OnEnter();
            // 激活鼠标点击
            // 激活相机移动
            // 激活 Game Gamera
            Main.MouseOpter.EnableMouseOpterByNextFrame();
            Main.MouseOpter.EnableCameraMove();
            Main.CameraOpter.EnableGameCamera();
        }

        public override void OnExit()
        {
            base.OnExit();
            Main.MouseOpter.DisableCameraMove();
            Main.MouseOpter.DisableMouseOpter();
        }
    }
}