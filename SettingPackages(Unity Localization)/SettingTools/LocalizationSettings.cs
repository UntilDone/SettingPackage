using UnityEngine;
using UnityEditor;
using System.IO;
using ExcelDataReader;
using UnityEditor.Localization;
using UnityEditor.Localization.Editor;
using UnityEngine.Localization;
using System.Collections.Generic;
using System;
using UnityEngine.Localization.Tables;
using System.Globalization;
public class LocalizationSettings : Editor
{
    public static string localizationDataPath = Path.Combine(Application.dataPath, "Resources", "LocalizationData");
    public static string localizationSettingsPath = Path.Combine(Application.dataPath, "Resources", "LocalizationSettings");
    [MenuItem("Tools/01. SettingTools/02. LocalizationSettings")]
    public static void Localization_Settings()
    {
#if DEBUG_MODE
        Debug.Log($"<color=lime>번역 세팅을 시작합니다.</color>");
        Debug.Log($"<color=blue>번역 엑셀 파일에 엑세스 합니다.</color>");
#endif
        string excelFilePath = EditorUtility.OpenFilePanel("Select Excel File", localizationDataPath, "xlsx");
        if (string.IsNullOrEmpty(excelFilePath))
        {
#if DEBUG_MODE
            Debug.LogWarning($"<color=yellow>파일을 선택하지 않았습니다.</color>");
            Debug.Log($"<color=lime>번역 세팅을 종료합니다.</color>");
#endif
            return;
        }
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(excelFilePath);
        string csOutputPath = Path.Combine(localizationDataPath, $"{fileNameWithoutExtension}.cs");

        if (File.Exists(csOutputPath))
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "덮어쓰기 확인",
                "같은 위치에 이미 파일이 존재합니다. 덮어씌우겠습니까?",
                "예", "아니요"
            );

            if (!overwrite)
            {
#if DEBUG_MODE
                Debug.Log($"<color=yellow>덮어씌우기를 중지하였습니다.</color>");
                Debug.Log($"<color=lime>번역 세팅을 종료합니다.</color>");
#endif
                return;
            }
        }
        string[] headers = GenerateCS(excelFilePath, csOutputPath);
        ActivateLocales(headers);
        GenerateLocalizationTable(localizationSettingsPath, headers);
#if DEBUG_MODE
        Debug.Log($"<color=lime>번역 세팅을 종료합니다.</color>");
#endif
    }

    private static string[] GenerateCS(string excelFilePath, string outputPath)
    {
#if DEBUG_MODE
        Debug.Log($"<color=blue>번역 키 상수 파일 가공을 시작합니다.</color>");
#endif
        using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                using (var writer = new StreamWriter(outputPath))
                {
                    writer.WriteLine("public static class LocalizationConstants");
                    writer.WriteLine("{");

                    bool isHeaderRow = true;
                    string[] headers = null;

                    while (reader.Read())
                    {
                        if (isHeaderRow)
                        {
                            headers = new string[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                headers[i] = reader.GetValue(i)?.ToString();
                            }
                            isHeaderRow = false;
                            continue;
                        }

                        if (reader.GetValue(0) != null)
                        {
                            string id = reader.GetValue(0).ToString().ToUpperInvariant();
                            string value = reader.GetValue(0).ToString();

                            writer.WriteLine($"    public const string {id} = \"{value}\";");
                        }
                    }
                    writer.WriteLine("}");
#if DEBUG_MODE
                    Debug.Log($"<color=blue>번역 상수 파일 생성 완료</color>");
                    Debug.Log($"<color=blue>번역 키 상수 파일 가공을 종료합니다.</color>");
#endif
                    AssetDatabase.Refresh();
                    return headers;

                }
            }
        }


    }

    private static void GenerateLocalizationTable(string settingsPath, string[] headers)
    {
        string tableFolderPath = Path.Combine(settingsPath, "LocalizationTable");

        if (Directory.Exists(tableFolderPath))
        {
            Directory.Delete(tableFolderPath, true);
#if DEBUG_MODE
            Debug.Log($"<color=red>기존 LocalizationTable 폴더를 삭제했습니다: {tableFolderPath}</color>");
#endif
        }

        Directory.CreateDirectory(tableFolderPath);
#if DEBUG_MODE
        Debug.Log($"<color=green>새로운 LocalizationTable 폴더를 생성했습니다: {tableFolderPath}</color>");
#endif
        AssetDatabase.Refresh();
        string tableName = "LocalizationStringTable";
        var newTableCollection = LocalizationEditorSettings.CreateStringTableCollection(tableName, tableFolderPath);

        if (newTableCollection == null)
        {
#if DEBUG_MODE
            Debug.LogError($"<color=red>StringTable 생성 실패: {tableName}</color>");
#endif
            return;
        }

#if DEBUG_MODE
        Debug.Log($"<color=green>새로운 StringTable 생성: {tableName}</color>");
#endif

        using (var stream = File.Open(localizationDataPath + "/localization_data.xlsx", FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                bool isHeaderRow = true;
                while (reader.Read())
                {
                    if (isHeaderRow)
                    {
                        isHeaderRow = false;
                        continue;
                    }

                    string key = reader.GetValue(0)?.ToString();
                    if (string.IsNullOrEmpty(key)) continue;  // 빈 키 건너뛰기

                    for (int i = 1; i < reader.FieldCount; i++)
                    {
                        string languageTag = headers[i];
                        LocaleIdentifier localeIdentifier = GetLocaleIdentifier(languageTag).Identifier;

                        // StringTable 가져오기
                        StringTable stringTable = newTableCollection.GetTable(localeIdentifier) as StringTable;
                        if (stringTable == null)
                        {
#if DEBUG_MODE
                            Debug.LogError($"<color=red>{languageTag}({localeIdentifier}) 테이블을 찾을 수 없습니다.</color>");
#endif
                            continue;
                        }

                        string value = reader.GetValue(i)?.ToString();
                        //if (string.IsNullOrEmpty(value)) continue;  // 빈 값 건너뛰기

                        // 기존 엔트리 확인 후 추가 또는 수정
                        var entry = stringTable.SharedData.GetEntry(key);
                        if (entry == null)
                        {
                            // 새 항목 추가
                            stringTable.AddEntry(key, value);
#if DEBUG_MODE
                            Debug.Log($"<color=blue>New Entry 추가됨: {key} ({languageTag}) -> {value}</color>");
#endif
                        }
                        else
                        {
                            // 기존 항목 업데이트 (기존 값 제거 후 추가)
                            stringTable.RemoveEntry(key);
                            stringTable.AddEntry(key, value);
#if DEBUG_MODE
                            Debug.Log($"<color=yellow>기존 항목 업데이트됨: {key} ({languageTag}) -> {value}</color>");
#endif
                        }
                    }
                }
            }
        }

        // 변경 사항 저장 및 갱신
        EditorUtility.SetDirty(newTableCollection);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }



    private static void ActivateLocales(string[] headers)
    {
        var localizationSettings = LocalizationEditorSettings.ActiveLocalizationSettings;
        if (localizationSettings == null)
        {
            localizationSettings = ScriptableObject.CreateInstance<UnityEngine.Localization.Settings.LocalizationSettings>();
            string settingsFilePath = Path.Combine(localizationSettingsPath, "LocalizationSettings.asset");
            AssetDatabase.CreateAsset(localizationSettings, settingsFilePath.Replace(Application.dataPath, "Assets"));
            LocalizationEditorSettings.ActiveLocalizationSettings = localizationSettings;
        }

        string localeFolderPath = Path.Combine(localizationSettingsPath, "Locales");
        if (!Directory.Exists(localeFolderPath))
        {
            Directory.CreateDirectory(localeFolderPath);
        }

        foreach (string header in headers)
        {
            if (string.IsNullOrEmpty(header)) continue;

            string localeName = header.ToUpperInvariant();
            string localeFilePath = Path.Combine(localeFolderPath, $"{localeName}.asset").Replace(Application.dataPath, "Assets");

            Locale existingLocale = AssetDatabase.LoadAssetAtPath<Locale>(localeFilePath);
            if (existingLocale == null)
            {
                var newLocale = GetLocaleIdentifier(header);
                AssetDatabase.CreateAsset(newLocale, localeFilePath);
                localizationSettings.GetAvailableLocales().AddLocale(newLocale);
            }
            else
            {
                localizationSettings.GetAvailableLocales().AddLocale(existingLocale);
            }
        }

        EditorUtility.SetDirty(localizationSettings);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static Locale GetLocaleIdentifier(string languageTag)
    {
        Locale locale = ScriptableObject.CreateInstance<Locale>();
        switch (languageTag.ToUpper())
        {
            case "KR":
                locale.Identifier = new LocaleIdentifier(new CultureInfo("ko-KR"));
                break;
            case "EN":
                locale.Identifier = new LocaleIdentifier(new CultureInfo("en-US"));
                break;
            case "JP":
                locale.Identifier = new LocaleIdentifier(new CultureInfo("ja-JP"));
                break;
            case "FR":
                locale.Identifier = new LocaleIdentifier(new CultureInfo("fr-FR"));
                break;
            case "DE":
                locale.Identifier = new LocaleIdentifier(new CultureInfo("de-DE"));
                break;
            default:
                Debug.LogWarning($"Unknown language tag: {languageTag}, defaulting to EN.");
                locale.Identifier = new LocaleIdentifier(new CultureInfo("en-US"));
                break;
        }

        locale.name = languageTag; // Locale 이름을 KR, EN 등으로 설정
        return locale;
    }

}