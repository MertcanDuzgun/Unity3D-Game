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
        // Girildiyse ve a��k de�ilse
        if (enter && !open)
        {
            // Upgrade'i i�eren panelin ve panel arka plan�n�n aktive edilmesi.
            panel.SetActive(true);
            panelbg.SetActive(true);

            // Dinamik olarak de�erlerin textlere yaz�lmas�.
            up1Lv.text = "Lv" + mng.msLv.ToString();
            up1Cost.text = "Cost: " + mng.msCost.ToString();
            up2Lv.text = "Lv" + mng.capLv.ToString();
            up2Cost.text = "Cost: " + mng.capCost.ToString();
            up3Lv.text = "Lv" + mng.dcIncLv.ToString();
            up3Cost.text = "Cost: " + mng.dcIncCost.ToString();
            up4Lv.text = "Lv" + mng.csLv.ToString();
            up4Cost.text = "Cost: " + mng.csCost.ToString();
        }

        // Alandan ��k�ld�ysa upgrade aray�z�n�n kapat�lmas� ve a��k olmad���n�n belirtilmesi.
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

    // ms alan�ndaki butona t�kland���nda paran�n msCost kadar azalt�lmas� ve msLv'�n 1 artt�r�lmas�.
    public void clickUpgrade1()
    {
        if(mng.money >= mng.msCost)
        {
            GetMoney(mng.msCost);
            mng.msLv += 1;
        }
    }

    // cap alan�ndaki butona t�kland���nda paran�n capCost kadar azalt�lmas� ve capLv'�n 1 artt�r�lmas�.
    public void clickUpgrade2()
    {
        if (mng.money >= mng.capCost)
        {
            GetMoney(mng.capCost);
            mng.capLv += 1;
        }
    }

    // dcInc alan�ndaki butona t�kland���nda paran�n dcIncCost kadar azalt�lmas� ve dcIncLv'�n 1 artt�r�lmas�.
    public void clickUpgrade3()
    {
        if (mng.money >= mng.dcIncCost)
        {
            GetMoney(mng.dcIncCost);
            mng.dcIncLv += 1;
        }
    }

    // cs alan�ndaki butona t�kland���nda paran�n csCost kadar azalt�lmas� ve csLv'�n 1 artt�r�lmas�.
    public void clickUpgrade4()
    {
        if (mng.money >= mng.csCost)
        {
            GetMoney(mng.csCost);
            mng.csLv += 1;
        }
    }

    // Hem kod i�erisinde gereken yer de hem de aray�zdeki Close butonuna t�kland���nda panelin ve panel arka plan�n�n kapat�lmas�.
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
                // Oyuncunun s�rt�ndaki paran�n deaktive edilmesi ve parent'�n�n null atanmas�.
                player.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                player.transform.GetChild(0).transform.GetChild(i).transform.parent = null;

                // Para yok oldu�undan yAxis'in de�erinin azalt�lmas�.
                player.GetComponent<PlayerControls>().yAxis -= 0.25f;

                // Para yat�r�ld���ndan dolay� GameManager'da tutulan paran�n azalt�lmas� ve progress'in artt�r�lmas�.
                mng.money -= 100;
            }
        }
    }
}
