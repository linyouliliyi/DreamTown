using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //private Dictionary<ItemName, bool> itemAvaliableDict = new Dictionary<ItemName, bool>();
    private Dictionary<string, bool> interactivesDoneDict = new Dictionary<string, bool>();

    private  void OnEnable()
    {
        EventHandler.BeforeSceneUnloadFadeOutEvent += OnBeforeSceneUnloadFadeOutEvent;
        EventHandler.AfterSceneUnloadFadeInEvent += OnAfterSceneUnloadFadeInEvent;
        //EventHandler.UpdateUIEvent += OnUpdateUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadFadeOutEvent -= OnBeforeSceneUnloadFadeOutEvent;
        EventHandler.AfterSceneUnloadFadeInEvent -= OnAfterSceneUnloadFadeInEvent;
        //EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
    }


    private void OnBeforeSceneUnloadFadeOutEvent()
    {
       
    }

    private void OnAfterSceneUnloadFadeInEvent()
    {
        
    }

        
}
