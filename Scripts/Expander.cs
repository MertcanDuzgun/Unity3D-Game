using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Expander : MonoBehaviour
{
    public int moneyToUnl, progress;
    public GameManager manager;
    private bool enter, exit, invoked = false;
    public TextMeshProUGUI stat, dyna;
    public GameObject expansions;
    public GameObject expanders;
    [SerializeField] GameObject player;


    void Update()
    {
        // Eðer progress bölge açýlmasý için gereken paradan yüksek veya eþitse:
        if (progress >= moneyToUnl)
        {
            // Eðer GameManager'daki expansionLv, expansions objesinin çocuk sayýsýndan az veya eþitse
            if (manager.expansionLv <= expansions.transform.childCount)
            {
                // expansionLv'a kadar olan bütün expansions objesinin çocuklarýnýn aktive dilmesi.
                for(int i =0; i<=(manager.expansionLv); i++)
                {
                    expansions.transform.GetChild(i).gameObject.SetActive(true);
                }

                // Eðer expansionLv bir arttýrýldýðýnda expansions objesinin çocuk sayýsýndan küçük veya eþitse expansionLv'ýn bir arttýrýlmasý.
                if ((manager.expansionLv+1) <= expansions.transform.childCount)
                {
                    manager.expansionLv += 1;
                }
                
                // Eðer expansionLv, expanders'ýn çocuk sayýsýndan azsa
                if(manager.expansionLv < expanders.transform.childCount)
                {
                    // expanders objesinde expansionLv indisli objenin aktive edilmesi.
                    expanders.transform.GetChild(manager.expansionLv).gameObject.SetActive(true);
                }

                transform.gameObject.SetActive(false);
            }
        }

        // Girildiðinde ve daha önce invoke edilmediyse para çekmeyi 0.05f aralýkla yapmasý.
        if (enter && !invoked)
        {
            InvokeRepeating("GetMoney", 0, 0.05f);
            invoked = true;
        }

        // Çýktýðýnda invoke'un cancellanmasý
        if (exit)
        {
            CancelInvoke("GetMoney");
            invoked = false;
        }

        // Statik olarak bölgeyi geniþletmek için gerekli paranýn yazdýrýlmasý ve dinamik olarak progressin bölgeyi açmak için gerekli paraya oranýnýn gösterilmesi.
        stat.text = "To Unlock: " + moneyToUnl.ToString();
        dyna.text = progress.ToString() + "/" + moneyToUnl.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControls ply))
        {
            if (!enter)
            {
                enter = true;
            }

            if (exit)
            {
                exit = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerControls ply))
        {
            if (!exit)
            {
                exit = true;
            }

            if (enter)
            {
                enter = false;
            }
        }
    }

    private void GetMoney()
    {
        for (int i = (player.transform.GetChild(0).transform.childCount - 1); i > -1; i--)
        {
            if (progress >= moneyToUnl)
            {
                break;
            }

            if ((player.transform.GetChild(0).transform.GetChild(i).gameObject.activeSelf) && 100 <= manager.money)
            {
                // Oyuncunun sýrtýndaki paranýn deaktive edilmesi ve parent'ýnýn null atanmasý.
                player.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                player.transform.GetChild(0).transform.GetChild(i).transform.parent = null;

                // Para yok olduðundan yAxis'in deðerinin azaltýlmasý.
                player.GetComponent<PlayerControls>().yAxis -= 0.25f;

                // Para yatýrýldýðýndan dolayý GameManager'da tutulan paranýn azaltýlmasý ve progress'in arttýrýlmasý.
                manager.money -= 100;
                progress += 100;

                break;
            }
        }
    }
}
