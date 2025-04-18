using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class CharacterInfoInputUI : MonoBehaviour
{
    [Header("录音按钮")]
    public Button recordButton;           // 录音按钮

    [Header("文本显示")]
    public TextMeshProUGUI resultText;    // 结果显示文本
    public string presetText = "Emma is a queen of kingdom, She is very beautiful. like reading books,"; // 预设文本

    [Header("关键词按钮")]
    public Button[] keywordButtons;       // 关键词按钮数组
    public Color[] normalColors;          // 关键词按钮正常颜色
    public Color[] selectedColors;        // 关键词按钮选中颜色
    private bool[] isSelected;            // 记录每个关键词的选中状态

    private bool isRecording = false;     // 是否正在录音
    private float recordTime = 0f;        // 录音时间
    private Sprite originalSprite;        // 记录原始按钮图片

    private void Start()
    {
        // 保存原始按钮图片
        if (recordButton != null)
        {
            originalSprite = recordButton.image.sprite;
            
            // 添加按钮事件监听
            EventTrigger trigger = recordButton.gameObject.AddComponent<EventTrigger>();
            
            // 添加按下事件
            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((data) => { OnRecordButtonDown(); });
            trigger.triggers.Add(entryDown);
            
            // 添加松开事件
            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((data) => { OnRecordButtonUp(); });
            trigger.triggers.Add(entryUp);
        }

        // 初始化关键词按钮状态
        isSelected = new bool[keywordButtons.Length];
        
        // 设置按钮颜色
        for (int i = 0; i < keywordButtons.Length; i++)
        {
            int index = i; // 闭包需要局部变量
            keywordButtons[i].onClick.AddListener(() => ToggleKeyword(index));
            
            // 如果颜色数组长度不够，使用默认颜色
            Color normalColor = i < normalColors.Length ? normalColors[i] : Color.white;
            Color selectedColor = i < selectedColors.Length ? selectedColors[i] : Color.gray;
            
            // 设置初始颜色
            if (keywordButtons[i].image != null)
            {
                keywordButtons[i].image.color = normalColor;
            }
        }
    }

    private void Update()
    {
        if (isRecording)
        {
            recordTime += Time.deltaTime;
            // 这里可以添加录音动画效果
        }
    }

    // 录音按钮按下事件
    private void OnRecordButtonDown()
    {
        Debug.Log("录音按钮按下");
        StartRecording();
    }

    // 录音按钮松开事件
    private void OnRecordButtonUp()
    {
        Debug.Log("录音按钮松开");
        if (isRecording)
        {
            StopRecording();
        }
    }

    // 开始录音
    private void StartRecording()
    {
        isRecording = true;
        recordTime = 0f;
        
        // 清空结果文本
        if (resultText != null)
        {
            resultText.text = "";
        }
    }

    // 停止录音
    private void StopRecording()
    {
        isRecording = false;
        
        // 显示预设文本
        if (resultText != null)
        {
            Debug.Log("显示预设文本: " + presetText);
            resultText.text = presetText;
        }
        else
        {
            Debug.LogError("resultText is null!");
        }
    }

    // 切换关键词选中状态
    private void ToggleKeyword(int index)
    {
        if (index >= 0 && index < keywordButtons.Length)
        {
            isSelected[index] = !isSelected[index];
            UpdateKeywordButtonColor(index);
        }
    }

    // 更新关键词按钮颜色
    private void UpdateKeywordButtonColor(int index)
    {
        if (index >= 0 && index < keywordButtons.Length)
        {
            Image buttonImage = keywordButtons[index].image;
            if (buttonImage != null)
            {
                // 如果颜色数组长度不够，使用默认颜色
                Color normalColor = index < normalColors.Length ? normalColors[index] : Color.white;
                Color selectedColor = index < selectedColors.Length ? selectedColors[index] : Color.gray;
                
                buttonImage.color = isSelected[index] ? selectedColor : normalColor;
            }
        }
    }
} 