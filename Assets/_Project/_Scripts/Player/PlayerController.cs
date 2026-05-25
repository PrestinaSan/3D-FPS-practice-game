using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float staminaRegenDelay;
    [SerializeField] private PlayerHealthScript healthScript;
    [SerializeField] private PlayerGunManager playerGun;
    public float basePlayerSpeed, currentPlayerSpeed, playerStamina, staminaDrain, staminaGain, maxStamina, speedMultiplier;
    private float timer;

    void Start()
    {
        playerStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = playerStamina;
    }

    void Update()
    {
        if (playerGun.currentGunObject.name == "Pistol") basePlayerSpeed = 9;
        else if (playerGun.currentGunObject.name == "Rifle") basePlayerSpeed = 7;
        else if (playerGun.currentGunObject.name == "Shotgun") basePlayerSpeed = 7;
        PlayerMovement();
        if (healthScript.alive == true) transform.position = new Vector3(transform.position.x,1,transform.position.z);
    }

    public void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 _move = this.transform.right * x + this.transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift) && playerStamina > 0)
        {
            currentPlayerSpeed = basePlayerSpeed * speedMultiplier;
            playerStamina -= Time.deltaTime * staminaDrain;
            staminaSlider.value = playerStamina;
            timer = 0;
        }
        else
        {
            currentPlayerSpeed = basePlayerSpeed;
            timer += Time.deltaTime;
            if (timer >= staminaRegenDelay && playerStamina < maxStamina)
            {
                playerStamina += Time.deltaTime * staminaGain;
                staminaSlider.value = playerStamina;
            }
        }

            characterController.Move(_move * currentPlayerSpeed * Time.deltaTime);
    }
}