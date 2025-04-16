using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;
    public static CharacterManager Instance => instance;

    private List<CharacterData> characters = new List<CharacterData>();
    private CharacterData currentCreatingCharacter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDefaultCharacter();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDefaultCharacter()
    {
        // 检查是否已经存在默认角色
        if (characters.Count > 0) return;

        // 创建默认角色 Mary
        CharacterData mary = new CharacterData
        {
            id = "mary_001",
            name = "Mary",
            characterName = "Mary",
            age = 25,
            gender = "Female",
            personality = "Friendly and outgoing",
            background = "A cheerful girl who loves helping others",
            originalPhotoPath = "DefaultPhotos/mary_default", // Resources文件夹下的路径
            enhancedPhotoPath = "DefaultPhotos/mary_default"  // 暂时使用同一张图片
        };

        characters.Add(mary);
        Debug.Log("Default character Mary has been initialized");
    }

    private string GetDefaultPhotoPath()
    {
        // 确保Resources文件夹存在
        string resourcesPath = Path.Combine(Application.dataPath, "Resources");
        if (!Directory.Exists(resourcesPath))
        {
            Directory.CreateDirectory(resourcesPath);
        }

        // 确保DefaultPhotos文件夹存在
        string defaultPhotosPath = Path.Combine(resourcesPath, "DefaultPhotos");
        if (!Directory.Exists(defaultPhotosPath))
        {
            Directory.CreateDirectory(defaultPhotosPath);
        }

        // 返回默认照片的路径
        return Path.Combine("DefaultPhotos", "mary_default.png");
    }

    public void StartCharacterCreation()
    {
        currentCreatingCharacter = new CharacterData();
    }

    public CharacterData GetCurrentCreatingCharacter()
    {
        return currentCreatingCharacter;
    }

    public List<CharacterData> GetAllCharacters()
    {
        return characters;
    }

    public void AddCharacter(CharacterData character)
    {
        characters.Add(character);
    }

    public CharacterData GetCharacterById(string id)
    {
        return characters.Find(c => c.id == id);
    }

    public void SaveCharacter(CharacterData character)
    {
        if (character == null) return;

        // 如果角色已存在，更新它
        int index = characters.FindIndex(c => c.id == character.id);
        if (index >= 0)
        {
            characters[index] = character;
        }
        else
        {
            // 否则添加新角色
            characters.Add(character);
        }
    }

    public void FinishCharacterCreation()
    {
        if (currentCreatingCharacter != null)
        {
            SaveCharacter(currentCreatingCharacter);
            currentCreatingCharacter = null;
        }
    }
} 