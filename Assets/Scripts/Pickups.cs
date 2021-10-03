using UnityEngine;

public class Pickups : MonoBehaviour
{
    private void Awake()
    {
        int numberOfPickups = FindObjectsOfType<Pickups>().Length;
        if (numberOfPickups > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
