using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGunManager : MonoBehaviour
{
    public List<GunBehavior> GunBehaviorList;
    public List<GameObject> GunList;
    public GunBehavior currentGun;
    public GameObject currentGunObject;
    [SerializeField] private GameObject gunMenu;
    [SerializeField] private TextMeshProUGUI gun1, gun2, gun3;
    [SerializeField] private Color currentGunColor, textColor;
    private float timer, gunSwitchDelay = 1.5f;
    void Start()
    {
        gun1.text = GunList[0].name;
        gun2.text = GunList[1].name;
        gun3.text = GunList[2].name;
        foreach (GameObject gun in GunList)
        {
            gun.SetActive(false);
        }
        SetActiveGun(GunBehaviorList[0], GunList[0]);
        currentGunObject.SetActive(true);
        gun1.color = currentGunColor;
        textColor = gun1.color;
        textColor.a = 0.5f;
        gun2.color = textColor;
        gun3.color = textColor;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2) gunMenu.SetActive(false);
        if (timer > gunSwitchDelay && currentGun.reload == null)
        {
            if (Input.GetKey("1") && currentGunObject != GunList[0])
            {
                gunMenu.SetActive(true);
                timer = 0;
                currentGunObject.SetActive(false);
                SetActiveGun(GunBehaviorList[0], GunList[0]);
                currentGunObject.SetActive(true);
                gun1.color = currentGunColor;
                gun2.color = textColor;
                gun3.color = textColor;
            }
            else if (Input.GetKey("2") && currentGunObject != GunList[1])
            {
                gunMenu.SetActive(true);
                timer = 0;
                currentGunObject.SetActive(false);
                SetActiveGun(GunBehaviorList[1], GunList[1]);
                currentGunObject.SetActive(true);
                gun2.color = currentGunColor;
                gun1.color = textColor;
                gun3.color = textColor;

            }
            else if (Input.GetKey("3") && currentGunObject != GunList[2])
            {
                gunMenu.SetActive(true);
                timer = 0;
                currentGunObject.SetActive(false);
                SetActiveGun(GunBehaviorList[2], GunList[2]);
                currentGunObject.SetActive(true);
                gun3.color = currentGunColor;
                gun1.color = textColor;
                gun2.color = textColor;
                
            }
        }
    }
    public void SetActiveGun(GunBehavior gunBehavior, GameObject gunObject)
    {
        currentGun = gunBehavior;
        currentGunObject = gunObject;
    }
    public void InstakillSetActive()
    {
        StartCoroutine(InstakillActive(10));
    }
    public IEnumerator InstakillActive(float duration)
    {
        foreach (var _gun in GunBehaviorList)
        {
        _gun.gunDamage = 99999;
        }
        yield return new WaitForSeconds(duration);
        foreach (var _gun in GunBehaviorList)
        {
        _gun.gunDamage = _gun.baseGunDamage;
        }
    }
}
