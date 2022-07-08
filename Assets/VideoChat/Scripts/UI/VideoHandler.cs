using UnityEngine;
using UnityEngine.EventSystems;

namespace VideoChat
{
    public class VideoHandler : MonoBehaviour, IPointerClickHandler
    {
        public uint Uid
        {
            get => uid;
            set => uid = value;
        }

        private uint uid;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            InteractionHandler.Instance.SwapBackgroundVideo(uid);
        }
    }
}