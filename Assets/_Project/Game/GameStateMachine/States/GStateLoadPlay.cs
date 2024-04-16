public class GStateLoadPlay : GStateBase
{
    public GStateLoadPlay(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        _context._loadPlayDone = false;
        
        ScreenTransition.Instance.DelayedOpen(1f);
    }

    public override void OnExit()
    {
        base.OnExit();

        if (_context == null) return;

        _context._loadPlayDone = false;
    }
}