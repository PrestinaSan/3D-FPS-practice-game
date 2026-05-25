using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthScript : MonoBehaviour
{
    [SerializeField] private Slider healthSlider, shieldSlider;
    [SerializeField] private int maxHealth, naturalHealing;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject gameOver, canvas,enemySpawner;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private float regenCooldown, healingRate;
    private int health;
    private float timeSinceDamaged, timer;
    private int remainingShield = 0, shield = 3;
    public bool alive = true;

    void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        remainingShield = 0;
        shieldSlider.maxValue = shield;
        shieldSlider.value = remainingShield;
        gameOver.gameObject.SetActive(false);
    }

    void Update()
    {
        timeSinceDamaged += Time.deltaTime;
        timer += Time.deltaTime;
        if (timeSinceDamaged >= regenCooldown && timer >= healingRate &&health <= maxHealth)
        {
            Heal(naturalHealing);
            timer = 0;
        }
    }
    public void TakeDamage(int damage)
    {
        if (remainingShield > 0)
        {
            remainingShield--;
            shieldSlider.value = remainingShield;
            return;
        }
        health -= damage;
        healthSlider.value = health;
        timeSinceDamaged = 0;
        if (health <= 0)
        {
            alive = false;
            foreach (Transform child in playerCamera.transform)
            {
                 child.gameObject.SetActive(false);
            }
            foreach (Transform child in canvas.transform)
            {
                child.gameObject.SetActive(false);
            }
            gameObject.transform.position = new Vector3(0, 50, 0);
            enemySpawner.SetActive(false);
            gameOver.SetActive(true);
            ScoreManager scoreManager = gameObject.GetComponent<ScoreManager>();
            finalScore.text = "Final Score: " + scoreManager.GetScore().ToString();
        }
    }
    public void Heal(int amount)
    {
        health += amount;
        healthSlider.value = health;
        if (health > maxHealth) health = maxHealth;
    }

    public void SetShield(int amount)
    {
        remainingShield = amount;
        shieldSlider.value = remainingShield;
    }
}
