using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Follow : MonoBehaviour
{
    public GameObject BG;
    public GameObject MG;

    // Update is called once per frame
    void Update()
    {
        BG.transform.position = new Vector2(this.transform.position.x * 0.05f, BG.transform.position.y);
        MG.transform.position = new Vector2(this.transform.position.x * 0.1f, MG.transform.position.y);
    }
}
