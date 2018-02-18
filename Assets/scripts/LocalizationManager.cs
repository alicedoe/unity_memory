using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager instance;
    private bool isReady = false;
    private Dictionary<string, string> localizedText;
    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();

        string destinationPath = Path.Combine(Application.persistentDataPath, fileName);

        #if UNITY_EDITOR
                        string sourcePath = Path.Combine(Application.streamingAssetsPath, fileName);                
        #elif UNITY_ANDROID
                        string sourcePath = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
        #endif
        
        if (sourcePath.Contains("://"))
        {
            // Android  
            WWW www = new WWW(sourcePath);
            while (!www.isDone) {; }                // Wait for download to complete - not pretty at all but easy hack for now 
            if (string.IsNullOrEmpty(www.error))
            {
                File.WriteAllBytes(destinationPath, www.bytes);
                StreamReader reader = new StreamReader(destinationPath);
                var jsonString = reader.ReadToEnd();
                LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(jsonString);
                for (int i = 0; i < loadedData.items.Length; i++)
                {            
                    localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                }
                reader.Close();
            }
            else
            {
                Debug.Log("ERROR: the file DB named " + fileName + " doesn't exist in the StreamingAssets Folder, please copy it there.");
            }
        }
        else
        {
            Debug.Log("Mac, Windows, Iphone FILEPATH : " + sourcePath);
            // Mac, Windows, Iphone
            if (File.Exists(sourcePath))
            {
                string dataAsJson = File.ReadAllText(sourcePath);
                LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
                for (int i = 0; i < loadedData.items.Length; i++)
                {
                    Debug.Log(loadedData.items[i].key);
                    localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                }
            }
            else
            {
                Debug.LogError("Cannot find file ! : " + Application.streamingAssetsPath + fileName);
            }
        }

        isReady = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = key;
        
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }
        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }
}