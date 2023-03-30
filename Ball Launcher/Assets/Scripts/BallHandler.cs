using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
