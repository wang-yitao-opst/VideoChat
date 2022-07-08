using System;
using UnityEngine;

namespace AsFramework.Utilities
{
    public class CameraAdaptor : MonoBehaviour
    {
        private float _vaildWidth = 0f;
        private int _phoneWidth;
        private int _phoneHeight;
        private float _aspectRatio ;
        private float _defaultWidth = 1125;
        private float _defaultHeight = 2436;
        private float _defaultAspectRatio;
        private float _orthographicSize = 9.5f;

        private void Awake()
        {
            Adaptation();
        }
 
        private void Adaptation()
        {
            _phoneWidth = Screen.width;
            _phoneHeight = Screen.height;
            _defaultAspectRatio = _defaultWidth / _defaultHeight;
            _aspectRatio = _phoneWidth * 1f/ _phoneHeight;
            _vaildWidth = _orthographicSize * 2f * _defaultAspectRatio;
            float orthographicSize = _vaildWidth / _aspectRatio / 2f;
     
            GetComponent<Camera>().orthographicSize = orthographicSize;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Adaptation();
        }
#endif

    }
}
