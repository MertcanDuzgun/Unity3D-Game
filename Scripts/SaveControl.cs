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
            // Binan�n a��lmas�
            openingObject.SetActive(true);

            // Kilit a�may� yarayan objenin progress'inin kay�t edilen progress'e e�itlenmesi.
            unlocker.progress = PlayerPrefs.GetInt(progressString);
        }
    }

    public void Buy()
    {
        if(unlocker.progress >= unlocker.moneyToUnl)
        {
            // PlayerPrefs'e kay�t Set edilecek de�i�kenlerin de�erlerinin girilmesi.
            unlockValue = 1;
            progressValue = unlocker.progress;

            // PlayerPrefs'e de�i�kenlerin eklenmesi.
            PlayerPrefs.SetInt(saveString, unlockValue);
            PlayerPrefs.SetInt(progressString, progressValue);

            // Binan�n aktive edilmesi.
            openingObject.SetActive(true);
        }
    }
}
