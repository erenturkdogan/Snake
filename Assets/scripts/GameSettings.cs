using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public float moveSpeed = 5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişince kaybolmasın
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
