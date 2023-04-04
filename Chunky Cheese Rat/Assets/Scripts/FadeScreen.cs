using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public float fadeOut;
    public Image fadeScreen;
    public float speedMultiplier;

    void Update()
    {
        if (fadeOut >= 0)
        {
            fadeOut -= Time.deltaTime * speedMultiplier;
            fadeScreen.color = new Color(0, 0, 0, fadeOut);
        }
        else
            Destroy(this.gameObject);
    }
}
