using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterInventoryUI : MonoBehaviour
{
    [Header("UI组件")]
    public GameObject characterListPanel;
    public GameObject characterGroupPrefab;  // 使用CharacterGroupUI预制体
    public Transform characterListContent;
    public Button closeButton;

    private List<CharacterGroupUI> characterGroups = new List<CharacterGroupUI>();

    private void Start()
    {
        closeButton.onClick.AddListener(OnCloseClicked);
        RefreshCharacterList();
    }

    private void RefreshCharacterList()
    {
        // 清除现有列表
        foreach (var group in characterGroups)
        {
            Destroy(group.gameObject);
        }
        characterGroups.Clear();

        // 获取所有角色
        List<CharacterData> characters = CharacterManager.Instance.GetAllCharacters();
        
        // 创建角色列表项
        foreach (var character in characters)
        {
            GameObject item = Instantiate(characterGroupPrefab, characterListContent);
            CharacterGroupUI groupUI = item.GetComponent<CharacterGroupUI>();
            groupUI.Initialize(character, null);  // 不需要parentUI，因为不需要显示详情
            characterGroups.Add(groupUI);
        }
    }

    private void OnCloseClicked()
    {
        gameObject.SetActive(false);
    }
} 