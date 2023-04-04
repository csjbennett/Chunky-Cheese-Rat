using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodestruct : MonoBehaviour
{
    public float destroyTime;
    private void Awake()
    {
        StartCoroutine(delete());
    }

    IEnumerator delete()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
