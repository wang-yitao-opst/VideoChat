using System;
using agora_gaming_rtc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VideoChat.Scripts;

namespace VideoChat
{
    public class ChannelPanel : MonoBehaviour
    {
        [SerializeField] private Button _leaveBtn;
        [SerializeField] private Button _muteBtn;

        public GameObject BackgroundVideo;
        public GameObject SelfVideo;
        
        public GameObject ScrollBar; 
        public GameObject VideoPrefab;
        public GameObject RemoteVidPanel;

        public bool _mute = false;
        
        [SerializeField] private TextMeshProUGUI _sdkVersionText;
        private void Start()
        {
            _leaveBtn.onClick.AddListener(OnLeaveBtnClicked);
            //_sdkVersionText.SetText($"SDK Version {IRtcEngine.GetSdkVersion()}");
            
            _muteBtn.onClick.AddListener(OnMuteBtnClicked);
            OnLeaveBtnClicked();
            
            _sdkVersionText.SetText($"SDK Version : {AgoraInterface.Instance.GetSdkVersion()}");
        }

        private void OnMuteBtnClicked()
        {
            _mute = !_mute;
        }

        private void OnLeaveBtnClicked()
        {
        }
    }
}