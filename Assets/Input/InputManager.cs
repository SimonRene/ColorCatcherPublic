using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
    public delegate void TouchSwipeEvent(Vector2 delta);
    public event TouchSwipeEvent OnTouchSwipe;

    public delegate void MoveEvent(float speed);
    public event MoveEvent OnMove;



    private ColorCatcherInput ccInput;

    private float m_currentTouchStartTime;
    private Vector2 m_currentTouchStartPosition;

    private float m_maxTouchSwipeDuration = 0.5f;

    private float m_currentMoveVelocity;

    private void Awake()
    {
        ccInput = new ColorCatcherInput();
    }

    private void OnEnable()
    {
        ccInput.Enable();
    }

    private void OnDestroy()
    {
        ccInput.Disable();
    }

    private void Start()
    {
        ccInput.Player.TouchPress.started += ctx => StartTouch(ctx);
        ccInput.Player.TouchPress.canceled += ctx => EndTouch(ctx);

        ccInput.Player.Move.performed += ctx => Move(ctx);
        ccInput.Player.Move.canceled += ctx => Move(ctx);

        ccInput.Player.GyroTest.performed += ctx => Gyro(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("Touch Started " + ccInput.Player.TouchPosition.ReadValue<Vector2>());

        m_currentTouchStartTime = (float)context.startTime;
        m_currentTouchStartPosition = ccInput.Player.TouchPosition.ReadValue<Vector2>();

        if (OnStartTouch != null)
        {
            OnStartTouch(m_currentTouchStartPosition, m_currentTouchStartTime);
        }

    }
    
    private void EndTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("Touch Ended " + ccInput.Player.TouchPosition.ReadValue<Vector2>());

        if (OnEndTouch != null)
        {
            OnEndTouch(ccInput.Player.TouchPosition.ReadValue<Vector2>(), (float)context.time);
        }

        if(OnTouchSwipe != null)
        {
            Vector2 touchEndPosition = ccInput.Player.TouchPosition.ReadValue<Vector2>();
            float touchEndTime = (float)context.time;

            float touchDuration = touchEndTime - m_currentTouchStartTime;

            if(touchDuration <= m_maxTouchSwipeDuration)
            {
                Vector2 swipeDelta = touchEndPosition - m_currentTouchStartPosition;

                if(swipeDelta.magnitude > 10f)
                    OnTouchSwipe(swipeDelta);
            }

        }
    }

    private void Move(InputAction.CallbackContext context)
    {

        if(OnMove != null)
        {
            Vector2 v2 = ccInput.Player.Move.ReadValue<Vector2>();

            OnMove(v2.x);
        }
    }

    private void Gyro(InputAction.CallbackContext context)
    {

        if(OnMove != null)
        {
            Vector3 angularVelocity = ccInput.Player.GyroTest.ReadValue<Vector3>();

            OnMove(-angularVelocity.z);
        }
    }



}
