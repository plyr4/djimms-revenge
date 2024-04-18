using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GStateLoadPlay : GStateBase
{
    public GStateLoadPlay(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        _context._loadPlayDone = false;


        Scene play = SceneManager.GetSceneByName("Play");
        if (play != null && play.isLoaded)
        {
            CoroutineRunner.instance.StartCoroutine(unloadScene());
        }
        else
        {
            SceneManager.LoadScene("Play", LoadSceneMode.Additive);
        
            ScreenTransition.Instance.DelayedOpen(1f);   
        }
    }

    IEnumerator unloadScene()
    {
        SceneManager.UnloadSceneAsync("Play");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene("Play", LoadSceneMode.Additive);
        ScreenTransition.Instance.DelayedOpen(1f);  
    }
    
    public override void OnExit()
    {
        base.OnExit();

        if (_context == null) return;

        _context._loadPlayDone = false;
    }
}