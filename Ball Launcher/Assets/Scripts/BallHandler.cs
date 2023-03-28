using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] Rigidbody2D currentBallRB;

    Camera mainCamera;

    void Start() 
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        if (!Touchscreen.current.primaryTouch.press.IsPressed())
        {
            currentBallRB.isKinematic = false;
            return;
        }

        currentBallRB.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRB.position = worldPosition;

        
    }
}
