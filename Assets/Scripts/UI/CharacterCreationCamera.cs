using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;
using TMPro;

public class CharacterCreationCamera : MonoBehaviour
{
    [Header("UI组件")]
    public RawImage cameraView;           // 显示相机画面的RawImage
    public RawImage resultView;           // 显示处理结果的RawImage
    public Button captureButton;          // 拍照按钮
    public Button uploadButton;           // 上传图片按钮
    public Button backButton;             // 返回按钮
    public Slider styleSlider;            // 风格滑动条
    public GameObject loadingPanel;       // 加载动画面板
    public Image loadingImage;            // 加载动画图片
    public TextMeshProUGUI loadingText;   // 加载提示文本

    [Header("预设图片")]
    public Texture2D style1Texture;       // 第一种风格的图片
    public Texture2D style2Texture;       // 第二种风格的图片

    private Vector2 originalSize;         // RawImage的初始大小
    private bool isProcessing = false;    // 是否正在处理
    private float loadingTime = 3f;       // 加载时间
    private float currentLoadingTime = 0f; // 当前加载时间
    private float rotationSpeed = 360f;   // 加载动画旋转速度
    private bool isFirstLoad = true;      // 是否是第一次加载

    private void Awake()
    {
        // 确保loadingPanel默认隐藏
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }

    private void Start()
    {
        // 记录RawImage的初始大小
        originalSize = cameraView.rectTransform.sizeDelta;

        // 检查预设图片是否已设置
        if (style1Texture == null || style2Texture == null)
        {
            Debug.LogError("Please assign style textures in the Inspector.");
        }

        // 设置按钮事件
        captureButton.onClick.AddListener(CapturePhoto);
        uploadButton.onClick.AddListener(UploadPhoto);
        backButton.onClick.AddListener(OnBackClicked);
        styleSlider.onValueChanged.AddListener(OnStyleChanged);

        // 初始时禁用滑动条
        styleSlider.interactable = false;
        
        // 确保loadingPanel是隐藏的
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        // 初始时隐藏结果图片
        if (resultView != null)
        {
            resultView.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isProcessing)
        {
            // 更新加载动画
            currentLoadingTime += Time.deltaTime;
            loadingImage.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            
            // 更新加载进度文本
            float progress = Mathf.Clamp01(currentLoadingTime / loadingTime);
            loadingText.text = $"Generating... {Mathf.RoundToInt(progress * 100)}%";

            // 检查是否完成加载
            if (currentLoadingTime >= loadingTime)
            {
                FinishProcessing();
            }
        }
    }

    // 拍照
    private void CapturePhoto()
    {
        NativeCall.OpenCamera((Texture2D tex) =>
        {
            // 显示照片
            cameraView.texture = tex;
            cameraView.rectTransform.sizeDelta = originalSize;
            
            // 开始处理动画
            StartProcessing();
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
            
            // 开始处理动画
            StartProcessing();
        });
    }

    // 开始处理动画
    private void StartProcessing()
    {
        isProcessing = true;
        currentLoadingTime = 0f;
        loadingPanel.SetActive(true);
        styleSlider.interactable = true;

        // 隐藏结果图片
        if (resultView != null)
        {
            resultView.gameObject.SetActive(false);
        }
    }

    // 完成处理
    private void FinishProcessing()
    {
        isProcessing = false;
        loadingPanel.SetActive(false);

        // 显示结果图片
        if (resultView != null)
        {
            resultView.gameObject.SetActive(true);
            // 根据滑动条的值显示对应的风格图片
            UpdateResultImage();
        }
    }

    // 更新结果图片
    private void UpdateResultImage()
    {
        if (resultView != null)
        {
            // 根据滑动条的值选择图片
            Texture2D selectedTexture = styleSlider.value < 0.5f ? style1Texture : style2Texture;
            if (selectedTexture != null)
            {
                resultView.texture = selectedTexture;
                
                // 计算图片的宽高比
                float textureAspect = (float)selectedTexture.width / selectedTexture.height;
                float viewAspect = resultView.rectTransform.rect.width / resultView.rectTransform.rect.height;
                
                // 调整UV Rect以保持图片比例
                Rect uvRect = new Rect(0, 0, 1, 1);
                
                if (textureAspect > viewAspect)
                {
                    // 图片更宽，需要调整高度
                    float scale = viewAspect / textureAspect;
                    uvRect.height = scale;
                    uvRect.y = (1 - scale) * 0.5f;
                }
                else
                {
                    // 图片更高，需要调整宽度
                    float scale = textureAspect / viewAspect;
                    uvRect.width = scale;
                    uvRect.x = (1 - scale) * 0.5f;
                }
                
                resultView.uvRect = uvRect;
            }
            else
            {
                Debug.LogError("Selected style texture is null");
            }
        }
    }

    // 风格滑动条值改变
    private void OnStyleChanged(float value)
    {
        if (!isProcessing)
        {
            StartProcessing();
        }
    }

    // 返回按钮点击事件
    private void OnBackClicked()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        TransitionManager.Instance.Transition(currentScene, "H2");
    }
} 