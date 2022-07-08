using agora_gaming_rtc;
using UnityEngine;

namespace VideoChat.Scripts
{
    public class AgoraInterface: MonoBehaviour
    {
        [SerializeField]
        public string _appID = "your_appid";
        [SerializeField]
        private string _token = "your_token";
        public IRtcEngine Agora{
            get; private set;
        }
        
        public delegate void OnInitialized();
        public event OnInitialized OnInitialize;
        
        public static AgoraInterface Instance{
            get; private set;
        }
        
        private void Awake()
        {
            if(Instance == null)
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
            InitEngine(_appID);
   
            OnInitialize?.Invoke();
        }
        
        public void Mute(bool channelPanelMute)
        {
            if (channelPanelMute)
            {
                Agora.DisableAudio();
            }
            else
            {
                Agora.EnableAudio();
            }
        }
        
 

        private VideoView _viewTempInMemory = null;

        public void InitEngine(string appID)
    {
        Debug.Log("InitializeEngine");
        if (Agora != null)
        {
            Debug.Log("Engine exists. Please unload it first!");
            return;
        }

        Agora = IRtcEngine.GetEngine(_appID);
        Agora.EnableAudio();
        Agora.EnableVideo();
        Agora.EnableVideoObserver();

        //Enable Log
        Agora.SetLogFilter(LOG_FILTER.DEBUG |
                           LOG_FILTER.INFO |
                           LOG_FILTER.WARNING |
                           LOG_FILTER.ERROR |
                           LOG_FILTER.CRITICAL);
        Agora.SetLogFile("log.txt");

        // //SetCallBack
        // Agora.OnJoinChannelSuccess += OnJoinChannelSuccess;
        // Agora.OnUserJoined += OnUserJoined;
        //
        // Agora.OnWarning += OnWarningHandler;
        // Agora.OnError += OnError;
        //
        // Agora.OnLeaveChannel += OnLeaveChannelHandler;
        // Agora.OnConnectionLost += OnConnectionLostHandler;
        // Agora.OnUserOffline += OnUserOffline;
    }

        public string GetSdkVersion()
        {
            string ver = IRtcEngine.GetSdkVersion();
            return ver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="token"></param>
        /// <param name="enableAudio"></param>
        /// <param name="enableCamera"></param>
        public void JoinChannel(string channelName, bool enableAudio, bool enableCamera)
        {
            if (Agora == null)
                return;
            Debug.Log($"calling join (channel = {channelName})");

            // if (enableAudio)
            // {
            //     Agora.EnableAudio();
            // }
            // else
            // {
            //     Agora.DisableAudio();
            // }
            //
            // if (enableCamera)
            // {
            //     Agora.EnableVideo();
            // }
            // else
            // {
            //     Agora.DisableVideo();
            // }
            //
            // // allow camera output callback
            // Agora.EnableVideoObserver();
            Agora.JoinChannelByKey( _token, channelName);
            // _rtcEngine.JoinChannel(channelName, null, 0);
        }

        public void JoinChannel(string channelName)
        {
            Debug.Log($"calling join (channel = {channelName})");
            Agora.JoinChannelByKey(_token,channelName, null, 0);
        }
        
        public void LeaveChannel()
        {
            Debug.Log("LeaveChannel");

            if (Agora == null)
                return;

            Agora.DisableAudio();
            Agora.DisableVideo();
            // leave channel
            Agora.LeaveChannel();
            // deregister video frame observers in native-c code
            Agora.DisableVideoObserver();
         
            IRtcEngine.Destroy();
            //Agora = null;
        }

        public void UnloadEngine()
        {
            Debug.Log("calling UnloadEngine");

            // delete
            if (Agora != null)
            {
                IRtcEngine.Destroy(); // Place this call in ApplicationQuit
                Agora = null;
            }
        }

    // public void CreateVideoView(Transform parent)
    // {
    //     VideoView videoViewGO = GameObject.Instantiate(_viewTempInMemory);
    //
    //     if (!ReferenceEquals(videoViewGO, null))
    //     {
    //         return; // reuse
    //     }
    //
    //     videoViewGO.name = Uid.ToString();
    //     videoViewGO.transform.SetParent(parent);
    //
    //     videoViewGO.SetUpVideoView(Uid);
    // }

    // private void DestroyVideoView(uint uid)
    // {
    //     GameObject go = GameObject.Find(uid.ToString());
    //     if (!ReferenceEquals(go, null))
    //     {
    //         Object.Destroy(go);
    //     }
    // }

    // private void OnJoinChannelSuccess(string channelname, uint uid, int elapsed)
    // {
    //     Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
    //     Uid = 0;
    //     EventHandler.CallOnCreateVideoViewEvent(0); //TODO 为什么用 0
    // }
    //
    // private void OnUserJoined(uint uid, int elapsed)
    // {
    //     Debug.Log($"On User Joined with uid: {uid} elapsed: {elapsed}");
    //     Uid = uid;
    //     // this is called in main thread
    //     EventHandler.CallOnCreateVideoViewEvent(uid);
    // }
    //
    // public uint Uid { get; set; }
    //
    // private void OnLeaveChannelHandler(RtcStats stats)
    // {
    //     Debug.Log($"Leave Channel Current user Count {stats.userCount}");
    //     DestroyVideoView(0); //TODO 为什么用 0
    // }
    //
    // private void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    // {
    //     Debug.Log($"OnUserOffLine uid: {uid}, reason: {reason}");
    //     DestroyVideoView(uid);
    // }
    //
    // private void OnConnectionLostHandler()
    // {
    //     Debug.Log("OnConnectionLost");
    // }

    // private void OnWarningHandler(int warn, string msg)
    // {
    //     Debug.LogWarning($"Warning Code {warn}\n msg {msg} ");
    // }

    // #region ErrorHandle
    //
    // private int _lastError { get; set; }
    //
    // private void OnError(int error, string msg)
    // {
    //     if (error == _lastError)
    //     {
    //         return;
    //     }
    //
    //     if (string.IsNullOrEmpty(msg))
    //     {
    //         msg = $"Error code:{error} msg:{IRtcEngine.GetErrorDescription(error)}";
    //     }
    //
    //     switch (error)
    //     {
    //         case 101:
    //             msg += "\nPlease make sure your AppId is valid and it does not require a certificate for this demo.";
    //             break;
    //     }
    //
    //     Debug.LogError(msg);
    //
    //     _lastError = error;
    // }
    //
    // #endregion


    }
}
