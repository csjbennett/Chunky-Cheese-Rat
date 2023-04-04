using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPiece : MonoBehaviour
{
    public GameObject spark;
    public GameObject boss;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            boss.GetComponent<Boss>().hurt();
            Instantiate(spark, col.gameObject.transform.position, Quaternion.identity, null);
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Bullet")
        {
            boss.GetComponent<Boss>().hurt();
            Instantiate(spark, obj.gameObject.transform.position, Quaternion.identity, null);
            Destroy(obj.gameObject);
        }
    }
}
