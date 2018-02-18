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
        string jsonString = "" ;

        string destinationPath = Path.Combine(Application.persistentDataPath, fileName);

        #if UNITY_EDITOR
                        string sourcePath = Path.Combine(Application.streamingAssetsPath, fileName);                
        #elif UNITY_ANDROID
                        string sourcePath = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
        #endif
        
        if (sourcePath.Contains("://"))
        {
            WWW www = new WWW(sourcePath);
            while (!www.isDone) {; } 
            if (string.IsNullOrEmpty(www.error))
            {
                File.WriteAllBytes(destinationPath, www.bytes);
                StreamReader reader = new StreamReader(destinationPath);
                jsonString = reader.ReadToEnd();
                reader.Close();
            }
            else
            {
                Debug.LogError("Cannot find file ! : " + fileName);
            }
        }
        else
        {
            if (File.Exists(sourcePath))
            {
                jsonString = File.ReadAllText(sourcePath);
            }
            else
            {
                Debug.LogError("Cannot find file ! : " + fileName);
            }
        }

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(jsonString);
        for (int i = 0; i < loadedData.items.Length; i++)
        {            
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
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