using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        m_gameOverMenue.SetActive(false);

        UpdatePoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdatePoints()
    {
        m_pointsText.text = "Points: " + GameController._instance.Points + "\nHighscore: " + PlayerPrefs.GetInt("Highscore");
    }

    public void ShowGameOver()
    {
        m_gameOverMenue.SetActive(true);

        m_pointsText.gameObject.SetActive(false);

        if(GameController._instance.m_newHighscore)
        {
            m_gameOverHighscoreText.gameObject.SetActive(true);
            m_gameOverPoints.text = "Score: " + GameController._instance.Points;
        }
        else
        {
            m_gameOverHighscoreText.gameObject.SetActive(false);
            m_gameOverPoints.text = "Score: " + GameController._instance.Points + "\nHigscore: " + PlayerPrefs.GetInt("Highscore");
        }
    }

    public TextMeshProUGUI m_pointsText, m_gameOverPoints, m_gameOverHighscoreText;
    public GameObject m_gameOverMenue;
}
