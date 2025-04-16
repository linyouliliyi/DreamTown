using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    public string id;
    public string name;
    public string characterName;  // 角色名称
    public int age;
    public string gender;
    public string personality;
    public string background;
    public string originalPhotoPath;
    public string enhancedPhotoPath;
    public List<string> preferences = new List<string>();  // 角色偏好
    public List<string> skills = new List<string>();      // 角色技能
} 