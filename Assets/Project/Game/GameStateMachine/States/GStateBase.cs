using System;

public abstract class GStateBase : StateBase
{
    protected new GStateMachineMono _context
    {
        get
        {
            try
            {
                return (GStateMachineMono)base._context;
            }
            catch (InvalidCastException e)
            {
                DebugLogger.LogError($"using the wrong context type: InvalidCastException: {e.Message}");
                return null;
            }
        }
        set => base._context = value;
    }

    protected GStateBase(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }
}