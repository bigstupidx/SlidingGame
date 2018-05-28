using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataLoader : MonoBehaviour {

    public static Data gameData;

    static string path;
    static string jsonData;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        path = Application.streamingAssetsPath + "/Data.json";

        if(File.Exists(path))
        {
            jsonData = File.ReadAllText(path);
            gameData = JsonUtility.FromJson<Data>(jsonData);
        }
        else
        {
            gameData = new Data();
            SaveData();
        }
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveData();
        }
    }


    public static void SaveData()
    {
        jsonData = JsonUtility.ToJson(gameData,true);
        File.WriteAllText(path, jsonData);
    }

    public static void LoadData()
    {
        jsonData = File.ReadAllText(path);
        Data fileData = JsonUtility.FromJson<Data>(jsonData);
        gameData = fileData;        
    }
}
