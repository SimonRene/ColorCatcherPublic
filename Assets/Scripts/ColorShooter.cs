using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShooter : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        m_originalPosition = transform.position;

        m_targetZ = 0f;
        m_speedZ = 0f;
        m_velocityZ = 0f;
        m_maximumZ = 1.4f;

        m_targetAvailable = false;


        m_movingDown = false;
        m_maximumDown = 0f;
        m_downStepDistance = (transform.localPosition.y - m_maximumDown) / m_downSteps;


    }

    // Update is called once per frame
    void Update()
    {
        if (!m_targetAvailable)
        {
            CalculateNewTargetZ();
        }
        else
        {

            float movementZ = m_velocityZ * Time.deltaTime;
            float distanceToTarget = m_targetZ - transform.localPosition.z;

            if (Mathf.Abs(movementZ) < Mathf.Abs(distanceToTarget))
            {
                MoveZ(movementZ);
            }
            else
            {
                MoveZ(distanceToTarget);

                m_targetAvailable = false;
            }
        }

        if(m_movingDown)
        {
            float movementDown = -m_downVelocity * Time.deltaTime;
            float distanceToTarget = m_downTarget - transform.localPosition.y;

            var pos = transform.localPosition;

            if (Mathf.Abs(movementDown) < Mathf.Abs(distanceToTarget))
            {
                pos.y += movementDown;
            }
            else
            {
                pos.y += distanceToTarget;
                m_movingDown = false;
            }

            transform.localPosition = pos;
        }
    }

    public void MoveDown()
    {
        if (++m_stepsDownMade > m_downSteps)
        {
            m_downTarget = transform.localPosition.y - m_downStepDistance*3;
            LostGame();
        }
        else
        {
            m_downTarget = transform.localPosition.y - m_downStepDistance;
        }
            m_movingDown = true;
    }

    private void MoveZ(float offsetZ)
    {
        Vector3 pos = transform.localPosition;
        pos.z += offsetZ;
        transform.localPosition = pos;
    }

    private void LostGame()
    {
        
        GameController._instance.GameLost();
    }


    private void CalculateNewTargetZ()
    {
        m_targetZ = (Random.value * m_maximumZ * 2f) - m_maximumZ;

        m_speedZ = Random.value * (m_maximumSpeed - m_minumumSpeed) + m_minumumSpeed;

        m_velocityZ = (m_targetZ - transform.localPosition.z) > 0f ? (m_speedZ) : (-m_speedZ);

        float distanceToNewTarget = m_targetZ - transform.localPosition.z;

        if (Mathf.Abs(distanceToNewTarget) >= m_minumumDistanceToNewTarget)
            m_targetAvailable = true;

    }

    private bool m_targetAvailable;

    public float m_maximumSpeed;
    public float m_minumumSpeed;
    public float m_minumumDistanceToNewTarget;

    private float m_targetZ;
    private float m_speedZ;
    private float m_velocityZ;

    private float m_maximumZ;

    // down movement

    private bool m_movingDown;

    public int m_downSteps;
    private int m_stepsDownMade;

    private float m_downStepDistance;

    public float m_downVelocity;

    private float m_maximumDown;

    private float m_downTarget;


    private Vector3 m_originalPosition;
}
