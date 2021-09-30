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

        PauseMenue(false);
        UpdatePoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdatePoints()
    {
        m_pointsText.text = "Score: " + GameController._instance.Points;
        
        m_highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
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

    public void PauseMenue(bool show)
    {
        m_pauseMenue.SetActive(show);
    }

    public TextMeshProUGUI m_pointsText, m_highscoreText, m_gameOverPoints, m_gameOverHighscoreText;
    public GameObject m_gameOverMenue, m_pauseMenue;
}
