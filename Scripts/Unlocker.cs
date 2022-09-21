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
        // Binanýn kilidi açýlacaksa:
        if(progress >= moneyToUnl)
        {
            CancelInvoke("MoneyGetTween");
            spawner.SetActive(true);
            saveCtrl.Buy();
            transform.gameObject.SetActive(false);
            
            // Eðer upgrade alaný açýlacaksa atanmýþ upgrade objelerinin aktive edilmesi.
            if (upgrade)
            {
                upgradeUp.SetActive(true);
                upgradeDown.SetActive(true);
            }
        }

        // Girildiðinde ve daha önce invoke edilmediyse para çekmeyi 0.12f aralýkla yapmasý.
        if (enter && !invoked)
        { 
            InvokeRepeating("MoneyGetTween", 0, 0.12f);
            invoked = true;
        }

        // Çýktýðýnda invoke'un cancellanmasý ve invoke'un kaldýrýldýðýnýn belirtilmesi.
        if (exit)
        {
            CancelInvoke("MoneyGetTween");
            invoked = false;
        }

        // Text'le deðerlerin yazdýrýlmasý.
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
            // Eðer progress kilidin açýlmasý için gerekli paradan büyük eþitse döngüyü sonlandýrýyor.
            if (progress >= moneyToUnl)
            {
                break;
            }

            if ((player.transform.GetChild(0).transform.GetChild(i).gameObject.activeSelf) && 100 <= manager.money )
            {
                player.transform.GetChild(0).transform.GetChild(i).transform.DOMove(building.transform.position, duration: 0.1f, snapping: false).SetEase(Ease.InQuart).OnComplete(() =>
                {
                    // Oyuncunun sýrtýndaki paranýn deaktive edilmesi ve parent'ýnýn null atanmasý.
                    player.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                    player.transform.GetChild(0).transform.GetChild(i).transform.parent = null;

                    // Para yok olduðundan yAxis'in deðerinin azaltýlmasý.
                    player.GetComponent<PlayerControls>().yAxis -= 0.25f;

                    // Animasyonun para yatýrýlýrken de pozisyonun ayarlanmasý ve aktive edilmesi.
                    anim.transform.position = building.transform.position;
                    anim.SetActive(true);

                    // Para yatýrýldýðýndan dolayý GameManager'da tutulan paranýn azaltýlmasý ve progress'in arttýrýlmasý.
                    manager.money -= 100;
                    progress += 100;
                });
                break;
            }
        }
    }
}
