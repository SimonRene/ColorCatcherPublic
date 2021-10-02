using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{


    private int m_currentStep;

    private bool m_rotatedRight, m_rotatedLeft;

    public GameStarter m_gameStarter;

    public GameObject m_swipeLR_Text, m_swipeD_Text, m_swipeLR_sym, m_swipeD_sym, m_tilt_Text, m_OK_button;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        InputManager.Instance.OnTouchSwipe += Swipe;


        m_swipeLR_Text.SetActive(true);
        m_swipeLR_sym.SetActive(true);

        m_swipeD_Text.SetActive(false);
        m_swipeD_sym.SetActive(false);

        m_OK_button.SetActive(false);
        m_tilt_Text.SetActive(false);

        m_currentStep = 0;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnTouchSwipe -= Swipe;
    }




    private void Swipe(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0f)
            {
                RotatedRight();
            }
            else if (direction.x < 0f)
            {
                RotatedLeft();
            }
        }
        else
        {
            if (direction.y < 0f)
            {
                Stamped();
            }
        }
    }

    public void NextStep()
    {
        m_OK_button.SetActive(false);

        if (m_currentStep == 0)
        {
            m_swipeLR_Text.SetActive(false);
            m_swipeLR_sym.SetActive(false);

            m_swipeD_Text.SetActive(true);
            m_swipeD_sym.SetActive(true);

        }
        else if(m_currentStep == 1)
        {
            m_swipeD_Text.SetActive(false);
            m_swipeD_sym.SetActive(false);

            m_tilt_Text.SetActive(true);
            m_OK_button.SetActive(true);

        }
        else if (m_currentStep == 2)
        {
            m_gameStarter.CloseTutorial();
        }

        ++m_currentStep;

    }

    private void ShowOK_Button()
    {
        m_OK_button.SetActive(true);
    }


    private void RotatedRight()
    {
        if(m_currentStep == 0 && m_rotatedLeft)
        {
            ShowOK_Button();
        }
        else if(m_currentStep == 0)
        {
            m_rotatedRight = true;
        }
    }

    private void RotatedLeft()
    {
        if (m_currentStep == 0 && m_rotatedRight)
        {
            ShowOK_Button();
        }
        else if (m_currentStep == 0)
        {
            m_rotatedLeft = true;
        }
    }

    private void Stamped()
    {
        if(m_currentStep == 1)
        {
            ShowOK_Button();
        }
    }
}
