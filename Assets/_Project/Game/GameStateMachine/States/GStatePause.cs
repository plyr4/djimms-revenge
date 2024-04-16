using UnityEngine;

public class GStatePause : GStateBase
{
    public GStatePause(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        Time.timeScale = 0f;
        
        _context._playPause = false;
        _context._pauseContinue = false;
    }

    public override void OnExit()
    {
        base.OnExit();

        if (_context == null) return;

        Time.timeScale = 1f;
        
        _context._playPause = false;
    }
}