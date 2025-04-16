using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CharacterGroupUI : MonoBehaviour
{
    [Header("UI组件")]
    public RawImage characterImage;
    public TextMeshProUGUI characterNameText;
    public Button selectButton;  // 内层Panel上的Button组件
    public GameObject innerPanel; // 内层Panel，用于显示内容

    [Header("颜色设置")]
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(0.8f, 0.8f, 1f, 1f);

    private CharacterData characterData;
    private CharacterShowcaseUI showcaseUI;
    private bool isSelected = false;
    private Image backgroundImage;  // 内层Panel的Image组件

    private void Awake()
    {
        // 获取内层Panel的Image组件
        if (innerPanel != null)
        {
            backgroundImage = innerPanel.GetComponent<Image>();
            if (backgroundImage == null)
            {
                backgroundImage = innerPanel.AddComponent<Image>();
            }

            // 获取或添加Button组件到内层Panel
            selectButton = innerPanel.GetComponent<Button>();
            if (selectButton == null)
            {
                selectButton = innerPanel.AddComponent<Button>();
            }
            selectButton.onClick.AddListener(OnSelected);
        }
        else
        {
            Debug.LogError("Inner panel is not assigned!");
        }
    }

    public void Initialize(CharacterData data, CharacterShowcaseUI ui)
    {
        characterData = data;
        showcaseUI = ui;
        
        // 设置角色名字
        if (characterNameText != null)
        {
            Debug.Log($"Character data: {data.characterName}, Text component: {characterNameText.gameObject.name}");
            characterNameText.text = data.characterName;
            
            // 检查TextMeshProUGUI的设置
            Debug.Log($"TextMeshPro settings - Color: {characterNameText.color}, Font: {characterNameText.font}, Size: {characterNameText.fontSize}");
            
            // 确保文本可见
            characterNameText.color = new Color(1f, 1f, 1f, 1f); // 设置为完全不透明
            characterNameText.fontSize = 24; // 设置合适的字体大小
        }
        else
        {
            Debug.LogError("characterNameText is not assigned! Please check the prefab setup.");
        }
        
        // 加载角色图片
        if (!string.IsNullOrEmpty(data.originalPhotoPath))
        {
            StartCoroutine(LoadImage(data.originalPhotoPath));
        }
        else if (!string.IsNullOrEmpty(data.enhancedPhotoPath))
        {
            StartCoroutine(LoadImage(data.enhancedPhotoPath));
        }
        else
        {
            Debug.LogWarning($"No image path found for character: {data.characterName}");
            characterImage.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        }
        
        // 初始状态
        SetSelected(false);
    }

    private IEnumerator LoadImage(string path)
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
            // 直接设置图片
            characterImage.texture = texture;
            characterImage.color = Color.white;
            
            // 设置UV Rect来修正显示
            characterImage.uvRect = new Rect(0, 0, 1, 1);
        }
        else
        {
            Debug.LogError($"Failed to load image: {path}");
            characterImage.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        }
    }

    private void OnSelected()
    {
        // 切换选中状态
        SetSelected(!isSelected);
        
        // 通知CharacterShowcaseUI角色被选中
        if (showcaseUI != null)
        {
            showcaseUI.OnCharacterSelected(characterData);
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (backgroundImage != null)
        {
            backgroundImage.color = selected ? selectedColor : normalColor;
        }
    }
} 