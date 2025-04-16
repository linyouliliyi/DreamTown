using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPreviewUI : MonoBehaviour
{
    public RawImage characterImage;
    public RawImage gifImage;
    public TextMeshProUGUI characterInfoText;
    public Button confirmButton;
    public Button backButton;

    private CharacterData currentCharacter;

    private void Start()
    {
        // 获取当前正在创建的角色
        currentCharacter = CharacterManager.Instance.GetCurrentCreatingCharacter();
        if (currentCharacter == null)
        {
            Debug.LogError("没有找到正在创建的角色");
            TeleportManager.Instance.TeleportToCharacterShowcase();
            return;
        }

        // 加载并显示角色图片
        StartCoroutine(LoadImage(currentCharacter.enhancedPhotoPath, characterImage));

        // 显示角色信息
        characterInfoText.text = $"名字: {currentCharacter.characterName}\n" +
                               $"性别: {currentCharacter.gender}\n" +
                               $"性格: {currentCharacter.personality}\n" +
                               $"喜好: {currentCharacter.preferences}\n" +
                               $"技能: {currentCharacter.skills}";

        // TODO: 加载并显示GIF动画
        // 这里可以添加GIF加载和播放的逻辑

        confirmButton.onClick.AddListener(OnConfirmClicked);
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnConfirmClicked()
    {
        // 完成角色创建
        CharacterManager.Instance.FinishCharacterCreation();
        TeleportManager.Instance.TeleportToCharacterShowcase();
    }

    private void OnBackClicked()
    {
        TeleportManager.Instance.TeleportToCharacterInfoInput();
    }

    private System.Collections.IEnumerator LoadImage(string path, RawImage targetImage)
    {
        if (string.IsNullOrEmpty(path)) yield break;

        string filePath = "file://" + path;
        using (WWW www = new WWW(filePath))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                targetImage.texture = www.texture;
            }
        }
    }
} 