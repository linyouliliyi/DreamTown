using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportManager : Singleton<TeleportManager>
{
    // 场景名称常量
    public const string CHARACTER_SHOWCASE_SCENE = "CharacterShowcase";
    public const string CHARACTER_CREATION_SCENE = "CharacterCreation";
    public const string CHARACTER_INFO_INPUT_SCENE = "CharacterInfoInput";
    public const string CHARACTER_PREVIEW_SCENE = "CharacterPreview";

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void TeleportToCharacterShowcase()
    {
        // 这里可以添加场景切换前的处理逻辑
        // 例如：保存当前场景状态、播放过渡动画等
        UnityEngine.SceneManagement.SceneManager.LoadScene(CHARACTER_SHOWCASE_SCENE);
    }

    public void TeleportToCharacterCreation()
    {
        // 开始创建新角色
        CharacterManager.Instance.StartCharacterCreation();
        UnityEngine.SceneManagement.SceneManager.LoadScene(CHARACTER_CREATION_SCENE);
    }

    public void TeleportToCharacterInfoInput()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CHARACTER_INFO_INPUT_SCENE);
    }

    public void TeleportToCharacterPreview()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CHARACTER_PREVIEW_SCENE);
    }
} 