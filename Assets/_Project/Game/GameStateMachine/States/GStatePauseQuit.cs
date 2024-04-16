using UnityEngine;

public class GStatePauseQuit : GStateBase
{
    public GStatePauseQuit(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        Time.timeScale = 1f;
        ScreenTransition.Instance.Close();
        _context._pauseQuit = false;
    }

    public override void OnExit()
    {
        base.OnExit();

        if (_context == null) return;

        _context._pauseQuit = false;
    }
}