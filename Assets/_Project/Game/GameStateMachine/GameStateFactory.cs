public class GStateFactory : StateFactory
{
    public GStateFactory(StateMachineMono context) : base(context)
    {
    }

    public GStateBase Null()
    {
        return new GStateNull(_context, this);
    }

    public GStateBase Init()
    {
        return new GStateInit(_context, this);
    }

    public GStateBase StartIn()
    {
        return new GStateStartIn(_context, this);
    }

    public GStateBase Start()
    {
        return new GStateStart(_context, this);
    }

    public GStateBase StartOut()
    {
        return new GStateStartOut(_context, this);
    }
    
    public GStateBase LoadIntro()
    {
        return new GStateLoadIntro(_context, this);
    }
    public GStateBase Intro()
    {
        return new GStateIntro(_context, this);
    }

    public GStateBase LoadPlay()
    {
        return new GStateLoadPlay(_context, this);
    }

    public GStateBase Play()
    {
        return new GStatePlay(_context, this);
    }

    public GStateBase Pause()
    {
        return new GStatePause(_context, this);
    }

    public GStateBase PauseQuit()
    {
        return new GStatePauseQuit(_context, this);
    }

    public GStateBase GameOver()
    {
        return new GStateGameOver(_context, this);
    }

    public GStateBase RetryIn()
    {
        return new GStateRetryIn(_context, this);
    }

    public GStateBase Retry()
    {
        return new GStateRetry(_context, this);
    }
}