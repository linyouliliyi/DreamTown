using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("背景音乐")]
    public AudioClip[] backgroundMusics;  // 不同场景的背景音乐
    public float fadeDuration = 1f;       // 淡入淡出时间
    public float volume = 0.5f;           // 音量大小

    private AudioSource audioSource;
    private int currentMusicIndex = -1;

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
        audioSource.loop = true;
        audioSource.volume = 0f; // 初始音量为0，等待淡入
    }

    // 播放指定索引的背景音乐
    public void PlayBackgroundMusic(int index)
    {
        if (index < 0 || index >= backgroundMusics.Length)
        {
            Debug.LogError("Invalid music index!");
            return;
        }

        if (currentMusicIndex == index)
        {
            return; // 已经在播放相同的音乐
        }

        currentMusicIndex = index;
        StartCoroutine(FadeInMusic(backgroundMusics[index]));
    }

    // 停止当前背景音乐
    public void StopBackgroundMusic()
    {
        if (audioSource.isPlaying)
        {
            StartCoroutine(FadeOutMusic());
        }
    }

    // 暂停背景音乐
    public void PauseBackgroundMusic()
    {
        audioSource.Pause();
    }

    // 继续播放背景音乐
    public void ResumeBackgroundMusic()
    {
        audioSource.UnPause();
    }

    // 设置音量
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        audioSource.volume = volume;
    }

    // 淡入音乐
    private IEnumerator FadeInMusic(AudioClip clip)
    {
        // 如果正在播放其他音乐，先淡出
        if (audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeOutMusic());
        }

        audioSource.clip = clip;
        audioSource.Play();

        float startTime = Time.time;
        while (Time.time - startTime < fadeDuration)
        {
            float progress = (Time.time - startTime) / fadeDuration;
            audioSource.volume = Mathf.Lerp(0f, volume, progress);
            yield return null;
        }

        audioSource.volume = volume;
    }

    // 淡出音乐
    private IEnumerator FadeOutMusic()
    {
        float startVolume = audioSource.volume;
        float startTime = Time.time;

        while (Time.time - startTime < fadeDuration)
        {
            float progress = (Time.time - startTime) / fadeDuration;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, progress);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0f;
    }
} 