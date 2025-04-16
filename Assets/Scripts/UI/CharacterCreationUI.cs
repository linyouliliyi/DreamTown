using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class CharacterCreationUI : MonoBehaviour
{
    public GameObject characterCreationPanel;
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
        confirmButton.onClick.AddListener(ConfirmCharacter);
        cancelButton.onClick.AddListener(CancelCreation);
    }

    public void ShowCreationPanel(CharacterData character)
    {
        currentCharacter = character;
        characterCreationPanel.SetActive(true);
        
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
        // 可以添加一个计时器或等待用户手动停止录音
    }

    private void ConfirmCharacter()
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

        // 关闭创建面板
        characterCreationPanel.SetActive(false);
    }

    private void CancelCreation()
    {
        characterCreationPanel.SetActive(false);
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