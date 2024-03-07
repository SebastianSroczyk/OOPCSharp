namespace StudentGame.Code.Screens
{
    // Just another Screen to test transitions with
    internal class OtherWorld : Screen
    {
        public override void Start(Core core)
        {
            base.Start(core);
            Transition.Instance.EndTransition();
        }
    }
}
