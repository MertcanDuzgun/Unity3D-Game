using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Unlocker : MonoBehaviour
{
    public int moneyToUnl, progress = 0;
    public GameObject spawner;
    public GameObject upgradeUp, upgradeDown;
    public GameObject player; 
    public GameObject building;
    public GameObject anim;
    public GameManager manager;
    private bool enter, exit , invoked= false;
    public bool upgrade = false;
    public TextMeshProUGUI stat, dyna, holo;
    [SerializeField] SaveControl saveCtrl;

    void Update()
    {
        // Binan�n kilidi a��lacaksa:
        if(progress >= moneyToUnl)
        {
            CancelInvoke("MoneyGetTween");
            spawner.SetActive(true);
            saveCtrl.Buy();
            transform.gameObject.SetActive(false);
            
            // E�er upgrade alan� a��lacaksa atanm�� upgrade objelerinin aktive edilmesi.
            if (upgrade)
            {
                upgradeUp.SetActive(true);
                upgradeDown.SetActive(true);
            }
        }

        // Girildi�inde ve daha �nce invoke edilmediyse para �ekmeyi 0.12f aral�kla yapmas�.
        if (enter && !invoked)
        { 
            InvokeRepeating("MoneyGetTween", 0, 0.12f);
            invoked = true;
        }

        // ��kt���nda invoke'un cancellanmas� ve invoke'un kald�r�ld���n�n belirtilmesi.
        if (exit)
        {
            CancelInvoke("MoneyGetTween");
            invoked = false;
        }

        // Text'le de�erlerin yazd�r�lmas�.
        holo.text = "To Unlock: " + moneyToUnl.ToString();
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

    public void MoneyGetTween()
    {
        for (int i = (player.transform.GetChild(0).transform.childCount - 1); i > -1; i--)
        {
            // E�er progress kilidin a��lmas� i�in gerekli paradan b�y�k e�itse d�ng�y� sonland�r�yor.
            if (progress >= moneyToUnl)
            {
                break;
            }

            if ((player.transform.GetChild(0).transform.GetChild(i).gameObject.activeSelf) && 100 <= manager.money )
            {
                player.transform.GetChild(0).transform.GetChild(i).transform.DOMove(building.transform.position, duration: 0.1f, snapping: false).SetEase(Ease.InQuart).OnComplete(() =>
                {
                    // Oyuncunun s�rt�ndaki paran�n deaktive edilmesi ve parent'�n�n null atanmas�.
                    player.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                    player.transform.GetChild(0).transform.GetChild(i).transform.parent = null;

                    // Para yok oldu�undan yAxis'in de�erinin azalt�lmas�.
                    player.GetComponent<PlayerControls>().yAxis -= 0.25f;

                    // Animasyonun para yat�r�l�rken de pozisyonun ayarlanmas� ve aktive edilmesi.
                    anim.transform.position = building.transform.position;
                    anim.SetActive(true);

                    // Para yat�r�ld���ndan dolay� GameManager'da tutulan paran�n azalt�lmas� ve progress'in artt�r�lmas�.
                    manager.money -= 100;
                    progress += 100;
                });
                break;
            }
        }
    }
}
