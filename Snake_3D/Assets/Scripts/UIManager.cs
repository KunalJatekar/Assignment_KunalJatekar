using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text topScoreText;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject topScoreCanvas;

    static string SAVE_FILE_PATH;

    void Awake()
    {
        SAVE_FILE_PATH = Application.dataPath + "/Resources/";
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TopScore()
    {
        mainMenuCanvas.SetActive(false);

        if (loadData(SAVE_FILE_PATH + "TopScore.json") != null)
        {
            string highScoreJson = loadData(SAVE_FILE_PATH + "TopScore.json");
            TopScore topScoreDataTemp = JsonUtility.FromJson<TopScore>(highScoreJson);
            topScoreText.text = topScoreDataTemp.topScore.ToString();
        }
        else
        {
            topScoreText.text = "0";
        }
        topScoreCanvas.SetActive(true);
    }

    public void Back()
    {
        topScoreCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public string loadData(string filePath)
    {
        if (File.Exists(filePath))
        {
            string saveFileData = File.ReadAllText(filePath);
            return saveFileData;
        }
        else
        {
            return null;
        }
    }
}
