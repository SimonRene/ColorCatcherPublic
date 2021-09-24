using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{

    public static GameController _instance;

    private void Awake()
    {

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

    }
    

    // Start is called before the first frame update
    void Start()
    {
        
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

    private void SpawnCatchable()
    {
        var catchable = GameObject.Instantiate(prefab_catchable, transform);
        var catchablePosition = catchable.transform.position;
        catchablePosition.y = 7.7f;
        catchable.transform.position = catchablePosition;
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

    public int Points
    {
        get
        {
            return m_points;
        }
    }

    public UnityEvent pointsChanged;

    private int m_points;

    public GameObject prefab_catchable;

    float m_timeSinceLastCatchable;
    public float m_catchableInterval;

}
