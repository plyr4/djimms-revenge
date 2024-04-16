public class GStateLoadIntro : GStateBase
{
    public GStateLoadIntro(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        _context._loadIntroDone = false;
        
#if UNITY_EDITOR
        if (_context._skipIntroInEditMode) _context._loadIntroDone = true;
#endif
    }

    public override void OnExit()
    {
        base.OnExit();

        if (_context == null) return;

        ScreenTransition.Instance.DelayedOpen(1f);
        
        _context._loadIntroDone = false;
    }
}