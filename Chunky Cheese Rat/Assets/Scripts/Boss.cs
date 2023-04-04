using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public GameObject healthBar;
    private float healthInit;
    public float health;
    private int playerCount;
    public GameObject player;
    public bool attacking;
    private bool hurting;
    private float hurtVal;
    public SpriteRenderer hpspr;
    public GameObject head;
    private SpriteRenderer headSpr;

    public GameObject projectile;
    public GameObject[] balls;

    private AudioSource sound;
    public AudioClip screech;
    public AudioClip hurtSound;
    public AudioClip slamSound;
    public AudioClip deathSound;
    public AudioClip summonSound;

    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject sparkEmitter;
    public GameObject longSparkEmitter;
    public GameObject enemy;

    public Animator anm;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player1");

        headSpr = head.GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();
        hpspr = healthBar.GetComponent<SpriteRenderer>();

        healthInit = health;
        sound.PlayOneShot(deathSound, 0.5f);
    }

    public void playNext()
    {
        float rand = Random.value;
        int choice;
        if (rand < 0.25f)
            choice = 1;
        else if (rand >= 0.25f && rand < 0.5f)
            choice = 2;
        else if (rand >= 0.5f && rand < 0.75f)
            choice = 3;
        else
            choice = 4;

        switch (choice)
        {
            case 1:
                anm.Play("RockBarrage");
                break;
            case 2:
                anm.Play("Idle2");
                break;
            case 3:
                anm.Play("Idle1");
                break;
            case 4:
                anm.Play("Summon");
                break;
            default:
                anm.Play("Idle1");
                break;
        }

        rand = Random.value;

        if (rand > 0.5f)
            transform.eulerAngles = new Vector2(0, 180);
        else
            transform.eulerAngles = new Vector2(0, 0);
    }

    public void hurt()
    {
        hurtVal = 0;
        health -= 1;
        healthBar.transform.localScale = new Vector3((10 * health / healthInit), 2, 1);
        hpspr.color = new Color((healthInit - health) / healthInit, healthInit / health, 0);
        sound.PlayOneShot(hurtSound, 0.5f);
        if (health <= 0)
        {
            var activeScene = SceneManager.GetActiveScene();
            if (PlayerPrefs.GetInt("UnlockedLevels", 1) < activeScene.buildIndex)
                PlayerPrefs.SetInt("UnlockedLevels", activeScene.buildIndex);

            sound.PlayOneShot(deathSound, 0.5f);
            rightHand.GetComponent<SpriteRenderer>().sortingOrder = -1;
            leftHand.GetComponent<SpriteRenderer>().sortingOrder = -1;
            Destroy(head.GetComponent<BoxCollider2D>());
            Destroy(rightHand.GetComponent<BoxCollider2D>());
            Destroy(leftHand.GetComponent<BoxCollider2D>());
            anm.StopPlayback();
            Destroy(healthBar);
            anm.Play("Death");
        }
    }

    public void screechPlay()
    {
        sound.PlayOneShot(screech);
    }

    public void hurtSoundPlay()
    {
        sound.PlayOneShot(hurtSound);
    }

    public void slamSoundPlay()
    {
        sound.PlayOneShot(slamSound);
    }

    public void shoot()
    {
        Instantiate(projectile, head.transform.position, Quaternion.Euler(new Vector3(0, 0, findAngle())));
    }

    private float findAngle()
    {
        float angle = Mathf.Atan2(player.transform.position.y - head.transform.position.y,
                                  player.transform.position.x - head.transform.position.x) * Mathf.Rad2Deg;
        
        return angle;
    }

    public void dropBall()
    {
        float rand = Random.value;
        int choice;
        if (rand < 0.25f)
            choice = 0;
        else if (rand >= 0.25f && rand < 0.5f)
            choice = 1;
        else if (rand >= 0.5f && rand < 0.75f)
            choice = 2;
        else
            choice = 3;

        Instantiate(balls[choice], rightHand.transform.position, Quaternion.identity);
        sound.PlayOneShot(summonSound, 0.5f);
    }

    public void summonEnemies()
    {
        GameObject rat1 = Instantiate(enemy, leftHand.transform.position, Quaternion.identity);
        GameObject rat2 = Instantiate(enemy, rightHand.transform.position, Quaternion.identity);
        var script1 = rat1.GetComponent<Enemy>();
        var script2 = rat2.GetComponent<Enemy>();
        sound.PlayOneShot(summonSound);
    }

    public void enableColliders()
    {
        head.GetComponent<BoxCollider2D>().enabled = true;
        rightHand.GetComponent<BoxCollider2D>().enabled = true;
        leftHand.GetComponent<BoxCollider2D>().enabled = true;
        rightHand.GetComponent<SpriteRenderer>().sortingOrder = 10;
        leftHand.GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    public void rightHandShortEmit()
    {
        Instantiate(sparkEmitter, rightHand.transform.position, rightHand.transform.rotation, rightHand.transform);
    }

    public void leftHandShortEmit()
    {
        Instantiate(sparkEmitter, leftHand.transform.position, leftHand.transform.rotation, leftHand.transform);
    }

    public void rightHandLongEmit()
    {
        Instantiate(longSparkEmitter, rightHand.transform.position, rightHand.transform.rotation, rightHand.transform);
    }

    public void leftHandLongEmit()
    {
        Instantiate(longSparkEmitter, leftHand.transform.position, leftHand.transform.rotation, leftHand.transform);
    }

    public void deathSequence()
    {
        var vitals = GameObject.FindGameObjectWithTag("Vitals").GetComponent<Vitals>();
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) + 25);
        vitals.win();
    }
}
