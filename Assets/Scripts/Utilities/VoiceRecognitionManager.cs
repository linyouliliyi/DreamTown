using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography;

public class VoiceRecognitionManager : MonoBehaviour
{
    private static VoiceRecognitionManager instance;
    public static VoiceRecognitionManager Instance => instance;

    // 百度语音API配置
    private const string API_KEY = "YOUR_API_KEY";
    private const string SECRET_KEY = "YOUR_SECRET_KEY";
    private const string TOKEN_URL = "https://aip.baidubce.com/oauth/2.0/token";
    private const string ASR_URL = "https://vop.baidu.com/server_api";

    private string accessToken;
    private bool isRecording = false;
    private AudioClip recordingClip;
    private const int RECORDING_FREQUENCY = 16000;
    private const int RECORDING_LENGTH = 60; // 最大录音时长（秒）

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        StartCoroutine(GetAccessToken());
    }

    private IEnumerator GetAccessToken()
    {
        string url = $"{TOKEN_URL}?grant_type=client_credentials&client_id={API_KEY}&client_secret={SECRET_KEY}";
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                var response = JsonUtility.FromJson<TokenResponse>(www.text);
                accessToken = response.access_token;
            }
            else
            {
                Debug.LogError("Failed to get access token: " + www.error);
            }
        }
    }

    // 开始录音（暂时禁用）
    public void StartRecording()
    {
        Debug.LogWarning("语音识别功能暂时禁用");
        // isRecording = true;
        // recordingClip = Microphone.Start(null, false, RECORDING_LENGTH, RECORDING_FREQUENCY);
        // StartCoroutine(RecordingTimer());
    }

    // 停止录音（暂时禁用）
    public void StopRecording()
    {
        Debug.LogWarning("语音识别功能暂时禁用");
        // if (isRecording)
        // {
        //     isRecording = false;
        //     Microphone.End(null);
        //     StartCoroutine(ProcessRecording());
        // }
    }

    // 获取语音识别状态
    public bool IsRecording()
    {
        return isRecording;
    }

    // 模拟语音识别结果（用于测试）
    public string GetTestVoiceInput()
    {
        return "这是一个测试语音输入";
    }

    private IEnumerator RecordingTimer()
    {
        yield return new WaitForSeconds(RECORDING_LENGTH);
        if (isRecording)
        {
            StopRecording();
        }
    }

    private IEnumerator ProcessRecording()
    {
        // 将录音转换为WAV格式
        byte[] wavData = ConvertToWav(recordingClip);
        
        // 调用百度语音识别API
        string url = $"{ASR_URL}?dev_pid=1537&cuid=unity_client&token={accessToken}";
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "Content-Type", "audio/wav;rate=16000" }
        };

        using (WWW www = new WWW(url, wavData, headers))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                var response = JsonUtility.FromJson<AsrResponse>(www.text);
                if (response.err_no == 0)
                {
                    // 处理识别结果
                    ProcessRecognitionResult(response.result[0]);
                }
                else
                {
                    Debug.LogError("Speech recognition failed: " + response.err_msg);
                }
            }
            else
            {
                Debug.LogError("Failed to send audio data: " + www.error);
            }
        }
    }

    private void ProcessRecognitionResult(string result)
    {
        // 这里可以添加对识别结果的处理逻辑
        // 例如：分析角色信息、更新角色数据等
        Debug.Log("Recognition result: " + result);
    }

    private byte[] ConvertToWav(AudioClip clip)
    {
        // 将AudioClip转换为WAV格式的字节数组
        // 这里需要实现WAV格式转换的具体逻辑
        return new byte[0];
    }

    [System.Serializable]
    private class TokenResponse
    {
        public string access_token;
        public string expires_in;
        public string error;
        public string error_description;
    }

    [System.Serializable]
    private class AsrResponse
    {
        public int err_no;
        public string err_msg;
        public string[] result;
    }
} 