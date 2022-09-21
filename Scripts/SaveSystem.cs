using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public GameManager manage;
    public Expander expand;
    private bool save= true;
    private bool load = true;

    private void Start()
    {
        // E�er y�kleme yapmak isteniyorsa ve daha �nceden kay�t yap�lm��sa oyunu y�kl�yor.
        if (load && PlayerPrefs.GetInt("saved") == 2)
        {
            LoadGame();
        }
    }

    private void OnApplicationQuit()
    {
        // E�er kay�t yapmak isteniyorsa oyundaki gerekli noktalar� kay�t ediyor.
        if (save)
        {
            SaveGame(); 
        }
    }

    public void SaveGame()
    {
        // GameManager saveleri.

        //PlayerPrefs.SetFloat("money", manage.money);

        PlayerPrefs.SetFloat("msCost", manage.msCost);
        PlayerPrefs.SetFloat("csCost", manage.csCost);
        PlayerPrefs.SetFloat("capCost", manage.capCost);
        PlayerPrefs.SetFloat("dcIncCost", manage.dcIncCost);

        PlayerPrefs.SetInt("msLv", manage.msLv);
        PlayerPrefs.SetInt("csLv", manage.csLv);
        PlayerPrefs.SetInt("capLv", manage.capLv);
        PlayerPrefs.SetInt("dcIncLv", manage.dcIncLv);

        PlayerPrefs.SetInt("expansionLv", manage.expansionLv);


        // Expander saveleri.
        PlayerPrefs.SetInt("expander1MoneyToUnl", expand.moneyToUnl);
        PlayerPrefs.SetInt("expander1Progress", expand.progress);
        

        // Sadece kay�t edilmi�se y�klemenin yap�lmas� i�in.
        PlayerPrefs.SetInt("saved", 2);

    }

    public void LoadGame()
    {
        // GameManager y�klemeleri.
        //manage.money = PlayerPrefs.GetFloat("money");
        
        manage.msCost = PlayerPrefs.GetFloat("msCost");
        manage.csCost = PlayerPrefs.GetFloat("csCost");
        manage.capCost = PlayerPrefs.GetFloat("capCost");
        manage.dcIncCost = PlayerPrefs.GetFloat("dcIncCost");

        manage.msLv = PlayerPrefs.GetInt("msLv");
        manage.csLv = PlayerPrefs.GetInt("csLv");
        manage.capLv = PlayerPrefs.GetInt("capLv");
        manage.dcIncLv = PlayerPrefs.GetInt("dcIncLv");

        manage.expansionLv = PlayerPrefs.GetInt("expansionLv");

        // Expander y�klemeleri.
        expand.moneyToUnl = PlayerPrefs.GetInt("expander1MoneyToUnl");
        expand.progress = PlayerPrefs.GetInt("expander1Progress");
        
   
    }
}