using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreManager : MonoBehaviour
{
    public static scoreManager Instance;

    private int player1Score = 0;
    private int player2Score = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int player, int points)
    {
        if (player == 1)
            player1Score += points;
        else
            player2Score += points;

        Debug.Log($"Player 1: {player1Score} | Player 2: {player2Score}");
    }

    public int GetScore(int player)
    {
        return (player == 1) ? player1Score : player2Score;
    }
}

