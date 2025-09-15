using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;

    [Header("Prefabs")]
    public GameObject ballPrefab;
    public GameObject pinPrefab;

    [Header("Spawn Points")]
    public Transform ballSpawn;
    public Transform[] pinSpawns;   // 10 pin spawn points
    public Transform cameraTransform;

    [Header("Background")]
    public Transform backgroundTransform;
    private bool backgroundFlipped = false;

    private int currentPlayer = 1;
    private GameObject currentBall;
    private GameObject[] currentPins;

    public Material player1BallMaterial;
    public Material player2BallMaterial;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnBallAndPins();
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void EndTurn()
    {
        StartCoroutine(SwitchTurn());
    }

    private IEnumerator SwitchTurn()
    {
        yield return new WaitForSeconds(2f);

        // Flip camera 180
        Quaternion targetRot = cameraTransform.rotation * Quaternion.Euler(0, 0, 180);
        Vector3 scale = backgroundTransform.localScale;
        scale.x *= -1;  // horizontal flip
        scale.y *= -1;  // vertical flip if needed

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRot, t);

            //backgroundTransform.localScale = scale;
            //yield return null;
        }

        // After camera flip, set exact scale
        backgroundTransform.localScale = scale;

        // Manually set Y position
        Vector3 pos = backgroundTransform.localPosition;
        pos.y = backgroundFlipped ? 177f : -177f;  // desired Y position
        backgroundTransform.localPosition = pos;

        backgroundFlipped = !backgroundFlipped;

        // Destroy old ball & pins
        if (currentBall) Destroy(currentBall);
        if (currentPins != null)
        {
            foreach (var pin in currentPins)
                if (pin) Destroy(pin);
        }

        // Switch player
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
        Debug.Log("Now Player " + currentPlayer + "'s turn!");

        // Spawn new ball & pins
        SpawnBallAndPins();
    }

    private void SpawnBallAndPins()
    {
        // Spawn Ball
        currentBall = Instantiate(ballPrefab, ballSpawn.position, ballSpawn.rotation);

        // ✅ Change ball material based on player
        Renderer ballRenderer = currentBall.GetComponent<Renderer>();
        if (ballRenderer != null)
        {
            if (currentPlayer == 1)
                ballRenderer.material = player1BallMaterial;
            else
                ballRenderer.material = player2BallMaterial;
        }

        // Spawn Pins
        currentPins = new GameObject[pinSpawns.Length];
        for (int i = 0; i < pinSpawns.Length; i++)
        {
            currentPins[i] = Instantiate(pinPrefab, pinSpawns[i].position, pinSpawns[i].rotation);
        }
    }

}

