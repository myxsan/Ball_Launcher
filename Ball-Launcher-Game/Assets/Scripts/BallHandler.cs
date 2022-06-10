using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float respawnDelay; 
    [SerializeField] float detachDelay;

    Rigidbody2D currentBallRigidbody;
    SpringJoint2D currentBallSpringJoint;
    
    private Camera mainCamera;
    private bool isDragging;

    
    private void Start() {
        mainCamera = Camera.main;

        SpawnNewBall();
    }

    void Update()
    {
        if(currentBallRigidbody == null) { return; }
        BodyTypeHandler();
    }

    void BodyTypeHandler()
    {
        if(Touchscreen.current.primaryTouch.press.isPressed)
        {
            currentBallRigidbody.bodyType = RigidbodyType2D.Kinematic;

            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            currentBallRigidbody.position = worldPosition;
            isDragging = true;

        }
        else
        {
            currentBallRigidbody.bodyType = RigidbodyType2D.Dynamic;

            if(isDragging)
            {
                LaunchBall();
            }

            isDragging = false;
        }
    }

    private void LaunchBall()
    {
        currentBallRigidbody = null;
        Debug.Log("true");
        Invoke(nameof(DeatchBall),detachDelay);
    }

    private void DeatchBall()
    {
        Debug.Log("true");
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.transform.position, Quaternion.identity);

        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }
}