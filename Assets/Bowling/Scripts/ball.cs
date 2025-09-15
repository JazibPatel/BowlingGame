using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    public float forceMultiplier = 10f; // Adjust to control swipe strength
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Rigidbody rb;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Mouse swipe (PC)
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            Swipe();
        }

        // Touch swipe (Mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                Swipe();
            }
        }
    }

    void Swipe()
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;

        if (swipeDirection.magnitude > 50f) // Minimum swipe distance
        {
            // Base direction from camera
            Vector3 forward =
                gameManager.Instance.GetCurrentPlayer() == 0
                    ? Vector3.back // Player 1 throws "backward"
                    : Vector3.forward; // Player 2 throws "forward" after camera flip

            // Add some left/right curve based on swipe X
            Vector3 forceDirection = forward + (Vector3.right * (swipeDirection.x / 200f));


            rb.AddForce(forceDirection.normalized * forceMultiplier, ForceMode.Impulse);
        }
    }

    // Detect collision with pin
    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit && collision.gameObject.CompareTag("pin"))
        {
            hasHit = true;
            Debug.Log("Ball hit a pin! Waiting 2 sec before switching...");

            StartCoroutine(WaitAndEndTurn());
        }
    }

    private IEnumerator WaitAndEndTurn()
    {
        yield return new WaitForSeconds(5f); // wait for pins to fall
        gameManager.Instance.EndTurn(); // call manager to flip camera + switch player
    }
}
