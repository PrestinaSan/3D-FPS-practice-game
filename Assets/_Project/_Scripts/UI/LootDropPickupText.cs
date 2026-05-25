using System.Collections;
using TMPro;
using UnityEngine;

public class LootDropPickupText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lootUI;
    void Start()
    {
        lootUI.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    public IEnumerator MaxAmmo()
    {
        lootUI.gameObject.SetActive(true);
        lootUI.text = "Max Ammo";
        yield return new WaitForSeconds(3);
        lootUI.gameObject.SetActive(false);
    }
    public IEnumerator Shield()
    {
        lootUI.gameObject.SetActive(true);
        lootUI.text = "Shield";
        yield return new WaitForSeconds(3);
        lootUI.gameObject.SetActive(false);
    }
    public IEnumerator Instakill()
    {
        lootUI.gameObject.SetActive(true);
        lootUI.text = "Instakill";
        yield return new WaitForSeconds(3);
        lootUI.gameObject.SetActive(false);
    }
}
