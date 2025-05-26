using System.IO;
using UnityEditor;
using UnityEngine;

public class FolderSettings : Editor
{
    public static string AssetsPath = Application.dataPath;
   
    [MenuItem("Tools/01. SettingTools/01. FolderSettings")]
    public static void Folder_Settings()
    {
        
#if DEBUG_MODE
        Debug.Log($"<color=lime>폴더 세팅을 시작합니다.</color>");
#endif
        string ResourcesPath = Path.Combine(AssetsPath, "Resources");
        string ScriptsPath = Path.Combine(AssetsPath, "Scripts");
        string ScenesPath = Path.Combine(AssetsPath, "Scenes");
        ////////////////////////////////////////
        /////////////건드리지 마시오/////////////
        string[] ScriptsSubfolders = { "Managers", "Mains", "Datas", "Infos", "Behaviours" };
        string[] ResourcesSubfolders = { "LocalizationSettings", "Atlas","SampleImages","LocalizationData" };
        string[] ScenesSubfolders = { "MainScene", "TestScene"};
        ////////////////////////////////////////
        ////////////////////////////////////////

        if (!Directory.Exists(ScriptsPath))
        {
#if DEBUG_MODE
            Debug.Log("Scripts 폴더가 존재하지 않습니다.");
            Debug.Log("Scripts 폴더를 생성합니다.");
#endif
            Directory.CreateDirectory(ScriptsPath);
        }
        else
        {
#if DEBUG_MODE
            Debug.Log("Scripts 폴더가 존재합니다.");
#endif
        }
#if DEBUG_MODE
        Debug.Log($"<color=blue>Scripts 하위 폴더를 생성을 시작합니다.</color>");
#endif
        for (int i =0; i < ScriptsSubfolders.Length; i++)
        {
            MakingSubfolders("Scripts/", ScriptsSubfolders[i]);
        }
#if DEBUG_MODE
        Debug.Log($"<color=blue>Scripts 하위 폴더를 생성이 종료되었습니다.</color>");
#endif

        if (!Directory.Exists(ResourcesPath))
        {
#if DEBUG_MODE
            Debug.Log("Resources 폴더가 존재하지 않습니다.");
            Debug.Log("Resources 폴더를 생성합니다.");
#endif
            Directory.CreateDirectory(ResourcesPath);
        }
        else
        {
#if DEBUG_MODE
            Debug.Log("Resources 폴더가 존재합니다.");
#endif
        }

#if DEBUG_MODE
        Debug.Log($"<color=blue>Resources 하위 폴더를 생성을 시작합니다.</color>");
#endif
        for (int i = 0; i < ResourcesSubfolders.Length; i++)
        {
            MakingSubfolders("Resources", ResourcesSubfolders[i]);
        }
#if DEBUG_MODE
        Debug.Log($"<color=blue>Resources 하위 폴더를 생성이 종료되었습니다.</color>");
#endif

        if (!Directory.Exists(ScenesPath))
        {
#if DEBUG_MODE
            Debug.Log("Scenes 폴더가 존재하지 않습니다.");
            Debug.Log("Scenes 폴더를 생성합니다.");
#endif
            Directory.CreateDirectory(ScenesPath);
        }
        else
        {
#if DEBUG_MODE
            Debug.Log("Scenes 폴더가 존재합니다.");
#endif
        }

#if DEBUG_MODE
        Debug.Log($"<color=blue>Scenes 하위 폴더를 생성을 시작합니다.</color>");
#endif
        for (int i = 0; i < ScenesSubfolders.Length; i++)
        {
            MakingSubfolders("Scenes", ScenesSubfolders[i]);
        }
#if DEBUG_MODE
        Debug.Log($"<color=blue>Scenes 하위 폴더를 생성이 종료되었습니다.</color>");
#endif

        AssetDatabase.Refresh();
#if DEBUG_MODE
        Debug.Log($"<color=lime>폴더 세팅이 종료되었습니다.</color>");
#endif
    }
    public static void MakingSubfolders(string Folder, string pathName)
    {
        string SettingPath = Path.Combine(AssetsPath, Folder, pathName);
        if (!Directory.Exists(SettingPath))
        {
#if DEBUG_MODE
            Debug.Log($"{pathName} 폴더가 존재하지 않습니다.");
            Debug.Log($"{pathName} 폴더를 생성합니다.");
#endif
            Directory.CreateDirectory(SettingPath);
        }
        else
        {
#if DEBUG_MODE
            Debug.Log($"{pathName} 폴더가 존재합니다.");
#endif
        }
    }


}
