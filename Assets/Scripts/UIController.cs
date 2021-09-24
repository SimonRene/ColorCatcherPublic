using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdatePoints()
    {
        m_pointsText.text = "Points: " + GameController._instance.Points;
    }

    public TextMeshProUGUI m_pointsText;
}
