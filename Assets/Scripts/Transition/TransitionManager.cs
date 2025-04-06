using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TransitionManager : Singleton<TransitionManager>
{
  public string startingScene;

  public float fadeDuration;
  public CanvasGroup fadeCanvasGroup;
  public bool isFade;

   public void Transition(string from, string to)
   {
    if(!isFade)
      StartCoroutine(TransitionToScene(from, to));
   }

   private void Start()
   {
    StartCoroutine(TransitionToScene(string.Empty, startingScene));
   }

   private IEnumerator TransitionToScene(string from, string to)
   {
    yield return Fade(1);
    if( from != string.Empty)
    {
      EventHandler.CallBeforeSceneUnloadFadeOutEvent();
      yield return SceneManager.UnloadSceneAsync(from);
    }
    yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

    Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
    SceneManager.SetActiveScene(newScene);

    EventHandler.CallAfterSceneUnloadFadeInEvent();

    yield return Fade(0);
   }

    private IEnumerator Fade(float finalAlpha)
    {
      isFade = true;

      fadeCanvasGroup.blocksRaycasts = true;

      float fadeSpeed = Mathf.Abs(fadeCanvasGroup.alpha - finalAlpha) / fadeDuration;

      while (!Mathf.Approximately(fadeCanvasGroup.alpha, finalAlpha))
      {
        fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
        yield return null;
      }

      fadeCanvasGroup.blocksRaycasts = false;

      isFade = false;
    }
   
}

