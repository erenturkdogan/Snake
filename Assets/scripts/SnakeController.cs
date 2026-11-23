using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 currentDirection = Vector3.right;
    private Vector3 inputDirection = Vector3.right;

    [Header("Tail Settings")]
    public GameObject tailPrefab;
    public float distanceBetweenSegments = 0.5f;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    private List<Transform> tail = new List<Transform>();
    private List<Vector3> positionHistory = new List<Vector3>();
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;
        HandleInput();
    }

    void FixedUpdate()
    {
        if (isGameOver) return;
        Move();
        MoveTail();
    }

    void HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && currentDirection != Vector3.down)
        {
            inputDirection = Vector3.up;
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && currentDirection != Vector3.up)
        {
            inputDirection = Vector3.down;
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentDirection != Vector3.right)
        {
            inputDirection = Vector3.left;
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentDirection != Vector3.left)
        {
            inputDirection = Vector3.right;
        }
    }

    void Move()
    {
        currentDirection = inputDirection;
        transform.position += currentDirection * moveSpeed * Time.fixedDeltaTime;
        positionHistory.Insert(0, transform.position);

        RotateHead(currentDirection);
    }

    void RotateHead(Vector3 direction)
    {
        if (direction == Vector3.up)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (direction == Vector3.right)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (direction == Vector3.down)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction == Vector3.left)
            transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    void MoveTail()
    {
        float spacing = distanceBetweenSegments / (moveSpeed * Time.fixedDeltaTime);
        for (int i = 0; i < tail.Count; i++)
        {
            int index = Mathf.RoundToInt((i + 1) * spacing);
            if (index < positionHistory.Count)
            {
                tail[i].position = positionHistory[index];
            }
        }

        int maxHistory = Mathf.RoundToInt((tail.Count + 1) * spacing);
        if (positionHistory.Count > maxHistory)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    public void Grow()
    {
        Vector3 spawnPos = tail.Count == 0 ? transform.position : tail[tail.Count - 1].position;
        GameObject newTail = Instantiate(tailPrefab, spawnPos, Quaternion.identity);
        tail.Add(newTail.transform);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
            Destroy(other.gameObject);
            ScoreManager.Instance.AddScore(1);
            FindObjectOfType<FoodSpawner>()?.SpawnFood();
        }
        else if (other.CompareTag("Tail") || other.CompareTag("Wall"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // Restart fonksiyonu — UI butonundan çağrılabilir
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
{
    Application.Quit();

    // Unity editöründe test için
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
}

}
