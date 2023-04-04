using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public Text moneyText;
    private int money;

    private float pistolUpCost;
    private float shotgunUpCost;
    private float smgUpCost;
    private float minigunUpCost;

    public Text pistolUpgrade;
    public Text shotgunUpgrade;
    public Text smgUpgrade;
    public Text minigunUpgrade;

    public Button shotgunUpgradeButton;
    public Button smgUpgradeButton;
    public Button minigunUpgradeButton;


    private void Start()
    {
        money = PlayerPrefs.GetInt("Money", 0);
        moneyText.text = "Money: " + money.ToString();

        if (PlayerPrefs.GetInt("PistolLevel") < 5)
            pistolUpgrade.text = "Upgrade Pistol Firerate \nCost: " +
                                   ((PlayerPrefs.GetInt("PistolLevel") + 1) * 5).ToString() +
                                   "\nLevel: " + PlayerPrefs.GetInt("PistolLevel").ToString();
        else
            pistolUpgrade.text = "Upgrade Pistol Firerate \nMax Level \nLevel: " + PlayerPrefs.GetInt("PistolLevel").ToString();

        PlayerPrefs.SetInt("UnlockedWeapons", PlayerPrefs.GetInt("UnlockedLevels", 1));
        int unlocked = PlayerPrefs.GetInt("UnlockedWeapons", 1);

        if (unlocked >= 2)
        {
            if (PlayerPrefs.GetInt("ShotgunLevel") < 5)
                shotgunUpgrade.text = "Upgrade Shotgun Firerate \nCost: " +
                                       ((PlayerPrefs.GetInt("ShotgunLevel") + 1) * 5).ToString() +
                                       "\nLevel: " + PlayerPrefs.GetInt("ShotgunLevel").ToString();
            else
                shotgunUpgrade.text = "Upgrade Pistol Firerate \nMax Level \nLevel: " + PlayerPrefs.GetInt("PistolLevel").ToString();

            if (unlocked >= 3)
            {
                if (PlayerPrefs.GetInt("SMGLevel") < 5)
                    smgUpgrade.text = "Upgrade SMG Firerate \nCost: " +
                                           ((PlayerPrefs.GetInt("SMGLevel") + 1) * 5).ToString() +
                                           "\nLevel: " + PlayerPrefs.GetInt("SMGLevel").ToString();
                else
                    smgUpgrade.text = "Upgrade SMG Firerate \nMax Level \nLevel: " + PlayerPrefs.GetInt("SMGLevel").ToString();

                if (unlocked >= 4)
                {
                    if (PlayerPrefs.GetInt("MinigunLevel") < 5)
                        minigunUpgrade.text = "Upgrade Minigun Firerate \nCost: " +
                                               ((PlayerPrefs.GetInt("MinigunLevel") + 1) * 5).ToString() +
                                               "\nLevel: " + PlayerPrefs.GetInt("MinigunLevel").ToString();
                    else
                        minigunUpgrade.text = "Upgrade Minigun Firerate \nMax Level \nLevel: " + PlayerPrefs.GetInt("MinigunLevel").ToString();
                }
                else
                    minigunUpgradeButton.gameObject.SetActive(false);
            }
            else
            {
                smgUpgradeButton.gameObject.SetActive(false);
                minigunUpgradeButton.gameObject.SetActive(false);
            }
        }
        else
        {
            shotgunUpgradeButton.gameObject.SetActive(false);
            smgUpgradeButton.gameObject.SetActive(false);
            minigunUpgradeButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(0);

        if (Input.GetKeyDown(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            money = 999;
            updateMoney();
        }
    }

    public void upgradePistol()
    {
        if (PlayerPrefs.GetInt("PistolLevel", 0) < 5 && (money - (PlayerPrefs.GetInt("PistolLevel", 0) + 1) * 5) >= 0)
        {
            money -= (PlayerPrefs.GetInt("PistolLevel", 0) + 1) * 5;
            PlayerPrefs.SetInt("Money", money);

            int level = PlayerPrefs.GetInt("PistolLevel", 0) + 1;
            PlayerPrefs.SetInt("PistolLevel", level);

            if (level < 5)
                pistolUpgrade.text = "Upgrade Pistol Firerate \nCost: " +
                                       ((PlayerPrefs.GetInt("PistolLevel") + 1) * 5).ToString() +
                                       "\nLevel: " + PlayerPrefs.GetInt("PistolLevel").ToString();
            else
                pistolUpgrade.text = "Upgrade Pistol Firerate \nMax Level \nLevel: " + PlayerPrefs.GetInt("PistolLevel").ToString();

            updateMoney();
        }
    }
    public void upgradeShotgun()
    {
        if (PlayerPrefs.GetInt("ShotgunLevel", 0) < 5 && (money - (PlayerPrefs.GetInt("ShotgunLevel", 0) + 1) * 5) >= 0)
        {
            money -= (PlayerPrefs.GetInt("ShotgunLevel", 0) + 1) * 5;
            PlayerPrefs.SetInt("Money", money);

            int level = PlayerPrefs.GetInt("ShotgunLevel", 0) + 1;
            PlayerPrefs.SetInt("ShotgunLevel", level);

            if (level < 5)
                shotgunUpgrade.text = "Upgrade Shotgun Firerate \nCost: " +
                                       ((PlayerPrefs.GetInt("ShotgunLevel") + 1) * 5).ToString() +
                                       "\nLevel: " + PlayerPrefs.GetInt("ShotgunLevel").ToString();
            else
                shotgunUpgrade.text = "Upgrade Shotgun Firerate \nMax Level \nLevel: " + PlayerPrefs.GetInt("ShotgunLevel").ToString();

            updateMoney();
        }
    }
    public void upgradeSMG()
    {
        if (PlayerPrefs.GetInt("SMGLevel", 0) < 5 && (money - (PlayerPrefs.GetInt("SMGLevel", 0) + 1) * 5) >= 0)
        {
            money -= (PlayerPrefs.GetInt("SMGLevel", 0) + 1) * 5;
            PlayerPrefs.SetInt("Money", money);

            int level = PlayerPrefs.GetInt("SMGLevel", 0) + 1;
            PlayerPrefs.SetInt("SMGLevel", level);

            if (level < 5)
                smgUpgrade.text = "Upgrade SMG Firerate \nCost: " +
                                       ((PlayerPrefs.GetInt("SMGLevel") + 1) * 5).ToString() +
                                       "\nLevel: " + PlayerPrefs.GetInt("SMGLevel").ToString();
            else
                smgUpgrade.text = "Upgrade SMG Firerate \nMax Level \nLevel: " + PlayerPrefs.GetInt("SMGLevel").ToString();

            updateMoney();
        }
    }
    public void upgradeMinigun()
    {
        if (PlayerPrefs.GetInt("MinigunLevel", 0) < 5 && (money - (PlayerPrefs.GetInt("MinigunLevel", 0) + 1) * 5) >= 0)
        {
            money -= (PlayerPrefs.GetInt("MinigunLevel", 0) + 1) * 5;
            PlayerPrefs.SetInt("Money", money);

            int level = PlayerPrefs.GetInt("MinigunLevel", 0) + 1;
            PlayerPrefs.SetInt("MinigunLevel", level);

            if (level < 5)
                minigunUpgrade.text = "Upgrade Minigun Firerate \nCost: " +
                                       ((PlayerPrefs.GetInt("MinigunLevel") + 1) * 5).ToString() +
                                       "\nLevel: " + PlayerPrefs.GetInt("MinigunLevel").ToString();
            else
                minigunUpgrade.text = "Upgrade Minigun Firerate \nMax Level \nLevel: " + PlayerPrefs.GetInt("MinigunLevel").ToString();

            updateMoney();
        }
    }

    private void updateMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        moneyText.text = "Money: " + money.ToString();
    }
}
