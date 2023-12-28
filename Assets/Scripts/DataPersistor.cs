using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataPersistor : MonoBehaviour
{
    public static DataPersistor Instance;
    public TMP_InputField inputField;
    public Text bestScoreText;
    public string playerName;
    public string highScoreName;
    public int highScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadScore();
        bestScoreText.text = $"Best Score : {highScoreName} : {highScore}";
    }

    public void StartGame()
    {
        playerName = inputField.text;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class HighScoreData
    {
        public string name;
        public int highScore;
    }

    public void SaveScore(string name, int score)
    {
        HighScoreData data = new HighScoreData();
        data.name = name;
        data.highScore = score;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    private void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);

            highScoreName = data.name;
            highScore = data.highScore;
        }
    }
}
