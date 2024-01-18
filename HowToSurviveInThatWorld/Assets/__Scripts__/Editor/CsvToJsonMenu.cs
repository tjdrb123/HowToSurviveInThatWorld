using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class CsvToJsonMenu
{
    // 유니티에서 바로 실행 가능합니다.
    [MenuItem("Data Tools/CsvToJson")]
    static void ConvertCsvToJson()
    {
        CsvToJson("PlayerData");
    }
    
    /*
    일반적으로 쓰는 csv파일 형식은
    첫 번째 헤더라인에,
    Key Value값만 사용하면 됩니다.
    
    만약, 중첩된 구조(같은 hp에서도 curValue, maxValue가 있듯이)를 필요로 한다면
    첫 번째 헤더라인에,
    Key SubKey Value값을 만들어 주세요.
    */
    static void CsvToJson(string fileName, string path = Literals.CSV_PATH)
    {
        string fullPath = $"{path}{fileName}.csv";

        if (!FindFile(fullPath))
            return;

        string[] csvLines = File.ReadAllLines(fullPath);

        string[] headers = csvLines[0].Split(',');
        
        // 중첩된 데이터를 가지고 있는지 체크
        bool isNested = headers.Contains("SubKey");

        // 일반적인 JSON 객체를 생성하기 위한 데이터 구조
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
        
        // 중첩된 JSON 객체를 생성하기 위한 데이터 구조
        Dictionary<string, Dictionary<string, object>> dataDict = new Dictionary<string, Dictionary<string, object>>();

        for (int i = 1; i < csvLines.Length; i++)
        {
            string[] values = csvLines[i].Split(',');

            if (isNested)
            {
                string key = values[0];
                string subKey = values[1];
                object value = values[2];

                if (!dataDict.ContainsKey(key))
                {
                    dataDict[key] = new Dictionary<string, object>();
                }

                dataDict[key][subKey] = value;
            }
            else
            {
                Dictionary<string, object> dataEntry = new Dictionary<string, object>();
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    dataEntry[headers[j]] = values[j];
                }
                dataList.Add(dataEntry);
            }
        }

        string jsonSting = isNested ? JsonConvert.SerializeObject(dataDict, Formatting.Indented) : JsonConvert.SerializeObject(dataList, Formatting.Indented);
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
