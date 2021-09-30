using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum EdgeColor
{
    RED,
    BLUE,
    GREEN,
    YELLOW
}

public class PlayerController : MonoBehaviour
{


    // Start is called before the first frame update
    void Awake()
    {

        m_velocityX = 0f;
        m_defaultRotation = transform.rotation;
        m_isRotating = false;
        m_nextRotation = 0f;

        m_playerEdges = new PlayerEdge[4];
        m_playerEdges[0] = m_edgeSprites[0].GetComponent<PlayerEdge>();
        m_playerEdges[1] = m_edgeSprites[1].GetComponent<PlayerEdge>();
        m_playerEdges[2] = m_edgeSprites[2].GetComponent<PlayerEdge>();
        m_playerEdges[3] = m_edgeSprites[3].GetComponent<PlayerEdge>();

        m_playerEdges[0].EdgeColor = EdgeColor.RED;
        m_playerEdges[1].EdgeColor = EdgeColor.BLUE;
        m_playerEdges[2].EdgeColor = EdgeColor.GREEN;
        m_playerEdges[3].EdgeColor = EdgeColor.YELLOW;

        m_currentTopFacingEdge = m_playerEdges[0];

    }

    private void OnEnable()
    {
        InputManager.Instance.OnTouchSwipe += PerformRotation;
        InputManager.Instance.OnMove += Move;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnTouchSwipe -= PerformRotation;
        InputManager.Instance.OnMove -= Move;
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if(keyboard.qKey.wasPressedThisFrame)
        {
            ChangeEdgeColor(0, EdgeColor.RED);
        }
        else if (keyboard.wKey.wasPressedThisFrame)
        {
            ChangeEdgeColor(0, EdgeColor.BLUE);
        }
        else if (keyboard.eKey.wasPressedThisFrame)
        {
            ChangeEdgeColor(0, EdgeColor.GREEN);
        }
        else if (keyboard.rKey.wasPressedThisFrame)
        {
            ChangeEdgeColor(0, EdgeColor.YELLOW);
        }


        transform.Translate(m_velocityX * m_movementSpeed * Time.deltaTime, 0f, 0f);
        var playerPosition = transform.position;
        playerPosition.x = Mathf.Clamp(transform.position.x, -3f, 3f);
        transform.position = playerPosition;

        if(!m_isRotating)
        {
            // reset the rotation of the player after the rotation animation is finished
            if((m_playerRotationTransform.rotation.eulerAngles.z != 0f)) {
                m_playerRotationTransform.rotation = Quaternion.Euler(0f,0f,0f);
            }
        
            // perform the desired rotation saved during the last rotation
            if(m_nextRotation != 0f)
            {
                if(m_nextRotation > 0f)
                {
                    m_isRotating = true;
                    m_animation.Play("RotateRight");
                }
                else if(m_nextRotation < 0f)
                {
                    m_isRotating = true;
                    m_animation.Play("RotateLeft");
                }

                m_nextRotation = 0f;
            }
        }

    }


    public void RotateLeft()
    {
        if(!m_isRotating) {
            m_isRotating = true;
            m_animation.Play("RotateLeft");
        }
        else if (m_isRotating)
        {
            // saves the desired next rotation direction, to perform it after the current rotation is finished
            m_nextRotation = -1f;
        }

        // TODO: switch the current top facing edge in the middle of the animation !!!

    }
    public void RotateRight()
    {
        if (!m_isRotating)
        {
            m_isRotating = true;
            m_animation.Play("RotateRight");
        }
        else if (m_isRotating)
        {
            // saves the desired next rotation direction, to perform it after the current rotation is finished
            m_nextRotation = 1f;
        }
    }

    // gets called in the middle of the rotation animation to switch the top facing edge.
    private void UpdateTopFacingEdgeColor()
    {
        if (m_playerRotationTransform.rotation.z < 0f)
        {
            m_currentTopFacingEdge = m_playerEdges[1];
        }
        else if (m_playerRotationTransform.rotation.z > 0f)
        {
            m_currentTopFacingEdge = m_playerEdges[3];
        }
    }

    private void FinishRotationRight()
    {
        // multiple resets of the rotation to ensure that there will be no visual "lag"
        m_animation.Play("RotateReset");

        m_isRotating = false;
        m_playerRotationTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

        EdgeColor tmp = m_playerEdges[0].EdgeColor;

        for(int i = 0; i< m_playerEdges.Length-1; ++i)
        {
            m_playerEdges[i].EdgeColor = m_playerEdges[(i + 1) % m_playerEdges.Length].EdgeColor;
        }
        m_playerEdges[m_playerEdges.Length-1].EdgeColor = tmp;

        m_playerRotationTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

        m_currentTopFacingEdge = m_playerEdges[0];
    }

    private void FinishRotationLeft()
    {
        // multiple resets of the rotation to ensure that there will be no visual "lag"
        m_animation.Play("RotateReset");

        m_isRotating = false;
        m_playerRotationTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

        EdgeColor tmp = m_playerEdges[m_playerEdges.Length - 1].EdgeColor;

        for (int i = m_playerEdges.Length - 1; i > 0; --i)
        {
            m_playerEdges[i].EdgeColor = m_playerEdges[(i - 1)].EdgeColor;
        }
        m_playerEdges[0].EdgeColor = tmp;

        m_playerRotationTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

        m_currentTopFacingEdge = m_playerEdges[0];

    }

    public void Stamp()
    {
        m_subAnimation.Play();
    }

    // changes the color of the bottom player edge to the color the Player has been stamped on
    public void HasStamped()
    {
        float posX = transform.position.x;


        if(posX >= 0f)
        {
            if(posX > 2.1f)
            {
                // GREEN
                ChangeEdgeColor(2, EdgeColor.GREEN);
            }
            else
            {
                // YELLOW
                ChangeEdgeColor(2, EdgeColor.YELLOW);
            }
        }
        else
        {
            if (posX < -2.1f)
            {
                // RED
                ChangeEdgeColor(2, EdgeColor.RED);
            }
            else
            {
                // BLUE
                ChangeEdgeColor(2, EdgeColor.BLUE);
            }
        }


        print("Stamped");
    }


    public void Move(float value)
    {
        m_velocityX = value;
        
    }

    public void Rotation(InputAction.CallbackContext context)
    {
        if (!context.performed || context.duration > 0f)
            return;

        
        

        Vector2 delta = context.ReadValue<Vector2>();

        PerformRotation(delta);
    }

    private void PerformRotation(Vector2 direction)
    {
        

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0f)
            {
                RotateRight();
            }
            else if (direction.x < 0f)
            {
                RotateLeft();
            }
        }
        else
        {
            if (direction.y < 0f)
            {
                Stamp();
            }
        }
    }


    public bool CatchColor(EdgeColor color)
    {
        if(color == m_currentTopFacingEdge.EdgeColor)
        {
            //print("Caught color: " + color);
            return true;
        }
        else
        {
            //print("Caught color: " + color + " but top was " + m_currentTopFacingEdge.EdgeColor);
            m_currentTopFacingEdge.EdgeColor = color;
            return false;
        }
    }


    public void ChangeEdgeColor(int edge, EdgeColor color)
    {
        if (edge >= 0 && edge < 4)
            m_playerEdges[edge].EdgeColor = color;
        else
            Debug.LogError("PlayerController::ChangeEdgeColor - edge number out of range. ( " + (edge) + " )" );
    }

    public Transform m_playerRotationTransform;

    public Animation m_animation;
    public Animation m_subAnimation;

    public float m_movementSpeed;

    private PlayerEdge[] m_playerEdges;

    public SpriteRenderer[] m_edgeSprites;

    private float m_velocityX;
    
    private Quaternion m_defaultRotation;

    private bool m_isRotating;

    private float m_nextRotation;

    private PlayerEdge m_currentTopFacingEdge;

}
