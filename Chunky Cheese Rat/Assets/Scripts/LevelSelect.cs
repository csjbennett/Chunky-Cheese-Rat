using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    private int unlockedLevels;

    public Button level1;
    public Button level2;
    public Button level3;
    public Button level4;

    void Start()
    {
        unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        level2.gameObject.SetActive(false);
        level3.gameObject.SetActive(false);
        level4.gameObject.SetActive(false);

        if (unlockedLevels >= 2)
        {
            level2.gameObject.SetActive(true);
            if (unlockedLevels >= 3)
            {
                level3.gameObject.SetActive(true);
                if (unlockedLevels >= 4)
                    level4.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(0);
    }

    public void loadLevel1()
    {
        SceneManager.LoadScene(1);
    }
    public void loadLevel2()
    {
        SceneManager.LoadScene(2);
    }
    public void loadLevel3()
    {
        SceneManager.LoadScene(3);
    }
    public void loadLevel4()
    {
        SceneManager.LoadScene(4);

    }
}
