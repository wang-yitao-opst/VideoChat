using System;
using System.Collections.Generic;
using agora_gaming_rtc;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using VideoChat.Scripts;

namespace VideoChat
{
    public class InteractionHandler : MonoBehaviour
    {
        public static InteractionHandler Instance { get; private set; }

        [SerializeField] private LoginPanel _loginPanel;
        [SerializeField] private ChannelPanel _channelPanel;


        private uint _backgroundVideoUid;
        private List<GameObject> _userTiles = new List<GameObject>();
        private HashSet<uint> _users = new HashSet<uint>();
        private uint _numUsers;

        public AgoraInterface AgoraInterface { get; private set; } = null;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            AgoraInterface.Instance.OnInitialize += InstanceOnOnInitialize;
            
            _loginPanel._appId.SetText($"appId : {AgoraInterface.Instance._appID}");
        }
        
        private void InstanceOnOnInitialize()
        {
            AgoraInterface.Instance.Agora.OnUserJoined += OnUserJoined;
            AgoraInterface.Instance.Agora.OnUserOffline += OnUserOffline;
            AgoraInterface.Instance.Agora.OnJoinChannelSuccess += OnJoinChannelSuccess;
        }

        private void OnUserJoined(uint uid, int elapsed)
        {
            AgoraInterface.Instance.Agora.EnableVideoObserver();
            _channelPanel.SelfVideo.GetComponent<VideoSurface>().EnableFilpTextureApply(true, true);
            _channelPanel.BackgroundVideo.GetComponent<VideoSurface>().EnableFilpTextureApply(true, true);
        }

        private void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
        {
            // Find user and remove it from list
            var tileIndex = _userTiles.FindIndex((video) => video.GetComponent<VideoHandler>().Uid == uid);
            var tile = _userTiles[tileIndex];
            _userTiles.Remove(tile);
            _users.Remove(uid);
            DestroyImmediate(tile);

            var numChildren = _channelPanel.ScrollBar.gameObject.transform.childCount;
            if (numChildren > tileIndex)
            {
                for (int i = tileIndex; i < numChildren; i++)
                {
                    var rect = _userTiles[i].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector3(0, -i * (rect.rect.height + 20), 0);
                }
            }

            // Change background uid if needed
            if (_backgroundVideoUid == uid)
            {
                SwapBackgroundVideo(0);
            }
        }

        private void OnJoinChannelSuccess(string channelname, uint uid, int elapsed)
        {
            if (_users.Contains(uid))
                return;

            //生成需要的
            var video = Instantiate(_channelPanel.VideoPrefab);
            video.transform.SetParent(_channelPanel.ScrollBar.gameObject.transform, false);
            video.GetComponent<VideoHandler>().Uid = uid;
            _userTiles.Add(video);

            // Set video object inside scrollbar
            var numChildren = _channelPanel.ScrollBar.gameObject.transform.childCount;
            var rect = video.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector3(0, -(numChildren - 1) * (rect.rect.height + 20), 0);


            var vs = video.GetComponent<VideoSurface>();
            vs.SetForUser(uid);
            vs.SetEnable(true);
            vs.EnableFilpTextureApply(true, true);

            _numUsers++;
            _users.Add(uid);
        }

        public void OnJoinButtonClicked()
        {
            Debug.Log("Join Clicked");
            _loginPanel.gameObject.SetActive(false);
            _channelPanel.gameObject.SetActive(true);

            AgoraInterface.Instance.JoinChannel(_loginPanel.InputField.text,
                _loginPanel._enablevoice,_loginPanel._enablecamera);
        }

        public void OnMuteButtonClicked()
        {
            Debug.Log("Mute Clicked");

            AgoraInterface.Instance.Mute(_channelPanel._mute);
        }

        public void OnLeaveButtonClicked()
        {
            Debug.Log("Leave Clicked");
            _loginPanel.gameObject.SetActive(true);
            _channelPanel.gameObject.SetActive(false);

            AgoraInterface.Instance.LeaveChannel();
        }

        public void SwapBackgroundVideo(uint uid)
        {
            var videoSurface = _channelPanel.BackgroundVideo.GetComponent<VideoSurface>();

            if (videoSurface != null)
            {
                videoSurface.SetForUser(uid);
                _backgroundVideoUid = uid;
            }
            else
            {
                Debug.Log("no videoSurface attach");
            }
        }

        private bool isRemoteVidPanelOpen = true;
        
        public void RemoteVidPanelSwitchClicked()
        {
            isRemoteVidPanelOpen = !isRemoteVidPanelOpen;
            if (isRemoteVidPanelOpen)
            {
                OpenPanel();
            }
            else
            {
                ClosePanel();
            }
     
        }

        public void OpenPanel()
        {
            Vector3 targetPos = _channelPanel.RemoteVidPanel.transform.localPosition + Vector3.right * 300;
            _channelPanel.RemoteVidPanel.transform.DOLocalMoveX(targetPos.x, 2);
            
            _channelPanel.RemoteVidPanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            _channelPanel.RemoteVidPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        
        public void ClosePanel()
        {
            Vector3 targetPos = _channelPanel.RemoteVidPanel.transform.localPosition - Vector3.right * 300;
            _channelPanel.RemoteVidPanel.transform.DOLocalMoveX(targetPos.x, 2);
            
            _channelPanel.RemoteVidPanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            _channelPanel.RemoteVidPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }


        private void OnApplicationQuit()
        {
            AgoraInterface.Instance.UnloadEngine();
        }
    }

}
