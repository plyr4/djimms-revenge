public class GStatePressStartIn : GStateBase
{
    public GStatePressStartIn(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        _context._pressStartInDone = true;
    }

    public override void OnExit()
    {
        base.OnExit();

        if (_context == null) return;

        _context._pressStartInDone = false;
    }
}