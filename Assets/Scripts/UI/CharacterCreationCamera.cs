using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class CharacterCreationCamera : MonoBehaviour
{
    [Header("UI组件")]
    public RawImage cameraView;           // 显示相机画面的RawImage
    public Button captureButton;          // 拍照按钮
    public Button uploadButton;           // 上传图片按钮
    public Button confirmButton;          // 确认按钮
    public Button backButton;             // 返回按钮

    private Vector2 originalSize;         // RawImage的初始大小
    private string currentPhotoPath;      // 当前照片路径

    private void Start()
    {
        // 记录RawImage的初始大小
        originalSize = cameraView.rectTransform.sizeDelta;

        // 设置按钮事件
        captureButton.onClick.AddListener(CapturePhoto);
        uploadButton.onClick.AddListener(UploadPhoto);
        confirmButton.onClick.AddListener(OnConfirmClicked);
        backButton.onClick.AddListener(OnBackClicked);

        // 初始时禁用确认按钮
        confirmButton.interactable = false;
    }

    // 拍照
    private void CapturePhoto()
    {
        NativeCall.OpenCamera((Texture2D tex) =>
        {
            // 显示照片
            cameraView.texture = tex;
            cameraView.rectTransform.sizeDelta = originalSize;
            
            // 保存临时照片
            SaveTemporaryPhoto(tex);
            
            // 启用确认按钮
            confirmButton.interactable = true;
        });
    }

    // 上传图片
    private void UploadPhoto()
    {
        NativeCall.OpenPhoto((Texture2D tex) =>
        {
            // 显示照片
            cameraView.texture = tex;
            cameraView.rectTransform.sizeDelta = originalSize;
            
            // 保存临时照片
            SaveTemporaryPhoto(tex);
            
            // 启用确认按钮
            confirmButton.interactable = true;
        });
    }

    // 保存临时照片
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

    // 确认按钮点击事件
    private void OnConfirmClicked()
    {
        if (string.IsNullOrEmpty(currentPhotoPath))
        {
            Debug.LogWarning("请先拍照或上传图片");
            return;
        }

        // 开始创建新角色
        CharacterManager.Instance.StartCharacterCreation();
        
        // 保存原始照片路径
        CharacterData newCharacter = CharacterManager.Instance.GetCurrentCreatingCharacter();
        newCharacter.originalPhotoPath = currentPhotoPath;

        // 切换到信息输入场景
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "H5");
    }

    // 返回按钮点击事件
    private void OnBackClicked()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "H2");
    }
} 