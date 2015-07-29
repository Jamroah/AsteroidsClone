using UnityEngine;
using System.IO;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public static class CustomAssetUtility
{
    public static void CreateAsset<t>() where t : ScriptableObject
    {
#if UNITY_EDITOR
        t asset = ScriptableObject.CreateInstance<t>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(t).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
#endif
    }
}