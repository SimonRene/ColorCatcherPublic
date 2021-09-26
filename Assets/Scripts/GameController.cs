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

        pointsChanged.Invoke();
    }

    public void Mistake()
    {
        --m_points;

        pointsChanged.Invoke();
    }


    public void CorrectColorCatched()
    {
        Score();
        correctColorCatched.Invoke();
    }

    public void WrongColorCatched()
    {
        Mistake();
        wrongColorCatched.Invoke();
    }

    public void ColorNotCatched()
    {
        Mistake();
        colorNotCatched.Invoke();
    }

    public void GameLost()
    {
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

    public UnityEvent pointsChanged, gameLost;

    private int m_points;

    public GameObject prefab_catchable;

    float m_timeSinceLastCatchable;
    public float m_catchableInterval;

    public bool m_newHighscore;
    private bool m_lostGame;

    public UnityEvent correctColorCatched;
    public UnityEvent wrongColorCatched;
    public UnityEvent colorNotCatched;

}
