using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToGame : MonoBehaviour
{
    public SpriteRenderer spr;
    private float fade = 1.5f;
    private bool loadingIn = true;
    private bool loadingLevel = false;
    private bool loadingMenu = false;

    private void Update()
    {
        if (fade > 0 && loadingIn)
        {
            fade -= Time.deltaTime * 0.5f;
            spr.color = new Color(0, 0, 0, fade);
        }
        else
            loadingIn = false;


        if (loadingLevel && fade < 1.75f)
        {
            fade += Time.deltaTime;
            spr.color = new Color(0, 0, 0, fade);
        }
        else if (loadingLevel && fade > 1.51f)
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel", 0));
        }


        if (loadingMenu && fade < 1.75f)
        {
            fade += Time.deltaTime;
            spr.color = new Color(0, 0, 0, fade);
        }
        else if (loadingMenu && fade > 1.51f)
        {
            SceneManager.LoadScene(0);
        }

    }

    public void reloadLevel()
    {
        loadingLevel = true;
        loadingMenu = false;
        loadingIn = false;
    }

    public void loadMenu()
    {
        loadingMenu = true;
        loadingLevel = false;
        loadingIn = false;
    }
}
