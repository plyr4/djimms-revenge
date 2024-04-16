using UnityEngine;

public abstract class StateMachineMono : MonoBehaviour
{
    private static StateMachineMono _instance;
    public static StateMachineMono Instance
    {
        get
        {
            // attempt to locate the singleton
            if (_instance == null)
            {
                _instance = (StateMachineMono)FindObjectOfType(typeof(StateMachineMono));
            }

            // create a new singleton
            if (_instance == null)
            {
                _instance = (new GameObject("GameManager")).AddComponent<StateMachineMono>();
            }

            // return singleton
            return _instance;
        }
    }

    protected StateMachine _stateMachine;
    protected StateFactory _stateFactory;
    [SerializeField]
    [ReadOnlyInspector]
    protected string _currentStateName;
    [SerializeField]
    private bool _showDebug;
    [SerializeField]
    [ReadOnlyInspector]
    protected string[] _states;
    [SerializeField]
    [ReadOnlyInspector]
    protected string[] _transitions;
    [SerializeField]
    public GenericEvent _onStateChangeEvent;
    public virtual void Start()
    {
    }

    public virtual void Update()
    {
        if (_stateMachine == null) return;
        _stateMachine.Update();
        _currentStateName = _stateMachine._current?._state?.GetType().Name;
        if (_showDebug)
        {
            _states = _stateMachine.GetStates();
            _transitions = _stateMachine.GetTransitions();
        }
    }

    public virtual void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    protected virtual void Initialize(StateMachine stateMachine, StateFactory factory)
    {
        _stateMachine = stateMachine;
        _stateFactory = factory;
    }
    
    public static IState GetCurrentState() => Instance._stateMachine._current._state;

    // a state transition with a from, to, and condition
    protected void at(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);

    // a state transition with a to and condition, but no from
    protected void any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
}