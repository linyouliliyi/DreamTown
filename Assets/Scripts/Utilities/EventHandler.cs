using System;
using UnityEngine;

public static class EventHandler 
{
    

    public static event Action BeforeSceneUnloadFadeOutEvent;
    public static void CallBeforeSceneUnloadFadeOutEvent()
    {
        BeforeSceneUnloadFadeOutEvent.Invoke();
    }

    public static event Action AfterSceneUnloadFadeInEvent;
    public static void CallAfterSceneUnloadFadeInEvent()
    {
        AfterSceneUnloadFadeInEvent.Invoke();
    }

   
    
    
}
