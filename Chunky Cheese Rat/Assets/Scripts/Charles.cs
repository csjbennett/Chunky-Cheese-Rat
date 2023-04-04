using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.SceneManagement;

public class Charles : MonoBehaviour
{
    public KeyCode right;
    public KeyCode left;
    public KeyCode jump;
    public KeyCode shoot;
    public KeyCode up;
    public KeyCode switchGun;
    public KeyCode turn;
    public float speed;
    public float jumpHeight;
    public int playerCount;
    private GameObject otherPlayer;
    public Rigidbody2D rb;
    private GameObject mainCam;
    public GameObject marker;
    private bool grounded;
    private Vector3 rTurn = new Vector3(0, 0, 0);
    private Vector3 lTurn = new Vector3(0, 180, 0);
    public GameObject Chuck;   // Player 2 (if 2 player mode is selected)
    private float airTime = 0; // Variable used to determine how long player's been in the air
                               // Prevents player from jumping to the moon
    public Animator anm;
    public GameObject arm;
    public Animator armAnm;
    public float stageHeight; // Sets camera height based on stage's position
    public Transform point1; // 2 points used to check if player is grounded
    public Transform point2;
    public GameObject bullet;
    public int weapon;
    private int weaponsUnlocked;
    private bool chambered = true;
    public GameObject pistolBarrel;
    public GameObject shotgunBarrel;
    public GameObject smgBarrel;
    public GameObject minigunBarrel;
    public GameObject gauge;
    public GameObject gaugeBar;
    private Vector3 gaugePos;
    private float heat = 0;
    private bool overheat = false;
    public GameObject healthBar;
    public SpriteRenderer healthBarSpr;
    private float healthInit;
    public float health;
    private GameObject lives;
    public GameObject splat;

    public AudioSource audioSource;
    public AudioClip pistolShoot;
    public AudioClip shotgunShoot;
    public AudioClip smgShoot;
    public AudioClip minigunShoot;

    private float pistolLevel;
    private float shotgunLevel;
    private float smgLevel;
    private float minigunLevel;

    public bool bossBattle;

    void Start()
    {
        PlayerPrefs.SetFloat("StageHeight", stageHeight);

        gaugePos = gauge.transform.localPosition;

        gauge.transform.position = Vector2.down * 25;

        playerCount = PlayerPrefs.GetInt("PlayerCount");

        grounded = true;

        mainCam = GameObject.FindGameObjectWithTag("MainCamera");

        if (playerCount == 1)
        {
            otherPlayer = null;
        }
        else
        {
            otherPlayer = Instantiate(Chuck, this.transform.position - (Vector3.left * 5), Quaternion.identity);
            otherPlayer.gameObject.name = "Chuck";
            if (bossBattle)
                otherPlayer.GetComponent<Chuck>().bossBattle = true;
        }

        weaponsUnlocked = PlayerPrefs.GetInt("UnlockedWeapons", 1);

        healthInit = health;

        lives = GameObject.FindGameObjectWithTag("Vitals");

        pistolLevel = PlayerPrefs.GetInt("PistolLevel");
        shotgunLevel = PlayerPrefs.GetInt("ShotgunLevel");
        smgLevel = PlayerPrefs.GetInt("SMGLevel");
        minigunLevel = PlayerPrefs.GetInt("MingunLevel");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var activeScene = SceneManager.GetActiveScene();
            if (PlayerPrefs.GetInt("UnlockedLevels", 1) <= activeScene.buildIndex)
                PlayerPrefs.SetInt("UnlockedLevels", activeScene.buildIndex);

            SceneManager.LoadScene(0);
        }

        // Walk Right
        if (Input.GetKey(right) && !Input.GetKey(left))
        {
            if (grounded)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                anm.Play("Walk");
            }
            else
            {
                rb.velocity = new Vector2(speed * 0.7f, rb.velocity.y);
                anm.Play("Idle");
            }
        }

        // Walk Left
        else if (Input.GetKey(left) && !Input.GetKey(right))
        {
            if (grounded)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                anm.Play("Walk");
            }
            else
                rb.velocity = new Vector2(speed * -0.7f, rb.velocity.y);
        }

        else if (grounded)
            anm.Play("Idle");

        // Aim up
        if (Input.GetKeyDown(up))
        {
            arm.transform.localEulerAngles = new Vector3(0, 0, 45);
            arm.transform.localPosition = Vector2.up * 0.25f;
        }
        else if (Input.GetKeyUp(up))
        {
            arm.transform.localEulerAngles = Vector3.zero;
            arm.transform.localPosition = Vector2.zero;
        }

        // Turn Around
        if (Input.GetKeyDown(turn))
        {
            if (this.gameObject.transform.eulerAngles == rTurn)
            {
                this.gameObject.transform.eulerAngles = lTurn;
                marker.gameObject.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                this.gameObject.transform.eulerAngles = rTurn;
                marker.gameObject.transform.eulerAngles = Vector3.zero;
            }
        }

        // Tap Jump
        if (Input.GetKeyDown(jump) && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            grounded = false;
        }
        if (Input.GetKeyUp(jump) && !grounded)
            airTime = 0.25f;

        // Hold Jump
        if (Input.GetKey(jump) && airTime < 0.25f)
        {
            if (airTime > 0.05f)
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight / 1.15f);
        }
        if (!grounded)
            airTime += Time.deltaTime;



        if (heat > 0)
        {
            heat -= Time.deltaTime / 3.0f;
            gaugeBar.transform.localScale = new Vector3(heat, 1, 1);
            if (heat >= 1)
            {
                overheat = true;
            }
        }
        else if (heat <= 0)
            overheat = false;

        if (Input.GetKey(shoot))
        {
            switch (weapon)
            {
                case 1:
                    if (Input.GetKeyDown(shoot) && chambered)
                    {
                        armAnm.Play("PistolShoot");
                        Instantiate(bullet, pistolBarrel.transform.position, arm.transform.rotation, null);
                        StartCoroutine(chamberPistol());
                        chambered = false;
                        audioSource.PlayOneShot(pistolShoot);
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(shoot) && chambered)
                    {
                        armAnm.Play("ShotgunShoot");
                        Instantiate(bullet, shotgunBarrel.transform.position, Quaternion.Euler(arm.transform.eulerAngles + Vector3.forward * 2.5f), null);
                        Instantiate(bullet, shotgunBarrel.transform.position, arm.transform.rotation, null);
                        Instantiate(bullet, shotgunBarrel.transform.position, Quaternion.Euler(arm.transform.eulerAngles - Vector3.forward * 2.5f), null);
                        StartCoroutine(chamberShotgun());
                        chambered = false;
                        audioSource.PlayOneShot(shotgunShoot);
                    }
                    break;
                case 3:
                    if (chambered)
                    {
                        armAnm.Play("SMGShoot");
                        Instantiate(bullet, smgBarrel.transform.position, arm.transform.rotation, null);
                        StartCoroutine(chamberSMG());
                        chambered = false;
                        audioSource.PlayOneShot(smgShoot);
                    }
                        break;
                case 4:
                    if (chambered && !overheat)
                    {
                        armAnm.Play("MinigunShoot");
                        Instantiate(bullet, minigunBarrel.transform.position, arm.transform.rotation, null);
                        StartCoroutine(chamberMinigun());
                        chambered = false;
                        heat += (0.1f - (minigunLevel * 0.005f));
                        audioSource.PlayOneShot(minigunShoot);
                    }
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown(switchGun))
        {
            switch (weapon)
            {
                case 1:
                    if (weaponsUnlocked > 1)
                    {
                        weapon += 1;
                        armAnm.Play("Shotgun");
                    }
                    break;
                case 2:
                    if (weaponsUnlocked > 2)
                    {
                        weapon += 1;
                        armAnm.Play("SMG");
                    }
                    else
                    {
                        weapon = 1;
                        armAnm.Play("Pistol");
                    }
                    break;
                case 3:
                    if (weaponsUnlocked > 3)
                    {
                        weapon += 1;
                        armAnm.Play("Minigun");
                        gauge.transform.localPosition = gaugePos;
                    }
                    else
                    {
                        weapon = 1;
                        armAnm.Play("Pistol");
                    }
                    break;
                case 4:
                    {
                        weapon = 1;
                        armAnm.Play("Pistol");
                    }
                    gauge.transform.localPosition = Vector2.down * 25;
                    break;
                default:
                    break;
            }
        }


        if (playerCount == 2)
        {
            if (!bossBattle)
                mainCam.transform.position = camPos();
        }
        else
        {
            if (!bossBattle)
                mainCam.transform.position = new Vector3
                (this.transform.position.x + 17.5f,
                 stageHeight,
                 -10);
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.tag == "Bullet")
        {
            damage();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Collider2D[] hits = Physics2D.OverlapAreaAll(point1.transform.position, point2.transform.position, 8);
        if (hits != null && col.gameObject.tag != "Bullet")
        {
            grounded = true;
            airTime = 0;
        }

        if (col.gameObject.tag == "Bullet")
        {
            damage();
        }
    }

    private void damage()
    {
        health -= 1;
        healthBar.transform.localScale = new Vector3(health / healthInit, 1, 1);
        healthBarSpr.color = new Color((healthInit - health) / healthInit, health / healthInit, 0);
        if (health <= 0)
        {
            lives.GetComponent<Vitals>().p1die();
            Instantiate(splat, arm.transform.position, arm.transform.rotation, null);
            Destroy(this.gameObject);
        }
    }

    Vector3 camPos()
    {
        var dist = Mathf.Abs(this.transform.position.x - otherPlayer.transform.position.x);
        if (dist > 20)
        {
            if (dist < 40)
                Camera.main.orthographicSize = 19 + dist / 20;
        }
        else
            Camera.main.orthographicSize = 20;

        Vector3 camP = new Vector3((this.gameObject.transform.position.x + otherPlayer.transform.position.x + 35) / 2,
                                    stageHeight,
                                    -10);
        return camP;
    }


    IEnumerator chamberPistol()
    {
        yield return new WaitForSecondsRealtime(0.33f - pistolLevel * 0.05f);
        chambered = true;
    }
    IEnumerator chamberShotgun()
    {
        yield return new WaitForSecondsRealtime(0.75f - shotgunLevel * 0.1f);
        chambered = true;
    }
    IEnumerator chamberSMG()
    {
        yield return new WaitForSecondsRealtime(0.3f - smgLevel * 0.025f);
        chambered = true;
    }
    IEnumerator chamberMinigun()
    {
        yield return new WaitForSecondsRealtime(0.1f - minigunLevel * 0.01f);
        chambered = true;
    }
}
