using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivotRB;
    [SerializeField] float detachDelay;
    [SerializeField] float respawnDelay;

    Rigidbody2D currentBallRB;
    SpringJoint2D currentBallSJ;

    Camera mainCamera;
    bool isDragging;

    void Start() 
    {
        mainCamera = Camera.main;

        SpawnNewBall();
    }

    void OnEnable() 
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable() 
    {
        EnhancedTouchSupport.Disable();
    }
    
    void Update()
    {
        if (currentBallRB == null) { return; }

        if (Touch.activeTouches.Count == 0)
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;
            return;
        }

        Vector2 touchPosition = new Vector2();

        foreach (Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition;
        }

        touchPosition /= Touch.activeTouches.Count;

        isDragging = true;
        currentBallRB.isKinematic = true;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRB.position = worldPosition;

        
    }

    void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivotRB.position, Quaternion.identity);

        currentBallRB = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSJ = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSJ.connectedBody = pivotRB;
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

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
