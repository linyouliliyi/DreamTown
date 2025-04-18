using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class ButtonSoundManager : MonoBehaviour
{
    public static ButtonSoundManager Instance { get; private set; }

    [Header("按钮音效")]
    public AudioClip defaultClickSound;   // 默认点击音效
    public float volume = 0.5f;           // 音量大小

    private AudioSource audioSource;
    private Dictionary<Button, AudioClip> buttonSounds = new Dictionary<Button, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    // 为所有按钮添加音效
    public void AddSoundToAllButtons()
    {
        // 查找场景中所有的按钮
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            AddSoundToButton(button);
        }
    }

    // 为单个按钮添加音效
    public void AddSoundToButton(Button button)
    {
        if (button == null) return;

        // 如果按钮已经有音效，先移除旧的监听器
        if (buttonSounds.ContainsKey(button))
        {
            button.onClick.RemoveAllListeners();
        }

        // 添加新的监听器
        button.onClick.AddListener(() => PlayButtonSound(button));
        
        // 记录按钮和对应的音效
        buttonSounds[button] = defaultClickSound;
    }

    // 为按钮设置特定的音效
    public void SetButtonSound(Button button, AudioClip sound)
    {
        if (button == null || sound == null) return;

        // 确保按钮已经添加了音效监听器
        if (!buttonSounds.ContainsKey(button))
        {
            AddSoundToButton(button);
        }

        // 更新按钮的音效
        buttonSounds[button] = sound;
    }

    // 播放按钮音效
    private void PlayButtonSound(Button button)
    {
        if (buttonSounds.TryGetValue(button, out AudioClip sound))
        {
            if (sound != null)
            {
                audioSource.PlayOneShot(sound);
            }
        }
    }

    // 设置音量
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        audioSource.volume = volume;
    }
}

// 扩展方法，方便在其他脚本中使用
public static class ButtonSoundExtensions
{
    // 为按钮添加音效
    public static void AddSound(this Button button, AudioClip sound = null)
    {
        if (ButtonSoundManager.Instance != null)
        {
            if (sound != null)
            {
                ButtonSoundManager.Instance.SetButtonSound(button, sound);
            }
            else
            {
                ButtonSoundManager.Instance.AddSoundToButton(button);
            }
        }
    }
} 