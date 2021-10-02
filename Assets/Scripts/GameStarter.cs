using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        if(UnityEngine.InputSystem.Gyroscope.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);

            if (UnityEngine.InputSystem.Gyroscope.current.enabled)
            {
                textLine.text = "!! GYRO AVAILABLE !!";

            }
            else
            {
                textLine.text = "NO GYRO AVAILABLE";
                InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            }
        }
        else
        {
            Debug.LogWarning("NO GYROSCOPE AVAILABLE");
        }


        if (UnityEngine.InputSystem.Accelerometer.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);

            if (UnityEngine.InputSystem.Accelerometer.current.enabled)
            {
                textLine.text = "!! ACC AVAILABLE !!";

            }
            else
            {
                textLine.text = "NO ACC AVAILABLE";
                InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);
            }
        }
        else
        {
            Debug.LogWarning("NO Accelerometer AVAILABLE");
        }


        m_infoMenueOpen = false;
        m_infoMenue.SetActive(false);
        m_tutorialMenue.SetActive(false);
        m_mainManue.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void StartGame()
    {
        Debug.Log("START GAME");
        SceneManager.LoadScene("Scene");
    }

    public void ShowHighscore()
    {
        Debug.Log("SHOW HIGHSCORE");
    }

    public void OpenWebsite()
    {
        Debug.Log("OPEN WEBSITE");
        Application.OpenURL("https://DogTagGames.com/");
    }
    
    public void OpenLayerLabWebsite()
    {
        Debug.Log("OPEN WEBSITE");
        Application.OpenURL("https://layerlabgames.com/");
    }

    public void SwitchInfoMenue()
    {
        if(m_infoMenueOpen)
        {
            m_infoMenue.SetActive(false);
            m_mainManue.SetActive(true);

            m_infoMenueOpen = false;
        }
        else
        {
            m_infoMenue.SetActive(true);
            m_mainManue.SetActive(false);

            m_infoMenueOpen = true;
        }
    }

    public void OpenTutorial()
    {
        m_mainManue.SetActive(false);
        m_tutorialMenue.SetActive(true);
    }

    public void CloseTutorial()
    {
        m_tutorialMenue.SetActive(false);
        m_mainManue.SetActive(true);
    }


    public TextMeshProUGUI textLine;


    public GameObject m_mainManue, m_infoMenue, m_tutorialMenue;

    private bool m_infoMenueOpen;
}
