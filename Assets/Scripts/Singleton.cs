using UnityEngine;

public class Singleton : MonoBehaviour
{
    private void Awake()
    {
        int numberOfSingleton = FindObjectsOfType<Singleton>().Length;
        if (numberOfSingleton > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
