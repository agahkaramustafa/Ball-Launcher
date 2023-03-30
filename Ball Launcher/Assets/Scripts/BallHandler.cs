using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] Rigidbody2D currentBallRB;
    [SerializeField] SpringJoint2D currentBallSJ;
    [SerializeField] float detachDelay = .15f;

    Camera mainCamera;
    bool isDragging;

    void Start() 
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        if (currentBallRB == null) { return; }

        if (!Touchscreen.current.primaryTouch.press.IsPressed())
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;
            return;
        }

        isDragging = true;
        currentBallRB.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRB.position = worldPosition;

        
    }

    void LaunchBall()
    {
        currentBallRB.isKinematic = false;
        currentBallRB = null;

        Invoke(nameof(DetachBall), detachDelay);
    }

    void DetachBall()
    {
        currentBallSJ.enabled = false;
        currentBallSJ = null;
    }
}
