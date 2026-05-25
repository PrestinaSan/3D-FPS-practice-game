using UnityEngine;

public class EnemySpawnWarning : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float beepRate;
    [SerializeField] private Color initialColor;
    [SerializeField] private ScoreManager scoreManager;
    private float spawnTime = 0f, beepTime = 0f, speedMultiplier = 1f;
    private int scorePerLevel = 10, level = 0;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        scoreManager = player.GetComponent<ScoreManager>();
        Color currentColor = objectRenderer.material.color;
        currentColor.a = 0.75f;
        objectRenderer.material.color = currentColor;
        initialColor = objectRenderer.material.color;
        if (scoreManager.GetScore() >= (level + 1) * scorePerLevel)
        {
            level++;
            speedMultiplier += 0.1f;
        }
    }

    void Update()
    {
        spawnTime += Time.deltaTime;
        beepTime += Time.deltaTime;
        if (beepTime >= beepRate)
        {
            Color currentColor = objectRenderer.material.color;
            if (currentColor == initialColor)
            {
                currentColor = Color.yellow;
                currentColor.a = 0.75f;
                objectRenderer.material.color = currentColor;
            }
            else
            {
                currentColor = initialColor;
                objectRenderer.material.color = currentColor;
            }
            beepTime = 0f;
        }
        if (spawnTime >= spawnTimer)
        {
            GameObject enemy = Instantiate(enemyPrefab, gameObject.transform.position, Quaternion.identity);
            EnemyMovementBehavior enemyMovement = enemy.GetComponent<EnemyMovementBehavior>();
            enemyMovement.ModifyAgentSpeed(speedMultiplier);
            Destroy(gameObject);
        }
    }
}
