using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class Manager_Data
{
    public PlayerData PlayerData = new();
    public Dictionary<string, ItemData> ItemData = new();

    public void Initialize()
    {
        PlayerData = LoadData<PlayerData>("PlayerData"); // 씬에서 데이터 로드가 필요할 때 Initialize 사용
        ItemData = LoadDataToDictionary<ItemData>("ItemData");
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
    
    // json -> memory
    public Dictionary<string, T> LoadDataToDictionary<T>(string fileName, string path = Literals.JSON_PATH) where T : IKeyHolder
    {
        string fullPath = $"{path}{fileName}.json";

        if (!FindFile(fullPath))
            return default(Dictionary<string, T>);

        string jsonData = File.ReadAllText($"{path}{fileName}.json");
        List<T> loadData = JsonConvert.DeserializeObject<List<T>>(jsonData);

        Dictionary<string, T> dataDictionary = new Dictionary<string, T>();
        foreach (var item in loadData)
        {
            dataDictionary.Add(item.GetKey(), item);
        }

        return dataDictionary;
    }

    private bool FindFile(string path)
    {
        if (File.Exists(path))
            return true;

        Debug.Log($"File not found, Path : {path}");
        return false;
    }
}