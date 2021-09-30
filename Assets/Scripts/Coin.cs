using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip collectedSound;

    [SerializeField] int scoreValue = 25;

    bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected)
        {
            collected = true;

            AudioSource.PlayClipAtPoint(collectedSound, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddPoint(scoreValue);
            Destroy(gameObject);
        }
    }
}
