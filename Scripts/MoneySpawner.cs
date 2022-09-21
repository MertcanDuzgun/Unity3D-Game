using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MoneySpawner : MonoBehaviour
{
    public float genTime, witTime, witDelay = 1;
    public float genMoneyO = 80.0f;
    public float genMoney, genMoneyCur;
    public float genDelay;

    public TextMeshProUGUI display;

    public GameObject originalGameObject;
    public GameObject player;
    public GameObject anim;
    public GameObject money;

    public GameManager manager;

    private bool enter, exit, invoked = false;

    private Vector3[] positionArray = new Vector3[100];

    [SerializeField] private GameObject moneyStackPoints = null;
    
   
    void Start()
    {
        // DOMove yapýldýktan sonra para hareket ettiðinden oyun ilk baþladýðýnda dizilmiþ hali ile bölgeye atanmýþ paralarýn pozisyonlarýnýn alýnmasý.
        for (int i = 0; i < originalGameObject.transform.childCount; i++)
        {
            positionArray[i] = new Vector3(transform.GetChild(i).transform.position.x, transform.GetChild(i).transform.position.y, transform.GetChild(i).transform.position.z);
        }

        // Þu andaki para oluþturma oranýnýn tutulmasý.
        genMoneyCur = genMoney;

        // Para oluþturma sýklýðýnýn ayarlanmasý.
        genDelay = 100 / genMoney;

        InvokeRepeating("MoneyGenerate", genTime, genDelay);
    }

    void Update()
    {
        // Saniyede oluþturulan paranýn gösterilmesi için display.text'in ayarlanmasý.
        if (transform.gameObject.activeSelf)
        {
            display.gameObject.SetActive(true);
            display.text = genMoney.ToString() + "$/s";
        }

        // csLv yani toplama hýzý seviyesine göre para çekme aralýðýnýn azaltýlmasý ve geliþtirme ücretinin ayarlanmasý.
        switch (manager.csLv)
        {
            case 0:
                witDelay = genDelay / 1.5f;
                manager.csCost = 3000f;
                break;
            case 1:
                witDelay = genDelay / 2f;
                manager.csCost = 8000f;
                break;
            case 2:
                witDelay = genDelay / 2.5f;
                manager.csCost = 14000f;
                break;
            case 3:
                witDelay = genDelay / 3f;
                manager.csCost = 21000f;
                break;
            case 4:
                witDelay = genDelay / 3.5f;
                manager.csCost = 32000f;
                break;
            case 5:
                witDelay = genDelay / 4f;
                manager.csCost = 999999f;
                break;
        }

        // dcIncLv yani bölge gelir seviyesine göre para oluþturan bölgelerin para oluþturma oranýnýn arttýrýlmasý ve geliþtirme ücretinin ayarlanmasý.
        switch (manager.dcIncLv)
        {
            case 0:
                genMoney = genMoneyO * 1.0f;
                manager.dcIncCost = 4000.0f;
                break;
            case 1:
                genMoney = genMoneyO * 1.2f;
                manager.dcIncCost = 9000.0f;
                break;
            case 2:
                genMoney = genMoneyO * 1.4f;
                manager.dcIncCost = 16000.0f;
                break;
            case 3:
                genMoney = genMoneyO * 1.6f;
                manager.dcIncCost = 26000.0f;
                break;
            case 4:
                genMoney = genMoneyO * 1.8f;
                manager.dcIncCost = 38000.0f;
                break;
            case 5:
                genMoney = genMoneyO * 2.0f;
                manager.dcIncCost = 999999.0f;
                break;
        }

        // Para oluþturma deðeri deðiþtirildiði durumda oluþturulacak paranýn InvokeRepeating'de de deðiþtirilmesi.
        if (genMoney != genMoneyCur)
        {
            genMoneyCur = genMoney;
            genDelay = 100 / genMoney;
            CancelInvoke("MoneyGenerate");
            InvokeRepeating("MoneyGenerate", genTime, genDelay);
        }

        if (enter && !invoked)
        {
            InvokeRepeating("MoneyWithdrawTween", 0, witDelay);
            invoked = true;
        }

        if (exit)
        {
            CancelInvoke("MoneyWithdrawTween");
            invoked = false;
            for (int i = 0; i < originalGameObject.transform.childCount; i++)
            {
                transform.GetChild(i).transform.position = positionArray[i];
            }
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
    public void MoneyGenerate()
    {
        for (int i = 0; i < originalGameObject.transform.childCount; i++)
        {
            if (!(transform.GetChild(i).gameObject.activeSelf))
            {
                // Pozisyon array'indeki bilgiye göre oluþturulacak paranýn pozisyonunun ayarlanmasý ve paranýn aktive edilmesi.
                transform.GetChild(i).transform.position = positionArray[i];
                transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }
    }
    public void MoneyWithdrawTween()
    {
       
        for (int i = (originalGameObject.transform.childCount-1); i > -1 ; i--)
        {
            if ((transform.GetChild(i).gameObject.activeSelf) && manager.money < manager.maxMoney)
            {
                transform.GetChild(i).transform.DOMove(player.transform.position, duration: (witDelay / 2), snapping: false).OnComplete(() =>
                {
                    // Sarý partikül efekti olan animasyonun pozisyonunun ayarlanmasý ve aktive edilmesi.
                    anim.transform.position = player.transform.position + new Vector3(0, 1, 0);
                    anim.SetActive(true);

                    // Paranýn karakterin sýrtýnda yAxis'e eklenerek belirlenmiþ noktada paranýn oluþturulmasý ve yAxis'in arttýrýlmasý.
                    var moneyIns = Instantiate(money, moneyStackPoints.transform.position + new Vector3(0, player.GetComponent<PlayerControls>().yAxis, 0), Quaternion.identity);
                    player.GetComponent<PlayerControls>().yAxis += 0.25f;
                    
                    // Paranýn parentýnýn ve rotasyonunun ayarlanmasý. 
                    moneyIns.transform.parent = moneyStackPoints.transform;
                    moneyIns.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));

                    //Büyüyüp küçülme animasyonu.
                    moneyIns.transform.DORewind();
                    moneyIns.transform.DOPunchScale(new Vector3(1, 1, 1), .5f);

                    // Oyuncunun topladýðý paranýn deaktive edilmesi ve paranýn GameManager'daki paraya eklenmesi.
                    transform.GetChild(i).gameObject.SetActive(false);
                    manager.money += 100;
                });
                break;
            }
        }
    } 
}