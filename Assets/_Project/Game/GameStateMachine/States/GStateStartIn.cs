public class GStateStartIn : GStateBase
{
    public GStateStartIn(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        _context._startInDone = true;
    }

    public override void OnExit()
    {
        base.OnExit();

        if (_context == null) return;
        _context._startInDone = false;
    }
}