using UnityEngine;
using TMPro; // TMP_Text

// Updates the HUD every frame from the ScoreManager and PlayerPowerUps.
public class HUDController : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text health;

    private void Update()
    {
        UpdateScore();
        UpdateHealth();
    }

    private void UpdateScore()
    {
        // The Score property from Lab 4: anyone can read it
        scoreText.text = "Score: " + GameManager.Instance.Score;
    }

    private void UpdateHealth()
    {
        // Update the health text (assuming you have a way to get the player's health)
        health.text = "Health: " + GameManager.Instance.Health;
    }
}
