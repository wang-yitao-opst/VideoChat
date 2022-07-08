using System;
using System.Collections;
using System.Collections.Generic;
using agora_gaming_rtc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VideoChat;
using EventHandler = VideoChat.EventHandler;

public class LoginPanel : MonoBehaviour
{
    
    public TMP_InputField InputField;
    public Button _joinBtn;
    public RawImage _previewRawImage;
    public Button _previewBtn;
    public TextMeshProUGUI _previewText;
    public Button _cameraBtn;
    public TextMeshProUGUI _cameraText;
    public Button _voiceBtn;
    public TextMeshProUGUI _voiceText;
    public TextMeshProUGUI _appId;

    [Space] [SerializeField] [SceneName] private string _sceneToGo;
 
    public bool _enablePreviewImg = false;
    public bool _enablecamera = false;
    public bool _enablevoice = false;
    private void Start()
    {
        // _joinBtn.onClick.AddListener(OnJoinBtnClicked);
        
        UpdateCameraText();
        UpdateVoiceText();
        UpdatePreviewText();
        
        _previewBtn.onClick.AddListener(OnPreviewBtnClicked);
        _cameraBtn.onClick.AddListener(OnCameraBtnClicked);
        _voiceBtn.onClick.AddListener(OnVoiceBtnClicked);
    }
    
    private void OnVoiceBtnClicked()
    {
        _enablevoice = !_enablevoice;
        UpdateVoiceText();
    }
    
    private void UpdateVoiceText()
    {
        if (_enablevoice)
        {
            _voiceText.SetText("EnableVoice");
        }
        else
        {
            _voiceText.SetText("DisableVoice");
        }
    }
    
    private void OnCameraBtnClicked()
    {
        Debug.Log("OnCameraBtnClicked");
        _enablecamera = !_enablecamera;
        UpdateCameraText();
    }
    
    private void UpdateCameraText()
    {
        if (_enablecamera)
        {
            _cameraText.SetText("EnableCamera");
        }
        else
        {
            _cameraText.SetText("DisableCamera");
        }
    }
    private void OnPreviewBtnClicked()
    {
        _enablePreviewImg = !_enablePreviewImg;
        UpdatePreviewImage();
        UpdatePreviewText();
    }

    private void UpdatePreviewImage()
    {
    }

    private void UpdatePreviewText()
    {
        if (_enablePreviewImg)
        {
            _previewText.SetText("Preview On");
        }
        else
        {
            _previewText.SetText("Preview Off");
        }
    }
}
