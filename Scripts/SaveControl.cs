using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveControl : MonoBehaviour
{
    [SerializeField] private GameObject openingObject = null;
    [SerializeField] string saveString;
    [SerializeField] string progressString;
    [SerializeField] private int unlockValue, progressValue= 0;
    [SerializeField] private Unlocker unlocker;

    void Start()
    {
        if (PlayerPrefs.GetInt(saveString) == 1)
        {
            // Binanýn açýlmasý
            openingObject.SetActive(true);

            // Kilit açmayý yarayan objenin progress'inin kayýt edilen progress'e eþitlenmesi.
            unlocker.progress = PlayerPrefs.GetInt(progressString);
        }
    }

    public void Buy()
    {
        if(unlocker.progress >= unlocker.moneyToUnl)
        {
            // PlayerPrefs'e kayýt Set edilecek deðiþkenlerin deðerlerinin girilmesi.
            unlockValue = 1;
            progressValue = unlocker.progress;

            // PlayerPrefs'e deðiþkenlerin eklenmesi.
            PlayerPrefs.SetInt(saveString, unlockValue);
            PlayerPrefs.SetInt(progressString, progressValue);

            // Binanýn aktive edilmesi.
            openingObject.SetActive(true);
        }
    }
}
