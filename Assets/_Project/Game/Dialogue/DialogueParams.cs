using System;

[Serializable]
public class DialogueParams
{
    public Dialogue _dialogue;
    public string _text;
    public bool _waitForConfirmation;
    public bool _chainRandomOnComplete;
    
    public bool _triggerPlayerJump;
    
    public bool _triggerPlayerMove1;
    public bool _triggerPlayerMove2;
    public bool _triggerPlayerMove3;
    
    public bool _triggerGenieAppear;
    public bool _triggerGenieMove1;
    public bool _triggerGenieMove2;
    public bool _triggerGenieMove3;
    public bool _triggerGenieAttack;
    
    public bool _triggerGenieShake;
    public bool _triggerGenieSoundEffect;
}