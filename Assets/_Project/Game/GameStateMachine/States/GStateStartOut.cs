public class GStateStartOut : GStateBase
{
    public GStateStartOut(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        _context._startOnPlay = false;

        ScreenTransition.Instance.Close();
    }

    public override void OnExit()
    {
        base.OnExit();

        if (_context == null) return;

        _context._startOnPlay = false;
        _context._startOutDone = false;
    }
}