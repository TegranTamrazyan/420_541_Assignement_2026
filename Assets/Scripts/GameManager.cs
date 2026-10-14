using UnityEngine;
using UnityEngine.SceneManagement;


// Keeps track of the player's score.
// It's a singleton: there is only ever one, reachable from any script
// through GameManager.Instance.
public class GameManager : MonoBehaviour
{
    // The single shared instance that any script can reach.
    // "static" means it belongs to the class, not to one object.
    public static GameManager Instance { get; private set; }


    // Anyone can read the score, only GameManager can change it
    public int Score { get; private set; }
    public int Health { get; private set; } = 3;
    private bool isScoreBoostActive = false;

    // "static" means this value belongs to the GameManager CLASS,
    // not to one GameManager object. Objects are destroyed when a
    // new scene loads, but static values stay in memory.
    public static int FinalScore { get; private set; }
    public static string SceneSave { get; private set; }

    // Awake runs before any Start(), so Instance is ready before coins need it
    private void Awake()
    {
        // If a GameManager already exists, remove this extra component
        // (only the script, not the whole Player)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        // Otherwise, this is the one and only GameManager
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }


    // Health Related

    public void RemoveHealth(int amount)
    {
        Health -= amount;
        Debug.Log("Health: " + Health);
    }

    public void InstaKill()
    {
        Health = 0;
    }

    public void ResetHealth(int amount)
    {
        Health = amount;
    }


    // Score Related
    public void AddScore(int amount)
    {
        if (isScoreBoostActive)
        {
            amount *= 2; // Double the score if the boost is active
        }
        Score += amount;


        // Print the new total to the Console window
        Debug.Log("Score: " + Score);
    }

    // Called just before we leave the level
    public void SaveFinalScore()
    {
        FinalScore = Score;
    }
    public void ActivateScoreBoost()
    {
        isScoreBoostActive = true;
    }
    public void DeactivateScoreBoost()
    {
        isScoreBoostActive = false;
    }


    // Priogress Saving Related
    public void LevelReload()
    {
        // Reset the score and health to their saved values
        Score = FinalScore;
        Instance.ResetHealth(3);
        // Load the saved scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // I love google.
    }

}