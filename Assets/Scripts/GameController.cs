using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController _instance;

    private void Awake()
    {
        _instance = this;

        /*
        // check if a instance exists
        if (_instance == null)
        {
            // No instance exists so make this the instance
            _instance = this;
            // Do not destroy the gameobject this script is attached to so
            // it can persist through scenes.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Singleton instance already exists. This is an imposter so kill it. There can be only one!
            Destroy(this);
        }
        */

        

    }
    

    // Start is called before the first frame update
    void Start()
    {

        m_lostGame = false;

        m_correctInRow = 0;
        m_neededCorrectRow = 3;

        SetColorSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        m_timeSinceLastCatchable += Time.deltaTime;
        if(m_timeSinceLastCatchable > m_catchableInterval)
        {
            SpawnCatchable();
            m_timeSinceLastCatchable = 0f;
        }
    }

    public void PauseGame()
    {
        if(!m_gamePaused)
        {
            Debug.Log("PAUSE GAME");
            Time.timeScale = 0f;

            gamePaused.Invoke();

            m_gamePaused = true;

        }
        else
        {
            Debug.Log("RESUME GAME");
            Time.timeScale = 1f;

            gameResumed.Invoke();
            m_gamePaused = false;
        }

    }

    public void LeaveGame()
    {
        Debug.Log("LEAVE GAME");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene("MainMenue");
    }

    private void SetColorSpawner()
    {
        var spawns = GameObject.FindGameObjectsWithTag("ColorSpawner");
        if (spawns.Length != 2)
        {
            Debug.LogError("Wrong number of ColorSpawner");
        }
        else
        {
            leftColorSpawn = spawns[0].transform;
            rightColorSpawn = spawns[1].transform;
        }
    }

    private void SpawnCatchable()
    {
        if (m_lostGame)
            return;

        if (leftColorSpawn == null || rightColorSpawn == null)
            SetColorSpawner();

        Vector3 spawnPosition = Random.value < 0.5f ? leftColorSpawn.position : rightColorSpawn.position;


        var catchable = GameObject.Instantiate(prefab_catchable, transform);
        //var catchablePosition = catchable.transform.position;
        //catchablePosition.y = 7.7f;
        catchable.transform.position = spawnPosition;
    }

    public void Score()
    {
        ++m_points;
        if (++m_correctInRow > m_neededCorrectRow)
            m_correctInRow = 1;

        pointsChanged.Invoke();
    }

    public void Mistake(int penalty)
    {
        m_points -= 2;
        m_correctInRow = 0;

        pointsChanged.Invoke();
    }


    public void CorrectColorCatched()
    {
        Score();
        correctColorCatched.Invoke();
    }

    public void WrongColorCatched()
    {
        Mistake(2);
        wrongColorCatched.Invoke();
    }

    public void ColorNotCatched()
    {
        Mistake(1);
        colorNotCatched.Invoke();
    }

    public void GameLost()
    {
        m_lostGame = true;

        int highscore = PlayerPrefs.GetInt("Highscore");
        if (m_points > highscore)
        {
            m_newHighscore = true;
            PlayerPrefs.SetInt("Highscore", m_points);
        }
        else
        {
            m_newHighscore = false;
        }

        gameLost.Invoke();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


    public int Points
    {
        get
        {
            return m_points;
        }
    }

    public Transform leftColorSpawn, rightColorSpawn;

    public UnityEvent pointsChanged, gameLost, gamePaused, gameResumed;

    private int m_points;

    public GameObject prefab_catchable;

    float m_timeSinceLastCatchable;
    public float m_catchableInterval;

    [HideInInspector]
    public bool m_newHighscore;
    private bool m_lostGame, m_gamePaused;

    [HideInInspector]
    public int m_correctInRow;
    [HideInInspector]
    public int m_neededCorrectRow; // correct catched needed to move the machine up again

    public UnityEvent correctColorCatched;
    public UnityEvent wrongColorCatched;
    public UnityEvent colorNotCatched;

}
