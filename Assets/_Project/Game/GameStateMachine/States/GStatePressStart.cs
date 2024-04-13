public class GStatePressStart : GStateBase
{
    public GStatePressStart(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }
    
    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        // _context._startPlay = true;
    }
}