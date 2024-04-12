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