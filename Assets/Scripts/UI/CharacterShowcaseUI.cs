using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterShowcaseUI : MonoBehaviour
{
    [Header("角色展示Panel")]
    public GameObject characterShowcasePanel;  // 包含ScrollRect的Panel
    public ScrollRect characterScrollView;     // 在characterShowcasePanel中的ScrollRect
    public Transform characterContent;         // ScrollView的Content
    public Scrollbar horizontalScrollbar;      // 滚动条

    [Header("角色组预制体")]
    public GameObject characterGroupPrefab;    // 包含图片、名字、背景的预制体

    [Header("创建角色按钮")]
    public Button createCharacterButton;       // 在Canvas上的创建按钮

    private List<CharacterGroupUI> characterGroups = new List<CharacterGroupUI>();

    private void Start()
    {
        // 初始化UI
        InitializeUI();
        
        // 加载角色列表
        RefreshCharacterList();
    }

    private void InitializeUI()
    {
        // 确保角色展示Panel是激活的
        characterShowcasePanel.SetActive(true);
        
        // 设置滚动视图
        characterScrollView.horizontal = true;
        characterScrollView.vertical = false;
        
        // 设置Content的布局
        RectTransform contentRect = characterContent.GetComponent<RectTransform>();
        if (contentRect != null)
        {
            // 设置Content的锚点
            contentRect.anchorMin = new Vector2(0, 0.5f);
            contentRect.anchorMax = new Vector2(0, 0.5f);
            contentRect.pivot = new Vector2(0, 0.5f);
            
            // 设置Content的位置
            contentRect.anchoredPosition = new Vector2(0, 0);
            
            // 设置Content的大小
            contentRect.sizeDelta = new Vector2(0, contentRect.sizeDelta.y);
        }
        
        // 设置创建角色按钮
        createCharacterButton.onClick.AddListener(OnCreateNewCharacterClicked);
    }

    public void RefreshCharacterList()
    {
        // 清除现有角色组
        foreach (var group in characterGroups)
        {
            Destroy(group.gameObject);
        }
        characterGroups.Clear();

        // 获取所有角色数据
        var characters = CharacterManager.Instance.GetAllCharacters();
        
        // 创建新的角色组
        foreach (var character in characters)
        {
            GameObject groupObj = Instantiate(characterGroupPrefab, characterContent);
            
            // 重置Transform
            RectTransform rectTransform = groupObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one;
                rectTransform.localRotation = Quaternion.identity;
                rectTransform.localPosition = Vector3.zero;
                
                // 设置角色组的布局
                rectTransform.anchorMin = new Vector2(0, 0.5f);
                rectTransform.anchorMax = new Vector2(0, 0.5f);
                rectTransform.pivot = new Vector2(0, 0.5f);
                
                // 设置位置
                float spacing = 20f; // 角色组之间的间距
                rectTransform.anchoredPosition = new Vector2(
                    characterGroups.Count * (200 + spacing), 
                    0
                );
            }
            
            CharacterGroupUI groupUI = groupObj.GetComponent<CharacterGroupUI>();
            if (groupUI != null)
            {
                groupUI.Initialize(character, this);
                characterGroups.Add(groupUI);
            }
        }
        
        // 更新Content的宽度
        UpdateContentWidth();
    }

    private void UpdateContentWidth()
    {
        RectTransform contentRect = characterContent.GetComponent<RectTransform>();
        if (contentRect != null)
        {
            // 设置Content的宽度为所有角色组的总宽度
            float spacing = 40f; // 角色组之间的间距
            float totalWidth = characterGroups.Count * (200+ spacing);
            contentRect.sizeDelta = new Vector2(totalWidth, contentRect.sizeDelta.y);
        }
    }

    private void OnCreateNewCharacterClicked()
    {
        // 使用TransitionManager进行场景切换
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "H4");
    }

    // 当角色被选中时调用
    public void OnCharacterSelected(CharacterData characterData)
    {
        // 使用TransitionManager进行场景切换
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "H3");
    }
} 