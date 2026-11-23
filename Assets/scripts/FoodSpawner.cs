using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public float xRange = 8f;
    public float yRange = 4f;

    public void SpawnFood()
    {
        Vector2 spawnPos = new Vector2(
            Mathf.Round(Random.Range(-xRange, xRange)),
            Mathf.Round(Random.Range(-yRange, yRange))
        );
        Instantiate(foodPrefab, spawnPos, Quaternion.identity);
    }

    void Start()
    {
        SpawnFood(); // İlk başta 1 tane doğur
    }
}
