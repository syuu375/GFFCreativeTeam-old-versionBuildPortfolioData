using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float ClearTime = float.MaxValue;
    public int ClearStage;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string fileName = "gamedata.json";
    private string fullPath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 重複を消す
        }
        fullPath = Path.Combine(Application.persistentDataPath, fileName);
    }

    public void Save(GameData newdata)
    {
        GameData data = Load();

        if(newdata.ClearTime < data.ClearTime)
        {
            string json = JsonUtility.ToJson(newdata, true);
            File.WriteAllText(fullPath, json);
            Debug.Log("Saved to: " + fullPath + " | ClearTime: " + newdata.ClearTime);
        }
       
    }

    public GameData Load()
    {
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            return new GameData(); // デフォルト値
        }
    }

    public void ResetData()
    {
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
