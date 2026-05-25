using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    //public GunBehavior playerGun;
    public int ammo, totalAmmo, initialAmmo, maxAmmo, gunDamage, baseGunDamage;
    public Coroutine reload;
    private float time = 100f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private bool addBulletSpread;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private TextMeshProUGUI bulletText, reloadText;
    [SerializeField] private AudioSource shootingSound;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private ParticleSystem shootingParticleSystem;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private bool holdable, isShotgun;
    [SerializeField] private Camera playerCamera;



    void Start()
    {
        GunInitualizer();
        reloadText.text = "";
    }

    void Update()
    {
        GunInput();
    }

    public void GunInitualizer()
    {
        ammo = maxAmmo;
        initialAmmo = totalAmmo;
        baseGunDamage = gunDamage;
    }

    public void GunInput()
    {
        time += Time.deltaTime;
        bulletText.text = ammo.ToString() + "/" + totalAmmo.ToString();
        if (reload != null && isShotgun && Input.GetMouseButtonDown(0) && ammo > 0)
        {
            StopCoroutine(reload);
            reload = null;
            reloadText.text = "";
        }
        if (reload != null) return;
        //hold to fire
        if (holdable && Input.GetMouseButton(0) && !isShotgun)
        {

            if (ammo > 0)
            {
                float shootingDelay = timeBetweenShots;
                if (time >= shootingDelay)
                {
                    Shoot();
                    ammo--;
                    time = 0;
                }
            }
            else
            {
                reload = StartCoroutine(Reload());
            }
        }

        //click to fire
        if (!holdable && Input.GetMouseButtonDown(0) && !isShotgun && reload == null)
        {
            if (ammo > 0)
            {
                float shootingDelay = timeBetweenShots;
                if (time >= shootingDelay)
                {
                    Shoot();
                    ammo--;
                    time = 0;
                }
            }
            else
            {
                reload = StartCoroutine(Reload());
            }
        }
        //shotgun fire
        if (!holdable && Input.GetMouseButtonDown(0) && isShotgun)
        {
            if (ammo > 0)
            {
                float shootingDelay = timeBetweenShots;
                int shotgunBullets = 10;
                if (time >= shootingDelay)
                {
                    for (int i = 0; i < shotgunBullets; i++)
                    {
                        Shoot();
                    }
                    ammo--;
                    time = 0;
                }
            }
            else
            {
                    reload = StartCoroutine(ShotgunReload());
            }
        }
        if (Input.GetKey("r") && !isShotgun)
        {
            reload = StartCoroutine(Reload());
        }
        if (Input.GetKey("r") && isShotgun)
        {
            reload = StartCoroutine(ShotgunReload());
        }
    }

    private IEnumerator Reload()
    {
        if (ammo == maxAmmo) yield break;
        if (totalAmmo <= 0) yield break;
        reloadText.text = "Reloading...";
        yield return new WaitForSeconds(1.5f);
        reloadText.text = "";
        int ammoToReload = maxAmmo - ammo;
        if (totalAmmo >= ammoToReload)
        {
            ammo += ammoToReload;
            totalAmmo -= ammoToReload;
        }
        else
        {
            ammo += totalAmmo;
            totalAmmo -= totalAmmo;
        }
        reload = null;
    }
    private IEnumerator ShotgunReload()
    {
        if (ammo == maxAmmo) yield break;
        if (totalAmmo <= 0) yield break;

        reloadText.text = "Reloading...";

        while (ammo < maxAmmo && totalAmmo > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (ammo > 0 && time >= timeBetweenShots)
                {
                    Shoot();
                    ammo--;
                    time = 0;
                }
                break;
            }
            yield return new WaitForSeconds(0.5f);
            ammo++;
            totalAmmo--;
        }

        reloadText.text = "";
        reload = null;
    }
    private void Shoot()
    {
        Vector3 direction = GetDirection();
        shootingParticleSystem.Play();
        shootingSound.pitch = Random.Range(0.9f, 1.1f);
        shootingSound.Play();
        if (Physics.Raycast(playerCamera.transform.position, direction, out RaycastHit hit, float.MaxValue, mask))
        {
            TrailRenderer trail = Instantiate(bulletTrail, gunBarrel.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));



            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                GameObject enemy = hit.collider.gameObject;
                BasicEnemyBehavior enemyBehavior = enemy.GetComponent<BasicEnemyBehavior>();
                if (enemyBehavior.enemyAlive)
                {
                enemyBehavior.EnemyTakeDamage(gunDamage);
                }
            }
        }
        else
        {
            TrailRenderer trail = Instantiate(bulletTrail, gunBarrel.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, gunBarrel.position + GetDirection() * 100, Vector3.zero, false));
        }


    }

    private Vector3 GetDirection()
    {
        Vector3 direction = playerCamera.transform.forward;

        if (addBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-bulletSpread.x, bulletSpread.x),
                Random.Range(-bulletSpread.y, bulletSpread.y),
                Random.Range(-bulletSpread.z, bulletSpread.z)
                );
            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, bool madeImpact)
    {
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));
            remainingDistance -= bulletSpeed * Time.deltaTime;

            yield return null;
        }
        // Animator stuff here
        trail.transform.position = hitPoint;

        if (madeImpact)
        {
        Instantiate(impactParticleSystem, hitPoint, Quaternion.LookRotation(hitNormal));
        }

        Destroy(trail.gameObject, trail.time);
    }
}