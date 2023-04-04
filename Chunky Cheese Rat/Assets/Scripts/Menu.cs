using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject settings;
    public GameObject[] buttons;
    public Text buttonText;
    public GameObject returnButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("UnlockedWeapons", 1) == 1)
            PlayerPrefs.SetInt("UnlockedLevels", 1);
    }

    public void onePlayer()
    {
        PlayerPrefs.SetInt("PlayerCount", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void twoplayer()
    {
        PlayerPrefs.SetInt("PlayerCount", 1);
        SceneManager.LoadScene(8);
    }

    public void openShop()
    {
        SceneManager.LoadScene(7);
    }

    public void resetProgress()
    {
        PlayerPrefs.SetInt("PistolLevel", 0);
        PlayerPrefs.SetInt("ShotgunLevel", 0);
        PlayerPrefs.SetInt("SMGLevel", 0);
        PlayerPrefs.SetInt("MinigunLevel", 0);
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("UnlockedWeapons", 1);
        PlayerPrefs.SetInt("PlayerCount", 1);
        PlayerPrefs.SetInt("UnlockedLevels", 1);
    }

    public void openSettings()
    {
        if (settings.activeInHierarchy == false)
        {
            settings.SetActive(true);
            buttonText.text = "Return to Menu";
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.position -= Vector3.left * 3600;
            }

            returnButton.transform.position += Vector3.left * 750;
        }
        else
        {
            settings.SetActive(false);
            buttonText.text = "View Controls";
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.position += Vector3.left * 3600;
            }
            returnButton.transform.position -= Vector3.left * 750;
        }
    }
}
