using System;
using UnityEngine;
using Febucci.UI.Core;
using Febucci.UI.Core.Parsing;

[Serializable]
[CreateAssetMenu(fileName = "DialogueWait", menuName = "Text Animator/Actions/DialogueWait",
    order = 99)]
[TagInfo("dialogue_wait")]
public sealed class DialogueWait : Febucci.UI.Actions.ActionScriptableBase
{
    public GenericEvent _event;

    public override System.Collections.IEnumerator DoAction(ActionMarker action, TypewriterCore typewriter,
        TypingInfo typingInfo)
    {
        yield return null;
        GenericEventOpts opts = new GenericEventOpts
        {
        };
        _event.Invoke(opts);
    }
}