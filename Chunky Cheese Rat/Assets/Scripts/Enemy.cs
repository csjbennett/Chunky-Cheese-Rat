using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    private int playerCount;
    public bool active;
    private Rigidbody2D rb;
    public GameObject bullet;
    public GameObject arms;
    private Animator anm;
    private SpriteRenderer armSpr;
    private AudioSource sound;

    public AudioClip shootSound;

    public GameObject splat;

    public int health;
    public float speed;
    public float shotDelay;
    public float stopDistance;
    private float distance;
    public bool shooting;
    private float cooldown;
    private float maxCoolDown;
    public float shootDistance;

    public bool shotgun;

    public Animator armAnm;

    public LayerMask ignoreEnemy;

    private Vector3 rightTurn = new Vector3(0, 180, 0);
    private Vector3 leftTurn = Vector3.zero;

    public bool target;
    private bool inRange = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCount = PlayerPrefs.GetInt("PlayerCount", 1);
        if (playerCount == 1)
        {
            player = GameObject.FindGameObjectWithTag("Player1");
            StartCoroutine(closestCheck());
        }
        armSpr = arms.GetComponent<SpriteRenderer>();
        anm = GetComponent<Animator>();

        shotDelay += (Random.value / 2);

        maxCoolDown = shotDelay;

        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active && player != null)
        {
            distance = Mathf.Abs(this.transform.position.x - player.transform.position.x);

            if (player.transform.position.x > this.transform.position.x)
            {
                this.transform.eulerAngles = rightTurn;
                armSpr.flipX = true;
            }
            else
            {
                this.transform.eulerAngles = leftTurn;
                armSpr.flipX = false;
            }

            arms.transform.eulerAngles = new Vector3(0, 0, findAngle());

            if (distance > stopDistance)
            {
                if (player.transform.position.x > this.transform.position.x)
                    rb.velocity = new Vector2(1, rb.velocity.y) + Vector2.right * speed;
                else
                    rb.velocity = new Vector2(1, rb.velocity.y) + Vector2.right * -speed;

                anm.Play("Walk");

                if (distance <= shootDistance && !shooting)
                {
                    StopCoroutine(shoot());
                    StartCoroutine(shoot());
                    shooting = true;
                }
            }
            else
                anm.Play("Stand");
        }
        else
        {
            if (playerCount == 1)
                player = GameObject.FindGameObjectWithTag("Player1");

            active = false;
            arms.transform.eulerAngles = Vector3.zero;
            anm.Play("Stand");
        }   
        if (cooldown <= maxCoolDown)
        {
            cooldown += Time.deltaTime;
        }
    }

    private float findAngle()
    {
        float angle = Mathf.Atan2(player.transform.position.y - this.transform.position.y,
                                  player.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg + 180;
        if (player.transform.position.x > this.transform.position.x)
            angle += 180;
        return angle;
    }

    IEnumerator closestCheck()
    {
        yield return new WaitForSeconds(0.3f);

        RaycastHit2D hit;
        if (player != null)
        {
            var diff = new Vector2(player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y);
            hit = Physics2D.Raycast(this.transform.position, diff, Mathf.Infinity, ignoreEnemy);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player1")
                {
                    active = true;
                }
                else
                    active = false;
            }
        }
        else
            player = GameObject.FindGameObjectWithTag("Player1");

        StartCoroutine(closestCheck());
    }
 

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            health -= 1;
            if (health <= 0)
            {
                int money = PlayerPrefs.GetInt("Money", 0);
                money += 1;
                PlayerPrefs.SetInt("Money", money);
                GameObject.FindGameObjectWithTag("Vitals").GetComponent<Vitals>().updateMoney();
                Instantiate(splat, this.gameObject.transform.position, Quaternion.identity);
                if (target)
                    GameObject.FindGameObjectWithTag("Vitals").GetComponent<Vitals>().win();
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator shoot()
    {
        yield return new WaitForSeconds(shotDelay);
        if (shooting && cooldown > maxCoolDown && active)
        {
            if (transform.eulerAngles == leftTurn)
            {
                if (!shotgun)
                    Instantiate(bullet, arms.transform.position, Quaternion.Euler(arms.transform.eulerAngles + new Vector3(0, 0, 180)));
                else
                {
                    Instantiate(bullet, this.transform.position, Quaternion.Euler(arms.transform.eulerAngles + Vector3.forward * 1.5f + new Vector3(0, 0, 180)), null);
                    Instantiate(bullet, this.transform.position, Quaternion.Euler(arms.transform.eulerAngles - Vector3.forward * 1.5f + new Vector3(0, 0, 180)), null);
                }
            }
            else
            {
                if (!shotgun)
                    Instantiate(bullet, arms.transform.position, arms.transform.rotation);
                else
                {
                    Instantiate(bullet, this.transform.position, Quaternion.Euler(arms.transform.eulerAngles + Vector3.forward * 1.5f + new Vector3(0, 0, 0)), null);
                    Instantiate(bullet, this.transform.position, Quaternion.Euler(arms.transform.eulerAngles - Vector3.forward * 1.5f + new Vector3(0, 0, 0)), null);
                }
            }

            sound.PlayOneShot(shootSound);
            cooldown = 0;
            armAnm.Play("Shoot");
            StartCoroutine(shoot());
        }
        else
            StopCoroutine(shoot());
    }
}

/*                if (xDist1() < xDist2())
                {
                    var diff = new Vector2(players[0].transform.position.x - this.transform.position.x, players[0].transform.position.y - this.transform.position.y);
                    hit = Physics2D.Raycast(this.transform.position, diff, Mathf.Infinity, ignoreEnemy);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.tag == "Player1")
                        {
                            player = players[0];
                            active = true;
                        }
                        else if (hit.collider.gameObject.tag == "Player2")
                        {
                            player = players[1];
                            active = true;
                        }
                        else
                        {
                            var diff2 = new Vector2(players[1].transform.position.x - this.transform.position.x, players[1].transform.position.y - this.transform.position.y);
                            hit = Physics2D.Raycast(this.transform.position, diff2, Mathf.Infinity, ignoreEnemy);
                            if (hit.collider != null)
                            {
                                if (hit.collider.gameObject.tag == "Player2")
                                {
                                    player = players[1];
                                    active = true;
                                }
                                else if (hit.collider.gameObject.tag == "Player1")
                                {
                                    player = players[0];
                                    active = true;
                                }
                                else
                                    active = false;
                            }
                        }
                    }
                }
                else
                {
                    var diff = new Vector2(players[1].transform.position.x - this.transform.position.x, players[1].transform.position.y - this.transform.position.y);
                    hit = Physics2D.Raycast(this.transform.position, diff, Mathf.Infinity, ignoreEnemy);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.tag == "Player2")
                        {
                            player = players[1];
                            active = true;
                        }
                        else if (hit.collider.gameObject.tag == "Player1")
                        {
                            player = players[2];
                            active = true;
                        }
                        else
                        {
                            var diff2 = new Vector2(players[0].transform.position.x - this.transform.position.x, players[0].transform.position.y - this.transform.position.y);
                            hit = Physics2D.Raycast(this.transform.position, diff2, Mathf.Infinity, ignoreEnemy);
                            Debug.DrawRay(this.transform.position, diff2);
                            if (hit.collider != null)
                            {
                                if (hit.collider.gameObject.tag == "Player1")
                                {
                                    player = players[0];
                                    active = true;
                                }
                                else if (hit.collider.gameObject.tag == "Player2")
                                {
                                    player = players[1];
                                    active = true;
                                }
                                else
                                    active = false;
                            }
                        }
                    }
                }
            }
            else if (playerCount == 1)*/