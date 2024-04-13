using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GStateMachineGame : GStateMachineMono
{
    public override void Start()
    {
        base.Start();
        Initialize(new GStateMachine(), new GStateFactory(this));
    }

    protected override void Initialize(StateMachine stateMachine, StateFactory factory)
    {
        // set up the state machine and state factory
        base.Initialize(stateMachine, factory);

        // states
        GStateBase nan = ((GStateFactory)_stateFactory).Null();
        GStateBase init = ((GStateFactory)_stateFactory).Init();
        GStateBase pressStartIn = ((GStateFactory)_stateFactory).PressStartIn();
        GStateBase pressStart = ((GStateFactory)_stateFactory).PressStart();
        GStateBase play = ((GStateFactory)_stateFactory).Play();

        // transitions
        at(nan, init, new FuncPredicate(() =>
            true
        ));
        at(init, pressStartIn, new FuncPredicate(() =>
        _initDone
        ));
        at(pressStartIn, pressStart, new FuncPredicate(() =>
            _pressStartInDone
        ));
        at(pressStart, play, new FuncPredicate(() =>
            _startPlay
        ));

        _stateMachine.SetState(nan);
    }
}