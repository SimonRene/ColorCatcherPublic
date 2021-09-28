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

        GetComponentInChildren<SpriteRenderer>().color = PlayerEdge.colorPicker[m_color];

        m_particleSystem = GetComponentInChildren<ParticleSystem>();

        var ma = m_particleSystem.main;
        ma.startColor = PlayerEdge.colorPicker[m_color];



        m_animation = GetComponent<Animation>();

        m_catched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_catched)
            transform.Translate(0f, -m_speed * Time.deltaTime, 0f);
    }

    private void Catched()
    {
        m_catched = true;
        m_animation.Play();
        m_particleSystem.Play();
    }

    private void RemoveCatchable()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_catched)
            return;

        if (collision.gameObject.tag == "Player")
        {
            bool correct = collision.gameObject.GetComponentInParent<PlayerController>().CatchColor(m_color);

            if(correct)
            {
                // this happens when the color was correct
                GameController._instance.CorrectColorCatched();
                Catched();
            }
            else
            {
                // this happens when the color was wrong
                GameController._instance.WrongColorCatched();
                Catched();
            }


        }
        else if(collision.gameObject.tag == "Finish")
        {
            // this happens when the color has not been catched
            GameController._instance.ColorNotCatched();
            Catched();
        }
    }

    public float m_speed;
    private EdgeColor m_color;

    private Animation m_animation;

    private ParticleSystem m_particleSystem;

    private bool m_catched;
}
