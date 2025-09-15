using UnityEngine;
using UnityEngine.SceneManagement;

public class pin : MonoBehaviour
{
    private bool scored = false;

    void Update()
    {
        if (!scored)
        {
            float tilt = Vector3.Angle(Vector3.up, transform.up);

            if (tilt > 45f) // Pin fell
            {
                scored = true;
                int currentPlayer = gameManager.Instance.GetCurrentPlayer();
                scoreManager.Instance.AddScore(currentPlayer, 1);
            }
        }
    }
}
