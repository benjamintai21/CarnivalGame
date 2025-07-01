using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DuckGame : MonoBehaviour
{
    private const string V = "Press button to start game";
    public static DuckGame Instance;
    public DuckSpawner duckSpawner1;
    public DuckSpawner duckSpawner2;
    public DuckSpawner duckSpawner3;


    public int score = 0;
    public int targetScore = 5;
    public TextMeshPro scoreText; // Drag a UI Text here in the Inspector

    public GameObject weaponObject;         // Assign your weapon (e.g. mallet or gun)
    public Transform weaponRestingPlace;

    private bool gameActive = false;

    void Awake()
    {
        Instance = this;
        scoreText.text = V;
    }


    public void StartDuckGame()
    {
        if (gameActive) return;

        duckSpawner1.SpawnDucks();
        duckSpawner2.SpawnDucks();
        duckSpawner3.SpawnDucks();

        score = 0;
        gameActive = true;


        Debug.Log("Game started!");
        scoreText.text = "Score: 0";
    }

    void EndDuckGame()
    {
        score = 0;
        gameActive = false;


        Debug.Log("Game Over! Score reached 5.");

        // Drop weapon onto table
        if (weaponObject != null && weaponRestingPlace != null)
        {
            weaponObject.transform.SetPositionAndRotation(
                weaponRestingPlace.position, weaponRestingPlace.rotation
            );
        }
        scoreText.text = "Game Ended! Press button to start new game";
    }


    public void DuckAddScore(int value)
    {
        if (!gameActive) return;

        score += value;
        DuckUpdateScoreText();
        if (score >= targetScore)
        {
            EndDuckGame();
        }
        
    }
    private void DuckUpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
