using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public float money,maxMoney;
    public float maxMoneyO = 5000.0f;
    public int msLv,capLv,dcIncLv,csLv;
    public float msCost, csCost, capCost, dcIncCost, dcCost = 0f;
    public PlayerControls plCtrl;
    public TextMeshProUGUI moneyIndicator;
    public GameObject _player;
    public int expansionLv = 0;

    void Update()
    {
        // Paranýn maksimum deðeri aþmamasýný saðlamak için.
        if (money > maxMoney)
        {
            money = maxMoney;
        }

        // capLv'a göre playerSpeed'in yani oyuncu hareket hýzýnýn ve maliyetinin ayarlanmasý.
        switch (msLv)
        {
            case 0:
                plCtrl.playerSpeed = plCtrl.playerSpeedO * 1.0f;
                msCost = 2500.0f;
                break;
            case 1:
                plCtrl.playerSpeed = plCtrl.playerSpeedO * 1.25f;
                msCost = 7000.0f;
                break;
            case 2:
                plCtrl.playerSpeed = plCtrl.playerSpeedO * 1.5f;
                msCost = 15000.0f;
                break;
            case 3:
                plCtrl.playerSpeed = plCtrl.playerSpeedO * 1.75f;
                msCost = 20000.0f;
                break;
            case 4:
                plCtrl.playerSpeed = plCtrl.playerSpeedO * 2.0f;
                msCost = 27500.0f;
                break;
            case 5:
                plCtrl.playerSpeed = plCtrl.playerSpeedO * 2.5f;
                msCost = 999999.0f;
                break;
        }

        // capLv'a göre maxMoney'nin yani oyuncunun taþýyabileceði para kapasitesinin ve maliyetinin ayarlanmasý.
        switch (capLv)
        {
            case 0:
                maxMoney = maxMoneyO* 1.0f;
                capCost = maxMoney * 0.6f;
                break;
            case 1:
                maxMoney = maxMoneyO * 1.6f;
                capCost = maxMoney * 0.6f;
                break;
            case 2: 
                maxMoney = maxMoneyO * 2.6f;
                capCost = maxMoney * 0.6f;
                break;
            case 3: 
                maxMoney = maxMoneyO * 4.0f;
                capCost = maxMoney * 0.6f;
                break;
            case 4: 
                maxMoney = maxMoneyO * 7.6f;
                capCost = maxMoney * 0.6f;
                break;
            case 5:
                maxMoney = maxMoneyO * 15.0f;
                capCost = 999999.0f;
                break;
        }

        // Arayüzde sað üst kýsýmda paranýn gösterilmesini saðlayan text'in dinamik olarak ayarlanmasý.
        moneyIndicator.text = money.ToString() + "$ / " + maxMoney.ToString() + "$";
    }
}
