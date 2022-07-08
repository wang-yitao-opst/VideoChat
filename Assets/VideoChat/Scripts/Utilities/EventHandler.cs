using System;
using UnityEngine;

namespace VideoChat
{
    public static class EventHandler
    {
        public static event Action<string> TransitionEvent;

        public static void CallTransitionEvent(string sceneToGo)
        {
            TransitionEvent?.Invoke(sceneToGo);
        }
        
        public static event Action BeforeSceneUnloadEvent;
        public static void CallBeforeSceneUnloadEvent()
        {
            BeforeSceneUnloadEvent?.Invoke();
        }

        public static event Action AfterSceneLoadedEvent;
        public static void CallAfterSceneLoadedEvent()
        {
            AfterSceneLoadedEvent?.Invoke();
        }

        public static event Action<uint> OnCreateVideoViewEvent;

        public static void CallOnCreateVideoViewEvent(uint uid)
        {
            OnCreateVideoViewEvent?.Invoke(uid);
        }
        
        public static event Action<uint> OnDestroyVideoViewEvent;

        public static void CallOnDestroyVideoViewEvent(uint uid)
        {
            OnDestroyVideoViewEvent?.Invoke(uid);
        }
        
    }
}