using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class Manager_Data
{
    public TestData Test = new();
    
    public void Initialize()
    {
        Test = LoadData<TestData>("TestData"); // 씬에서 데이터 로드가 필요할 때 Initialize 사용
    }
    
    // memory -> json
    public void SaveData<T>(T obj, string fileName, string path = Literals.JSON_PATH)
    {
        string jsonSting = JsonConvert.SerializeObject(obj, Formatting.Indented);
        File.WriteAllText($"{path}{fileName}.json", jsonSting);
    }

    // json -> memory
    public T LoadData<T>(string fileName, string path = Literals.JSON_PATH)
    {
        string fullPath = $"{path}{fileName}.json";

        if (!FindFile(fullPath))
            return default(T);

        string jsonData = File.ReadAllText($"{path}{fileName}.json");
        T loadData = JsonConvert.DeserializeObject<T>(jsonData);
        return loadData;
    }

    private bool FindFile(string path)
    {
        if (File.Exists(path))
            return true;

        Debug.Log($"File not found, Path : {path}");
        return false;
    }
}