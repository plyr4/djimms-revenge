public abstract class StateBase : IState
{
    public StateMachineMono _context;
    public StateFactory _factory;

    protected StateBase()
    {
    }

    protected StateBase(StateMachineMono context, StateFactory factory)
    {
        _context = context;
        _factory = factory;
    }

    public virtual void OnEnter()
    {
        if (_context._onStateChangeEvent != null)
        {
            GenericEventOpts opts = new GenericEventOpts
            {
                _newState = this
            };
            _context._onStateChangeEvent.Invoke(opts);
        }
    }

    public virtual void Update()
    {
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void OnExit()
    {
    }
}