using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomLootBehavior : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LootDropPickupText lootText;
    [SerializeField] private List<string> PowerUps;
    [SerializeField] private string currentPowerUp;
    [SerializeField] private TextMeshPro powerUpText;
    [SerializeField] private float rotateSpeed, warningTime, beepRate = 0.25f;
    private float time, lifeTime = 15, beepTime = 0;
    private Camera playerCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject UIManager = GameObject.Find("UIManager");
        lootText = UIManager.GetComponent<LootDropPickupText>();
        currentPowerUp = PowerUps[Random.Range(0, PowerUps.Count)];
        powerUpText.text = currentPowerUp;
        playerCamera = player.GetChild(0).GetComponent<Camera>();
    }

    void Update()
    {
        powerUpText.gameObject.transform.rotation = playerCamera.transform.rotation;
        time += Time.deltaTime;
        if (time > warningTime)
        {
            Renderer objectRenderer = gameObject.GetComponent<Renderer>();
            beepTime += Time.deltaTime;
            if (beepTime >= beepRate)
            {
                Color currentColor = objectRenderer.material.color;
                if (currentColor.a == 1f)
                {
                    currentColor.a = 0.5f;
                    objectRenderer.material.color = currentColor;
                }
                else
                {
                    currentColor.a = 1f;
                    objectRenderer.material.color = currentColor;
                }
                beepTime = 0f;
            }
        }
        if (time >= lifeTime)
        {
            Destroy(gameObject);
        }
        gameObject.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        if (Vector3.Distance(gameObject.transform.position, player.position) <= 1)
        {
            if (currentPowerUp == "Max Ammo")
                MaxAmmoPickUp();
            else if (currentPowerUp == "Shield")
                ShieldPickUp();
            else if (currentPowerUp == "Instakill")
            {
                InstakillPickUp();
            }
        }
    }
    public void MaxAmmoPickUp()
    {
        PlayerGunManager playerGun = player.gameObject.GetComponent<PlayerGunManager>();
        foreach (var gun in playerGun.GunBehaviorList)
        {
            gun.ammo = gun.maxAmmo;
            gun.totalAmmo = gun.initialAmmo;
            gun.reload = null;
            lootText.StartCoroutine(lootText.MaxAmmo());
            Destroy(gameObject);
        }
    }
    public void ShieldPickUp()
    {
        PlayerHealthScript playerHealth = player.gameObject.GetComponent<PlayerHealthScript>();
        playerHealth.SetShield(3);
        lootText.StartCoroutine(lootText.Shield());
        Destroy(gameObject);
    }
    public void InstakillPickUp()
    {
        PlayerGunManager playerGun = player.gameObject.GetComponent<PlayerGunManager>();
        playerGun.InstakillSetActive();
        lootText.StartCoroutine(lootText.Instakill());
        Destroy(gameObject);
    }
}
