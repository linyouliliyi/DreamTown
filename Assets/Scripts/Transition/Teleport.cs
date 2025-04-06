using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public string SceneFrom;
    public string SceneToGo;

    public void TeleportToScene()
    {
        TransitionManager.Instance.Transition(SceneFrom, SceneToGo);
    }
}
