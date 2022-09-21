using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrader : MonoBehaviour
{
    public GameObject panel, panelbg;
    public TextMeshProUGUI up1Lv, up1Cost, up2Lv, up2Cost, up3Lv, up3Cost, up4Lv, up4Cost;
    private bool enter, exit, open= false;
    public GameObject player;
    public GameManager mng;

    void Update()
    {
        // Girildiyse ve açýk deðilse
        if (enter && !open)
        {
            // Upgrade'i içeren panelin ve panel arka planýnýn aktive edilmesi.
            panel.SetActive(true);
            panelbg.SetActive(true);

            // Dinamik olarak deðerlerin textlere yazýlmasý.
            up1Lv.text = "Lv" + mng.msLv.ToString();
            up1Cost.text = "Cost: " + mng.msCost.ToString();
            up2Lv.text = "Lv" + mng.capLv.ToString();
            up2Cost.text = "Cost: " + mng.capCost.ToString();
            up3Lv.text = "Lv" + mng.dcIncLv.ToString();
            up3Cost.text = "Cost: " + mng.dcIncCost.ToString();
            up4Lv.text = "Lv" + mng.csLv.ToString();
            up4Cost.text = "Cost: " + mng.csCost.ToString();
        }

        // Alandan çýkýldýysa upgrade arayüzünün kapatýlmasý ve açýk olmadýðýnýn belirtilmesi.
        if(exit)
        {
            clickClose();
            open = false;
        }
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
            open = false;

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

    // ms alanýndaki butona týklandýðýnda paranýn msCost kadar azaltýlmasý ve msLv'ýn 1 arttýrýlmasý.
    public void clickUpgrade1()
    {
        if(mng.money >= mng.msCost)
        {
            GetMoney(mng.msCost);
            mng.msLv += 1;
        }
    }

    // cap alanýndaki butona týklandýðýnda paranýn capCost kadar azaltýlmasý ve capLv'ýn 1 arttýrýlmasý.
    public void clickUpgrade2()
    {
        if (mng.money >= mng.capCost)
        {
            GetMoney(mng.capCost);
            mng.capLv += 1;
        }
    }

    // dcInc alanýndaki butona týklandýðýnda paranýn dcIncCost kadar azaltýlmasý ve dcIncLv'ýn 1 arttýrýlmasý.
    public void clickUpgrade3()
    {
        if (mng.money >= mng.dcIncCost)
        {
            GetMoney(mng.dcIncCost);
            mng.dcIncLv += 1;
        }
    }

    // cs alanýndaki butona týklandýðýnda paranýn csCost kadar azaltýlmasý ve csLv'ýn 1 arttýrýlmasý.
    public void clickUpgrade4()
    {
        if (mng.money >= mng.csCost)
        {
            GetMoney(mng.csCost);
            mng.csLv += 1;
        }
    }

    // Hem kod içerisinde gereken yer de hem de arayüzdeki Close butonuna týklandýðýnda panelin ve panel arka planýnýn kapatýlmasý.
    public void clickClose()
    {
        open = true;
        panel.SetActive(false);
        panelbg.SetActive(false);
    }

    private void GetMoney(float x)
    {
        for (int i = ((int)x/100) - 1; i >= 0; i--)
        {
            if ((player.transform.GetChild(0).transform.GetChild(i).gameObject.activeSelf) && 100 <= mng.money)
            {
                // Oyuncunun sýrtýndaki paranýn deaktive edilmesi ve parent'ýnýn null atanmasý.
                player.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                player.transform.GetChild(0).transform.GetChild(i).transform.parent = null;

                // Para yok olduðundan yAxis'in deðerinin azaltýlmasý.
                player.GetComponent<PlayerControls>().yAxis -= 0.25f;

                // Para yatýrýldýðýndan dolayý GameManager'da tutulan paranýn azaltýlmasý ve progress'in arttýrýlmasý.
                mng.money -= 100;
            }
        }
    }
}
