using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class JsonDataLoader : MonoBehaviour
{
    public static JsonDataLoader instance;

    public AppData appData = new AppData();
    private string filrPath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            filrPath = Path.Combine(Application.persistentDataPath, "appData.json");
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // loads data from json file
    public void LoadData()
    {
        if (File.Exists(filrPath))
        {
            string json = File.ReadAllText(filrPath);
            appData = JsonUtility.FromJson<AppData>(json);

            if (appData.game == null)
            {
                appData.game = new List<GameData>();
            }

            if (appData.mias == null)
            {
                appData.mias = new List<MiasData>();
            }

            if (appData.game.Count == 0)
            {
                appData.game.Add(new GameData { flowerCount = 0 });
            }

            if (appData.mias.Count == 0)
            {
                appData.mias.Add(new MiasData { reactionTime = 0 });
            }
        }
        else
        {
            appData = new AppData();

            appData.users.Add(new UserData { username = "Namie", password = "123" });

            /*appData.game.Add(new GameData { flowerCount = 0});

            appData.mias.Add(new MiasData { reactionTime = 0, bugExperiments = 0, bugTrapped = 0, bugEscape = 0});*/
            if (appData.game.Count == 0) appData.game.Add(new GameData { flowerCount = 0 });
            if (appData.mias.Count == 0)
                appData.mias.Add(new MiasData { reactionTime = 0, bugExperiments = 0, bugTrapped = 0, bugEscape = 0 });

            SaveData();
        }
    }
    // save data to json file
    public void SaveData()
    {
        string json = JsonUtility.ToJson(appData, true);
        File.WriteAllText(filrPath, json);
    }
   //checks the login data
    public bool ValidateLogin(string username, string password)
    {
        foreach (var user in appData.users)
        {
            if (user.username == username && user.password == password)
            {
                return true;
            }
        }

        return false;
    }
}