using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SampleCamera : MonoBehaviour
{
    public Button openCamera, openPhoto;
    public Button savePhoto;
    public RawImage rawImage;
    public Button createCharacterButton; // 新增创建角色按钮
    public CharacterCreationUI characterCreationUI; // 新增角色创建UI引用

    private Vector2 originalSize; // 用于存储 RawImage 的初始大小
    private string currentPhotoPath; // 存储当前照片路径

    void Start ()
    {
        // 记录 RawImage 的初始大小
        originalSize = rawImage.rectTransform.sizeDelta;

        openCamera.onClick.AddListener(OpenCamera);
        openPhoto.onClick.AddListener(OpenPhoto);
        savePhoto.onClick.AddListener(SavePhoto);
        createCharacterButton.onClick.AddListener(CreateNewCharacter);
    }

    // 打开相机
    private void OpenCamera()
    {
        NativeCall.OpenCamera((Texture2D tex) =>
        {
            rawImage.texture = tex;
            rawImage.rectTransform.sizeDelta = originalSize; // 使用原始大小
            SaveTemporaryPhoto(tex); // 保存临时照片
        });
    }

    // 打开相册
    private void OpenPhoto()
    {
        NativeCall.OpenPhoto((Texture2D tex) =>
        {
            rawImage.texture = tex;
            rawImage.rectTransform.sizeDelta = originalSize; // 使用原始大小
            SaveTemporaryPhoto(tex); // 保存临时照片
        });
    }

    // 保存照片
    private void SavePhoto()
    {
        if (rawImage.texture != null)
        {
            NativeCall.SavePhoto(rawImage.texture as Texture2D);
        }
    }

    private void SaveTemporaryPhoto(Texture2D texture)
    {
        // 创建临时目录
        string tempPath = Path.Combine(Application.persistentDataPath, "TempPhotos");
        if (!Directory.Exists(tempPath))
        {
            Directory.CreateDirectory(tempPath);
        }

        // 生成唯一的文件名
        string fileName = $"temp_photo_{DateTime.Now.Ticks}.png";
        currentPhotoPath = Path.Combine(tempPath, fileName);

        // 保存照片
        File.WriteAllBytes(currentPhotoPath, texture.EncodeToPNG());
    }

    private void CreateNewCharacter()
    {
        if (string.IsNullOrEmpty(currentPhotoPath))
        {
            Debug.LogWarning("请先拍照或选择照片");
            return;
        }

        // 开始创建新角色
        CharacterManager.Instance.StartCharacterCreation();
        
        // 保存原始照片路径
        CharacterData newCharacter = CharacterManager.Instance.GetCurrentCreatingCharacter();
        newCharacter.originalPhotoPath = currentPhotoPath;

        // 显示角色创建界面
        characterCreationUI.ShowCreationPanel(newCharacter);

        // 清空当前照片
        currentPhotoPath = null;
        rawImage.texture = null;
    }
}