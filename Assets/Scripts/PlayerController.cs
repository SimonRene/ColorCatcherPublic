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

    public UnityEvent correctColorCatched;
    public UnityEvent wrongColorCatched;


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


    public void RotateLeft(InputAction.CallbackContext context)
    {
        if(!m_isRotating && context.performed) {
            m_isRotating = true;
            m_animation.Play("RotateLeft");
        }
        else if (m_isRotating && context.performed)
        {
            // saves the desired next rotation direction, to perform it after the current rotation is finished
            m_nextRotation = -1f;
        }

        // TODO: switch the current top facing edge in the middle of the animation !!!

    }
    public void RotateRight(InputAction.CallbackContext context)
    {
        if (!m_isRotating && context.performed)
        {
            m_isRotating = true;
            m_animation.Play("RotateRight");
        }
        else if (m_isRotating && context.performed)
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


    public void Move(InputAction.CallbackContext context)
    {
        m_velocityX = context.ReadValue<Vector2>().x;
        
    }


    public bool CatchColor(EdgeColor color)
    {
        if(color == m_currentTopFacingEdge.EdgeColor)
        {
            print("Caught color: " + color);
            correctColorCatched.Invoke();
            return true;
        }
        else
        {
            print("Caught color: " + color + " but top was " + m_currentTopFacingEdge.EdgeColor);
            wrongColorCatched.Invoke();
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

    public float m_movementSpeed;

    private PlayerEdge[] m_playerEdges;

    public SpriteRenderer[] m_edgeSprites;

    private float m_velocityX;
    
    private Quaternion m_defaultRotation;

    private bool m_isRotating;

    private float m_nextRotation;

    private PlayerEdge m_currentTopFacingEdge;

}
