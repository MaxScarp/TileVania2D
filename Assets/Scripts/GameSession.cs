using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerPoints = 0;

    [SerializeField] TextMeshProUGUI textLives;
    [SerializeField] TextMeshProUGUI textPoints;

    int firstLevel = 0;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        textLives.text = playerLives.ToString();
        textPoints.text = playerPoints.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives <= 1)
        {
            ResetGameSession();
        }
        else
        {
            TakeLife();
        }
    }

    private void TakeLife()
    {
        playerLives--;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        UpdateUI();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(firstLevel);
        Destroy(FindObjectOfType<Pickups>().gameObject);
        Destroy(FindObjectOfType<Singleton>().gameObject);
    }

    public void AddPoint(int points)
    {
        playerPoints += points;
        UpdateUI();
    }
}
