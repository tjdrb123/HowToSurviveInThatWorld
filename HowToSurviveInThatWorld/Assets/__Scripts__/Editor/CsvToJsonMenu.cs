using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class CsvToJsonMenu
{
    [MenuItem("Data Tools/CsvToJson")]
    static void ConvertCsvToJson()
    {
        CsvToJson("TestData");
    }
    
    static void CsvToJson(string fileName, string path = Literals.CSV_PATH)
    {
        string fullPath = $"{path}{fileName}.csv";

        if (!FindFile(fullPath))
            return;

        string[] csvLines = File.ReadAllLines(fullPath);

        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        string[] headers = csvLines[0].Split(',');
        for (int i = 1; i < csvLines.Length; i++)
        {
            string[] values = csvLines[i].Split(',');
            Dictionary<string, object> dataEntry = new Dictionary<string, object>();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                dataEntry[headers[j]] = values[j];
            }

            dataList.Add(dataEntry);
        }

        string jsonSting = JsonConvert.SerializeObject(dataList, Formatting.Indented);
        File.WriteAllText($"{Literals.JSON_PATH}{fileName}.json", jsonSting);
    }
    
    private static bool FindFile(string path)
    {
        if (File.Exists(path))
            return true;

        Debug.Log($"File not found, Path : {path}");
        return false;
    }
}
