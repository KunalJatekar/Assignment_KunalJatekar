using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    static string SAVE_FILE_PATH;

    [SerializeField] GameObject endMenu;
    [SerializeField] Text endMenuScoreText; 

    PlayerMovement player;

    [HideInInspector] public FoodList foods;
    [HideInInspector] public int playerScore;

    public static GameManager instance;
    public Text scoreText;
    public Text streakText;
    public Image streakImage;
    public GameObject streakPanel;

    List<Color> m_colorStreek;

    public List<Color> colorStreek
    {
        get
        {
            return m_colorStreek;
        }
    }

    void Awake()
    {
        Time.timeScale = 1;
        CreateInstance();

        m_colorStreek = new List<Color>();

        SAVE_FILE_PATH = Application.dataPath + "/Resources/";
        string foodJson = loadData(SAVE_FILE_PATH + "Food.json");
        foods = JsonUtility.FromJson<FoodList>(foodJson);
        Debug.Log("Foods[0].color : " + foods.Foods[0].color + "Foods[1].color : " + foods.Foods[1].color);

        playerScore = 0;
    }

    void CreateInstance()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        player.OnCollision += EndGame;
        endMenu.SetActive(false);
        endMenuScoreText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EndGame()
    {
        Time.timeScale = 0;
        saveHighScore();
        endMenu.SetActive(true);
        endMenuScoreText.text = "" + playerScore;
        Debug.Log("End Game");
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

    public void saveData(string jsonDataToSave, string fileName)
    {
        try
        {
            Debug.Log("In SaveSystem path : " + fileName);
            File.WriteAllText(fileName, jsonDataToSave);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.GetBaseException());
        }
    }

    public void SetColorStreak(Color color)
    {
        m_colorStreek.Add(color);
    }

    public void saveHighScore()
    {
        TopScore topScore = new TopScore();

        if (loadData(SAVE_FILE_PATH + "TopScore.json") == null)
        {
            topScore.topScore = playerScore;
            string finalJson = JsonUtility.ToJson(topScore, true);
            Debug.Log("Inside GameManager finalJson : " + finalJson);
            saveData(finalJson, SAVE_FILE_PATH + "TopScore.json");
        }
        else
        {
            string highScoreJson = loadData(SAVE_FILE_PATH + "TopScore.json");
            TopScore topScoreDataTemp = JsonUtility.FromJson<TopScore>(highScoreJson);

            if(playerScore > topScoreDataTemp.topScore)
            {
                topScore.topScore = playerScore;
                string finalJson = JsonUtility.ToJson(topScore, true);
                Debug.Log("Inside GameManager finalJson : " + finalJson);
                saveData(finalJson, SAVE_FILE_PATH + "TopScore.json");
            }
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
