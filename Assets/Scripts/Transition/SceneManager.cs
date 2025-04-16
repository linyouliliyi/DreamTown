using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    // 场景名称常量
    public const string CHARACTER_SHOWCASE_SCENE = "CharacterShowcase";
    public const string CHARACTER_CREATION_SCENE = "CharacterCreation";
    public const string CHARACTER_INFO_INPUT_SCENE = "CharacterInfoInput";
    public const string CHARACTER_PREVIEW_SCENE = "CharacterPreview";

    public void LoadCharacterShowcase()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CHARACTER_SHOWCASE_SCENE);
    }

    public void LoadCharacterCreation()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CHARACTER_CREATION_SCENE);
    }

    public void LoadCharacterInfoInput()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CHARACTER_INFO_INPUT_SCENE);
    }

    public void LoadCharacterPreview()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CHARACTER_PREVIEW_SCENE);
    }
} 