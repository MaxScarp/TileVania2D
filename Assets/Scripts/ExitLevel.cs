using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    Animator animator;
    Animator playerAnimator;
    Player player;

    [SerializeField] float secondsToWait = 3.5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        playerAnimator = collision.gameObject.GetComponent<Animator>();

        StartCoroutine(LevelExitLogic());
    }

    private IEnumerator LevelExitLogic()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        animator.SetTrigger("exit");
        playerAnimator.SetTrigger("draw");
        player.SetRunSpeed(0f);

        yield return new WaitForSeconds(secondsToWait);

        Destroy(FindObjectOfType<Pickups>().gameObject);
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
