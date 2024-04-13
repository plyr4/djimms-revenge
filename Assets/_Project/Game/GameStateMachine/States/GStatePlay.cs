public class GStatePlay : GStateBase
{
    public GStatePlay(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        _context._startPlay = false;

        // todo: fix, track previous state when changing state
        _context._onGameStateChange.Invoke(this, this);
    }
}