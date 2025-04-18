using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ButtonSoundManager.Instance.AddSoundToAllButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
