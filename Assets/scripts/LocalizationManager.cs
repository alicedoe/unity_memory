using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour {

	public static LocalizationManager instance;
	private bool isReady = false;
	private Dictionary<string, string> localizedText;
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if ( instance != this ) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}
	
	public void LoadLocalizedText (string fileName) {
		localizedText = new Dictionary<string,string> ();

		// string filePath =Path.Combine(Application.streamingAssetsPath, fileName);
		string filePath = Path.Combine(Application.streamingAssetsPath + "/", fileName);
		Debug.LogError(filePath);
		if (File.Exists("jar:file:///data/app/com.defaultcompany.memory-1/base.apk/assets/localizedText_fr.json")) {
			// string dataAsJson = File.ReadAllText(filePath);

			#if UNITY_EDITOR || UNITY_IOS
			string dataAsJson = File.ReadAllText(filePath);

			#elif UNITY_ANDROID
			WWW reader = new WWW (filePath);
			while (!reader.isDone) {
			}
			string dataAsJson = reader.text;
			#endif

			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

			for (int i = 0; i < loadedData.items.Length; i++ ) {
				localizedText.Add(loadedData.items[i].key, loadedData.items [i].value);
			}
		}
		else {
			Debug.LogError("Cannot find file ! : " + filePath);
		}

		isReady = true;
	}

	public string GetLocalizedValue(string key) {
		string result = key;
		if (localizedText.ContainsKey(key)) {
			result = localizedText[key];
		}
		return result;
	}

	public bool GetIsReady() {
		return isReady;
	}
}
