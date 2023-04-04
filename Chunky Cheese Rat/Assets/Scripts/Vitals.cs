using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Vitals : MonoBehaviour
{
    public bool p1alive = true;
    public bool p2alive = true;

    public GameObject player1;
    public GameObject player2;

    public AudioSource levelTheme;

    private int currentScreen;

    public GameObject fadeOut;
    public SpriteRenderer fadeOutSpr;
    public float opacity = 2;
    private bool gameOver = false;
    private bool victory = false;
    Scene activeScene;

    private GameObject cameraMain;
    public Text money;

    public void updateMoney()
    {
        money.text = "Money: " + PlayerPrefs.GetInt("Money", 0).ToString();
    }

    private void Start()
    {
        cameraMain = GameObject.FindGameObjectWithTag("MainCamera");
        activeScene = SceneManager.GetActiveScene();
        money.text = "Money: " + (0 + PlayerPrefs.GetInt("Money", 0)).ToString();
        if (PlayerPrefs.GetInt("PlayerCount", 0) == 1)
        {
            player1 = GameObject.FindGameObjectWithTag("Player1");
            player2 = null;
        }
        else
        {
            player1 = GameObject.FindGameObjectWithTag("Player1");
            player2 = GameObject.FindGameObjectWithTag("Player2");
        }
    }

    private void Update()
    {
        if (!gameOver && opacity > -1.0f && !victory)
        {
            opacity -= Time.deltaTime;
            fadeOutSpr.color = new Color(0, 0, 0, opacity / 2);
            money.color = new Color(255, 255, 255, 1 - opacity);
            fadeOut.transform.position = cameraMain.transform.position + Vector3.forward * 10;
        }

        if (gameOver && !victory)
        {
            if (opacity < 5.0f)
            {
                opacity += Time.deltaTime;
                money.color = new Color(255, 255, 255, 1 - opacity);
                fadeOutSpr.color = new Color(0, 0, 0, opacity / 2);
                levelTheme.volume -= Time.deltaTime * 0.2f;
                if (p1alive)
                fadeOut.transform.position = cameraMain.transform.position + Vector3.forward * 10;
            }
            else
            {
                PlayerPrefs.SetInt("LastLevel", activeScene.buildIndex);
                SceneManager.LoadScene(6);
            }
        }

        if (victory)
        {
            if (opacity < 5.0f)
            {
                opacity += Time.deltaTime;
                money.color = new Color(255, 255, 255, 1 - opacity);
                fadeOutSpr.color = new Color(0, 0, 0, opacity / 2);
                levelTheme.volume -= Time.deltaTime * 0.2f;
                fadeOut.transform.position = cameraMain.transform.position + Vector3.forward * 10;
            }
            else
            {
                if (activeScene.buildIndex == 4)
                {
                    SceneManager.LoadScene(5);
                }
                else
                {
                    if (PlayerPrefs.GetInt("UnlockedLevels") == activeScene.buildIndex)
                    {
                        PlayerPrefs.SetInt("UnlockedWeapons", activeScene.buildIndex + 1);
                        PlayerPrefs.SetInt("UnlockedLevels", activeScene.buildIndex + 1);
                    }
                    SceneManager.LoadScene(7);
                }
            }
        }
    }

    public void p1die()
    {
        if (PlayerPrefs.GetInt("PlayerCount") == 1 || p2alive == false)
            gameOver = true;
        p1alive = false;
    }

    public void p2die()
    {
        if (p1alive == false)
            gameOver = true;
        p2alive = false;
    }

    public void win()
    {
        victory = true;
    }
}
