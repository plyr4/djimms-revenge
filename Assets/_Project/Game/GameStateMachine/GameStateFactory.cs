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
    
    public GStateBase PressStartIn()
    {
        return new GStatePressStartIn(_context, this);
    }
        
    public GStateBase PressStart()
    {
        return new GStatePressStart(_context, this);
    }
    
    public GStateBase Play()
    {
        return new GStatePlay(_context, this);
    }
}