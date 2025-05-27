using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class JsonViewerWindow : EditorWindow
{
    private string jsonContent = "";
    private Vector2 scrollPosition;

    [MenuItem("Tools/02. TestingTools/JSON Viewer")]
    public static void ShowWindow()
    {
        var window = GetWindow<JsonViewerWindow>("JSON Viewer");
        window.minSize = new Vector2(400, 300);
    }

    private void OnGUI()
    {
        GUILayout.Label("Drag and Drop JSON File Here", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // �巡�� �� ��� ���� ����
        var dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag JSON File Here", EditorStyles.helpBox);

        // �巡�� �� ��� �̺�Ʈ ó��
        Event evt = Event.current;
        if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
        {
            if (!dropArea.Contains(evt.mousePosition))
                return;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                foreach (var draggedObject in DragAndDrop.objectReferences)
                {
                    string path = AssetDatabase.GetAssetPath(draggedObject);
                    if (Path.GetExtension(path).Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        LoadJsonFile(path);
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Please drag a valid JSON file.");
                    }
                }
            }
        }

        EditorGUILayout.Space();

        // JSON ������ ǥ�� ����
        if (!string.IsNullOrEmpty(jsonContent))
        {
            GUILayout.Label("JSON Content:", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.TextArea(jsonContent, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }
    }

    private void LoadJsonFile(string path)
    {
        try
        {
            jsonContent = File.ReadAllText(path);
            jsonContent = FormatJson(jsonContent);
            Debug.Log("JSON file loaded successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load JSON file: " + ex.Message);
        }
    }

    private string FormatJson(string json)
    {
        try
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
        catch
        {
            Debug.LogError("Invalid JSON format.");
            return json; // �߸��� JSON ������ ��� �״�� ��ȯ
        }
    }
}
