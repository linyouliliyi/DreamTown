using UnityEngine;
using UnityEngine.UI;

public class SampleCamera : MonoBehaviour
{
    public Button openCamera, openPhoto;
    public Button savePhoto;
    public RawImage rawImage;

    private Vector2 originalSize; // 用于存储 RawImage 的初始大小

    void Start ()
    {
        // 记录 RawImage 的初始大小
        originalSize = rawImage.rectTransform.sizeDelta;

        openCamera.onClick.AddListener(OpenCamera);
        openPhoto.onClick.AddListener(OpenPhoto);
        savePhoto.onClick.AddListener(SavePhoto);
    }

    // 打开相机
    private void OpenCamera()
    {
        NativeCall.OpenCamera((Texture2D tex) =>
        {
            rawImage.texture = tex;
            rawImage.rectTransform.sizeDelta = originalSize; // 使用原始大小
        });
    }

    // 打开相册
    private void OpenPhoto()
    {
        NativeCall.OpenPhoto((Texture2D tex) =>
        {
            rawImage.texture = tex;
            rawImage.rectTransform.sizeDelta = originalSize; // 使用原始大小
        });
    }

    // 保存照片
    private void SavePhoto()
    {
        NativeCall.SavePhoto(rawImage.texture as Texture2D);
    }
}