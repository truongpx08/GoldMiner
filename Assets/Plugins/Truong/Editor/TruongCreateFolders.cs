using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TruongCreateFolders : MonoBehaviour
{
    [MenuItem("Truong/Create 2D Folders")]
    private static void Create2DFolders()
    {
        List<string> list = new List<string>
        {
            TruongConstant.SCENES,
            TruongConstant.SCRIPTS,
            TruongConstant.PREFABS,
            TruongConstant.RESOURCES,
            TruongConstant.PLUGINS,
            TruongConstant.SPRITES,
        };
        list.ForEach(item => { CreateAFolder(TruongConstant.ASSETS, item); });
    }

    private static void CreateAFolder(string parentFolder, string folderName)
    {
        string prefabFolderPath = Path.Combine(parentFolder, folderName);
        if (AssetDatabase.IsValidFolder(prefabFolderPath)) return;

        AssetDatabase.CreateFolder(parentFolder, folderName);
        Log(folderName);
    }

    private static void Log(string item)
    {
        Debug.Log("Has created folder " + item);
    }
}