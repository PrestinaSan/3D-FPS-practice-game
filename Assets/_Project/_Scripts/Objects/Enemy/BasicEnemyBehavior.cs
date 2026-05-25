using System.Collections;
using UnityEditor;
using UnityEngine;

public class BasicEnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private AudioSource proximityAudio;
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private int pointsWorth, attackDamage;
    [SerializeField] private float attackRate;
    [SerializeField] private int enemyHP;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] AudioClip deathSound;
    private int scorePerLevel = 30, hpIncrease = 0;
    private float timer, _pitch;
    private bool soundPlayed = false;
    public bool enemyAlive = true;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        scoreManager = player.gameObject.GetComponent<ScoreManager>();
        if (scoreManager.GetScore() >= (hpIncrease + 1) * scorePerLevel)
        {
            hpIncrease++;
        }
        enemyHP += hpIncrease;
        _pitch = Random.Range(0.5f, 2f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Vector3.Distance(gameObject.transform.position, player.position) <= 1)
        {
            if (timer >= attackRate)
            {
                PlayerHealthScript playerHP = player.GetComponent<PlayerHealthScript>();
                playerHP.TakeDamage(attackDamage);
                timer = 0;
            }
        }
        if (Vector3.Distance(gameObject.transform.position, player.position) <= 5)
        {
            if (soundPlayed == false)
            {
                proximityAudio.pitch = _pitch;
                proximityAudio.Play();
            soundPlayed = true;
            }
        }
    }
    public void OnDeathScore()
    {
        scoreManager.UpdateScore(pointsWorth);
    }

    public void OnDeathSpawnLoot()
    {
        int random = Random.Range(1, 15);
        if (random <= 1) {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }
    }

    public void EnemyTakeDamage(int damage)
    {
        enemyHP -= damage;
        StartCoroutine(DamageIndicator());
        if (enemyHP <= 0)
        {
            enemyAlive = false;
            OnDeathScore();
            OnDeathSpawnLoot();
            Destroy(gameObject);
            DestroyAfterAudio();
        }
    }
    IEnumerator DamageIndicator()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Color baseColor = renderer.material.color;
        renderer.material.color = new Color(190, 0, 0, 255);
        yield return new WaitForSeconds(0.05f);
        renderer.material.color = baseColor;
    }
    private void DestroyAfterAudio()
    {
        GameObject obj = new GameObject("Enemy Death");
        obj.transform.position = transform.position;
        AudioSource audio = obj.AddComponent<AudioSource>();
        audio.clip = deathSound;
        audio.pitch = _pitch;
        audio.Play();
        Destroy(obj, audio.clip.length);
    }
}
