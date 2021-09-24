using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchableController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int rnd = (int)(Random.value * 3.99);

        switch(rnd)
        {
            case 0:
                m_color = EdgeColor.RED;
                break;
            case 1:
                m_color = EdgeColor.BLUE;
                break;
            case 2:
                m_color = EdgeColor.GREEN;
                break;
            case 3:
                m_color = EdgeColor.YELLOW;
                break;
        }

        GetComponent<SpriteRenderer>().color = PlayerEdge.colorPicker[m_color];

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, -m_speed * Time.deltaTime, 0f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bool correct = collision.gameObject.GetComponentInParent<PlayerController>().CatchColor(m_color);

            if(correct)
            {
                // this happens when the color was correct
                Destroy(gameObject);
            }
            else
            {
                // this happens when the color was wrong
                Destroy(gameObject);
            }


        }
    }

    public float m_speed;
    private EdgeColor m_color;
}
