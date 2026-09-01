using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class JasonDataLoader : MonoBehaviour
{
    
    public static JasonDataLoader instance;
    
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

    public void LoadData()
    {
        if (File.Exists(filrPath))
        {
            string json = File.ReadAllText(filrPath);
            appData = JsonUtility.FromJson<AppData>(json);
        }
        else
        {
            appData = new AppData();
            
            appData.users.Add(new UserData { username = "Namie", password = "123" });
            SaveData();
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(appData, true);
        File.WriteAllText(filrPath, json);
    }

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
