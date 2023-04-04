using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float range = 5;
    public float speed;
    public GameObject sparks;
    void Update()
    {
        transform.localPosition += transform.right * Time.deltaTime * speed;
        range -= Time.deltaTime;
        if (range <= 0)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Enemy")
        {
            Instantiate(sparks, transform.position, Quaternion.identity, null);
        }
        Destroy(this.gameObject);
    }
}
