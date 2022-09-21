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
        // E�er progress b�lge a��lmas� i�in gereken paradan y�ksek veya e�itse:
        if (progress >= moneyToUnl)
        {
            // E�er GameManager'daki expansionLv, expansions objesinin �ocuk say�s�ndan az veya e�itse
            if (manager.expansionLv <= expansions.transform.childCount)
            {
                // expansionLv'a kadar olan b�t�n expansions objesinin �ocuklar�n�n aktive dilmesi.
                for(int i =0; i<=(manager.expansionLv); i++)
                {
                    expansions.transform.GetChild(i).gameObject.SetActive(true);
                }

                // E�er expansionLv bir artt�r�ld���nda expansions objesinin �ocuk say�s�ndan k���k veya e�itse expansionLv'�n bir artt�r�lmas�.
                if ((manager.expansionLv+1) <= expansions.transform.childCount)
                {
                    manager.expansionLv += 1;
                }
                
                // E�er expansionLv, expanders'�n �ocuk say�s�ndan azsa
                if(manager.expansionLv < expanders.transform.childCount)
                {
                    // expanders objesinde expansionLv indisli objenin aktive edilmesi.
                    expanders.transform.GetChild(manager.expansionLv).gameObject.SetActive(true);
                }

                transform.gameObject.SetActive(false);
            }
        }

        // Girildi�inde ve daha �nce invoke edilmediyse para �ekmeyi 0.05f aral�kla yapmas�.
        if (enter && !invoked)
        {
            InvokeRepeating("GetMoney", 0, 0.05f);
            invoked = true;
        }

        // ��kt���nda invoke'un cancellanmas�
        if (exit)
        {
            CancelInvoke("GetMoney");
            invoked = false;
        }

        // Statik olarak b�lgeyi geni�letmek i�in gerekli paran�n yazd�r�lmas� ve dinamik olarak progressin b�lgeyi a�mak i�in gerekli paraya oran�n�n g�sterilmesi.
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
                // Oyuncunun s�rt�ndaki paran�n deaktive edilmesi ve parent'�n�n null atanmas�.
                player.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                player.transform.GetChild(0).transform.GetChild(i).transform.parent = null;

                // Para yok oldu�undan yAxis'in de�erinin azalt�lmas�.
                player.GetComponent<PlayerControls>().yAxis -= 0.25f;

                // Para yat�r�ld���ndan dolay� GameManager'da tutulan paran�n azalt�lmas� ve progress'in artt�r�lmas�.
                manager.money -= 100;
                progress += 100;

                break;
            }
        }
    }
}
