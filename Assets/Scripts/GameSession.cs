using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;

    int firstLevel = 0;

    private void Awake()
    {
        int numberOfGameSession = FindObjectsOfType<GameSession>().Length;
        if (numberOfGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
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
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(firstLevel);
        Destroy(gameObject);
    }
}
