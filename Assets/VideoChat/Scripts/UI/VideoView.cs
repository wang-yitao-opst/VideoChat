using agora_gaming_rtc;
using TMPro;
using UnityEngine;

namespace VideoChat
{
    public class VideoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private uint _uid;
        private void Awake()
        {
            if (_text == null)
            {
                _text = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        public void SetUpVideoView(uint uid)
        {
            VideoSurface videoSurface = GetComponent<VideoSurface>();
            if (!ReferenceEquals(videoSurface, null))
            {
                // configure videoSurface
                videoSurface.SetForUser(uid);
                videoSurface.SetEnable(true);
                videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            }

            _uid = uid;
            _text.SetText($"UID: {_uid}");
        }
    }
}