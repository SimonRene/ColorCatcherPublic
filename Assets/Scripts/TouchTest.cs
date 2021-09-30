using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchTest : MonoBehaviour
{
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //inputManager.OnStartTouch += Touch;
        //inputManager.OnEndTouch += EndTouch;
        //inputManager.OnTouchSwipe += Swipe;

        

        TouchSimulation.Enable();
    }

    private void OnDisable()
    {
        //inputManager.OnStartTouch -= Touch;
        //inputManager.OnEndTouch -= EndTouch;
        //inputManager.OnTouchSwipe -= Swipe;


        TouchSimulation.Disable();
    }


    public void Swipe(Vector3 velocity)
    {
        touchTestText.text = ("GYRO " + velocity);
    }

    public void Touch(Vector2 position, float time)
    {
        touchTestText.text = "Started: " + position.x + " = " + position.y;
        
    }

    private void EndTouch(Vector2 position, float time)
    {
        touchTestText.text = "Ended: " + position.x + " = " + position.y;
    }

    public TextMeshProUGUI touchTestText;
}
