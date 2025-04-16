using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class CharacterInfoInputUI : MonoBehaviour
{
    public GameObject infoInputPanel;
    public RawImage characterImage;
    public TMP_InputField nameInput;
    public TMP_InputField genderInput;
    public TMP_InputField personalityInput;
    public TMP_InputField preferencesInput;
    public TMP_InputField skillsInput;
    
    public Button voiceInputButton;
    public Button confirmButton;
    public Button cancelButton;

    private CharacterData currentCharacter;

    private void Start()
    {
        voiceInputButton.onClick.AddListener(StartVoiceInput);
        confirmButton.onClick.AddListener(ConfirmInfo);
        cancelButton.onClick.AddListener(CancelInput);
    }

    public void ShowInfoInputPanel(CharacterData character)
    {
        currentCharacter = character;
        infoInputPanel.SetActive(true);
        
        // 加载并显示角色图片
        StartCoroutine(LoadImage(character.originalPhotoPath, characterImage));
        
        // 设置默认值
        nameInput.text = character.characterName;
        genderInput.text = character.gender;
        personalityInput.text = character.personality;
        preferencesInput.text = string.Join(", ", character.preferences);
        skillsInput.text = string.Join(", ", character.skills);
    }

    private void StartVoiceInput()
    {
        VoiceRecognitionManager.Instance.StartRecording();
    }

    private void ConfirmInfo()
    {
        // 更新角色信息
        currentCharacter.characterName = nameInput.text;
        currentCharacter.gender = genderInput.text;
        currentCharacter.personality = personalityInput.text;
        
        // 处理列表类型
        currentCharacter.preferences = preferencesInput.text.Split(',').Select(p => p.Trim()).ToList();
        currentCharacter.skills = skillsInput.text.Split(',').Select(s => s.Trim()).ToList();

        // 保存更新后的角色数据
        CharacterManager.Instance.SaveCharacter(currentCharacter);

        // 关闭输入面板
        infoInputPanel.SetActive(false);
    }

    private void CancelInput()
    {
        infoInputPanel.SetActive(false);
    }

    private System.Collections.IEnumerator LoadImage(string path, RawImage targetImage)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Image path is empty");
            yield break;
        }

        // 使用Resources.Load加载图片
        Texture2D texture = Resources.Load<Texture2D>(path);
        if (texture != null)
        {
            targetImage.texture = texture;
        }
        else
        {
            Debug.LogError($"Failed to load image: {path}");
        }
    }
} 