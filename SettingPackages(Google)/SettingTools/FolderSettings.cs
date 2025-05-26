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
        Debug.Log($"<color=lime>���� ������ �����մϴ�.</color>");
#endif
        string ResourcesPath = Path.Combine(AssetsPath, "Resources");
        string ScriptsPath = Path.Combine(AssetsPath, "Scripts");
        string ScenesPath = Path.Combine(AssetsPath, "Scenes");
        ////////////////////////////////////////
        /////////////�ǵ帮�� ���ÿ�/////////////
        string[] ScriptsSubfolders = { "Managers", "Mains", "Datas", "Infos", "Behaviours" };
        string[] ResourcesSubfolders = { "LocalizationSettings", "Atlas","SampleImages","LocalizationData" };
        string[] ScenesSubfolders = { "MainScene", "TestScene"};
        ////////////////////////////////////////
        ////////////////////////////////////////

        if (!Directory.Exists(ScriptsPath))
        {
#if DEBUG_MODE
            Debug.Log("Scripts ������ �������� �ʽ��ϴ�.");
            Debug.Log("Scripts ������ �����մϴ�.");
#endif
            Directory.CreateDirectory(ScriptsPath);
        }
        else
        {
#if DEBUG_MODE
            Debug.Log("Scripts ������ �����մϴ�.");
#endif
        }
#if DEBUG_MODE
        Debug.Log($"<color=blue>Scripts ���� ������ ������ �����մϴ�.</color>");
#endif
        for (int i =0; i < ScriptsSubfolders.Length; i++)
        {
            MakingSubfolders("Scripts/", ScriptsSubfolders[i]);
        }
#if DEBUG_MODE
        Debug.Log($"<color=blue>Scripts ���� ������ ������ ����Ǿ����ϴ�.</color>");
#endif

        if (!Directory.Exists(ResourcesPath))
        {
#if DEBUG_MODE
            Debug.Log("Resources ������ �������� �ʽ��ϴ�.");
            Debug.Log("Resources ������ �����մϴ�.");
#endif
            Directory.CreateDirectory(ResourcesPath);
        }
        else
        {
#if DEBUG_MODE
            Debug.Log("Resources ������ �����մϴ�.");
#endif
        }

#if DEBUG_MODE
        Debug.Log($"<color=blue>Resources ���� ������ ������ �����մϴ�.</color>");
#endif
        for (int i = 0; i < ResourcesSubfolders.Length; i++)
        {
            MakingSubfolders("Resources", ResourcesSubfolders[i]);
        }
#if DEBUG_MODE
        Debug.Log($"<color=blue>Resources ���� ������ ������ ����Ǿ����ϴ�.</color>");
#endif

        if (!Directory.Exists(ScenesPath))
        {
#if DEBUG_MODE
            Debug.Log("Scenes ������ �������� �ʽ��ϴ�.");
            Debug.Log("Scenes ������ �����մϴ�.");
#endif
            Directory.CreateDirectory(ScenesPath);
        }
        else
        {
#if DEBUG_MODE
            Debug.Log("Scenes ������ �����մϴ�.");
#endif
        }

#if DEBUG_MODE
        Debug.Log($"<color=blue>Scenes ���� ������ ������ �����մϴ�.</color>");
#endif
        for (int i = 0; i < ScenesSubfolders.Length; i++)
        {
            MakingSubfolders("Scenes", ScenesSubfolders[i]);
        }
#if DEBUG_MODE
        Debug.Log($"<color=blue>Scenes ���� ������ ������ ����Ǿ����ϴ�.</color>");
#endif

        AssetDatabase.Refresh();
#if DEBUG_MODE
        Debug.Log($"<color=lime>���� ������ ����Ǿ����ϴ�.</color>");
#endif
    }
    public static void MakingSubfolders(string Folder, string pathName)
    {
        string SettingPath = Path.Combine(AssetsPath, Folder, pathName);
        if (!Directory.Exists(SettingPath))
        {
#if DEBUG_MODE
            Debug.Log($"{pathName} ������ �������� �ʽ��ϴ�.");
            Debug.Log($"{pathName} ������ �����մϴ�.");
#endif
            Directory.CreateDirectory(SettingPath);
        }
        else
        {
#if DEBUG_MODE
            Debug.Log($"{pathName} ������ �����մϴ�.");
#endif
        }
    }


}
