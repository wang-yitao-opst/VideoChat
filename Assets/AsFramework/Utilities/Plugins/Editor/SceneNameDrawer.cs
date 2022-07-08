using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    int _sceneIndex = -1;
    GUIContent[] _sceneNameConTentArray;

    readonly string[] _scenePathSplit = { "/", ".unity" };
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (EditorBuildSettings.scenes.Length == 0) return;

        if (_sceneIndex == -1)
            GetSceneNameArray(property);

        int oldIndex = _sceneIndex;

        _sceneIndex = EditorGUI.Popup(position, label, _sceneIndex, _sceneNameConTentArray);

        if (oldIndex != _sceneIndex)
            property.stringValue = _sceneNameConTentArray[_sceneIndex].text;
    }

    private void GetSceneNameArray(SerializedProperty property)
    {
        var scenes = EditorBuildSettings.scenes;
        //初始化数组
        _sceneNameConTentArray = new GUIContent[scenes.Length];

        for (int i = 0; i < _sceneNameConTentArray.Length; i++)
        {
            string path = scenes[i].path;
            string[] splitPath = path.Split(_scenePathSplit, System.StringSplitOptions.RemoveEmptyEntries);

            string sceneName = "";

            if (splitPath.Length > 0)
            {
                sceneName = splitPath[splitPath.Length - 1];
            }
            else
            {
                sceneName = "(Deleted Scene)";
            }
            _sceneNameConTentArray[i] = new GUIContent(sceneName);
        }

        if (_sceneNameConTentArray.Length == 0)
        {
            _sceneNameConTentArray = new[] { new GUIContent("Check Your Build Settings") };
        }

        if (!string.IsNullOrEmpty(property.stringValue))
        {
            bool nameFound = false;

            for (int i = 0; i < _sceneNameConTentArray.Length; i++)
            {
                if (_sceneNameConTentArray[i].text == property.stringValue)
                {
                    _sceneIndex = i;
                    nameFound = true;
                    break;
                }
            }
            if (nameFound == false)
                _sceneIndex = 0;
        }
        else
        {
            _sceneIndex = 0;
        }

        property.stringValue = _sceneNameConTentArray[_sceneIndex].text;
    }
}