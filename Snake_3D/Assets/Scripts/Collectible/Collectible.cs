using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10;
    
    float delay = 1f;

    Spawner spawner;

    int m_points;

    public int points
    {
        get { return m_points; }
        set { m_points = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        spawner = GetComponentInParent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0, 1f, 0), Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Color color = transform.GetComponent<MeshRenderer>().material.color;

            if (GameManager.instance.colorStreek.Count != 0)
            {
                CheckStreak(GameManager.instance.colorStreek, color);
            }
            else
            {
                GameManager.instance.playerScore += points;
                GameManager.instance.SetColorStreak(color);
                GameManager.instance.scoreText.text = GameManager.instance.playerScore.ToString();
                GameManager.instance.streakText.text = GameManager.instance.colorStreek.Count.ToString();
                GameManager.instance.streakImage.color = color;
                GameManager.instance.streakPanel.SetActive(true);
            }
            
            other.gameObject.GetComponentInParent<PlayerController>().AddBodyPart();
            gameObject.SetActive(false);

            spawner.Invoke("spawnable", delay);
        }
    }

    void CheckStreak(List<Color> streakList, Color color)
    {
        bool streakFlag = true;

        foreach(Color tempColor in streakList)
        {
            if (!tempColor.Equals(color))
                streakFlag = false;
        }

        if (streakFlag)
        {
            GameManager.instance.SetColorStreak(color);
            GameManager.instance.playerScore += points * GameManager.instance.colorStreek.Count;
            GameManager.instance.scoreText.text = GameManager.instance.playerScore.ToString();
            GameManager.instance.streakText.text = GameManager.instance.colorStreek.Count.ToString();
            GameManager.instance.streakImage.color = color;
            GameManager.instance.streakPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.colorStreek.Clear();
            GameManager.instance.SetColorStreak(color);
            GameManager.instance.playerScore += points;
            GameManager.instance.scoreText.text = GameManager.instance.playerScore.ToString();
            GameManager.instance.streakText.text = GameManager.instance.colorStreek.Count.ToString();
            GameManager.instance.streakImage.color = color;
            GameManager.instance.streakPanel.SetActive(true);
        }
    }
}
