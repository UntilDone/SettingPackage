using System.IO;
using UnityEditor;
using UnityEngine;

public class FindOpenSaveFile : EditorWindow
{
    private string jsonContent = ""; // JSON ���� ������ ������ ����
    private Vector2 scrollPosition;

    [MenuItem("Tools/02. TestingTools/Open Save Folder and View JSON")]
    public static void ShowWindow()
    {
        // ������ â ����
        GetWindow<FindOpenSaveFile>("Save File Viewer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Persistent Data Path", EditorStyles.boldLabel);

        if (GUILayout.Button("Open Save Folder"))
        {
            OpenPersistentDataPath();
        }

        if (GUILayout.Button("Load and Display JSON"))
        {
            ReadAndDisplayJson();
        }

        GUILayout.Label("JSON Content:", EditorStyles.boldLabel);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));
        GUILayout.TextArea(jsonContent, GUILayout.ExpandHeight(true));
        GUILayout.EndScrollView();
    }

    private void OpenPersistentDataPath()
    {
        // ���� ���� ��ġ ���� ����
        EditorUtility.RevealInFinder(Application.persistentDataPath+ "/ProjectWarp");
    }

    private void ReadAndDisplayJson()
    {
        // JSON ���� ���
        string filePath = Path.Combine(Application.persistentDataPath, "savefile.json");

        if (File.Exists(filePath))
        {
            // JSON ���� �б�
            jsonContent = File.ReadAllText(filePath);
            Debug.Log("JSON file loaded successfully.");
        }
        else
        {
            jsonContent = "Save file not found.";
            Debug.LogWarning("Save file not found at: " + filePath);
        }
    }
}
