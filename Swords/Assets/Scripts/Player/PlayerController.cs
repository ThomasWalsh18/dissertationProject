using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public int wave = 0;

    public SpriteRenderer MyspriteRenderer;
    public float currentAmmount = 0;
    public GameObject abilityWeapon;
    public float maxHealth = 10.0f;
    public float actualMaxHealth;
    public float PlayerDamage = 1f;
    public float abilDamage = 2f;
    public GameObject SecondSword;
    private bool countable = true;
    public float graceTime = 0.5f;
    GameObject[] dynamicHealthBar;
    public float NumberScore = 0;
    public Transform abilityBar;
    public float fireRate = 0.8f;
    private float nextFire = 0f;
    public Transform healthBar;
    public float currentHealth;
    public Canvas playerCanvas;
    public float Mspeed = 5.5f;
    private bool count = false;
    public bool chosen = false;
    public float cooldown = 2f;
    float damageTaken = 0.0f;
    public Text score;
    
    Animator my_animator;
    public bool hitable = true;
    Vector2 BigSwordPos;
    public bool flipX;

    RoomTemplates rooms;
    public int roomsCompleted;
    public List<float> times = new List<float>();

    public int hits = 0;
    public int misses = 0;

    void Start()
    {
        rooms = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        if (PlayerPrefs.GetInt("Difficulty") == 0) // Adaptive needs no changes
        {

        }
        else if (PlayerPrefs.GetInt("Difficulty") == 1) // Easy
        {
            //These will change
            fireRate = 0.8f;
            Mspeed = 6;
            PlayerDamage = 1.5f;
            abilDamage = 2.5f;
            rooms.roomDiff = 50;

        }
        else if (PlayerPrefs.GetInt("Difficulty") == 2) // medium
        {
            //these are the default values
            //Starting off in medium will allow the code to easily judge if it too hard easy
            fireRate = 1;
            Mspeed = 5.5f;
            PlayerDamage = 1f;
            abilDamage = 2f;
            rooms.roomDiff = 100;
        }
        else if (PlayerPrefs.GetInt("Difficulty") == 3) // Hard
        {
            fireRate = 1.3f;
            Mspeed = 5;
            PlayerDamage = 0.8f;
            abilDamage = 1.5f;
            rooms.roomDiff = 160;
        }
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        currentHealth = maxHealth;
        playerCanvas = GetComponentInChildren<Canvas>();
        healthBar = playerCanvas.transform.GetChild(1);
        healthBar.gameObject.SetActive(false);
        abilityBar = playerCanvas.transform.GetChild(2);
        my_animator = GetComponent<Animator>();
        MyspriteRenderer = GetComponent<SpriteRenderer>();

        Vector3 startPos = new Vector3(60, 60, 0);
        Vector3 inialScale = playerCanvas.transform.localScale;
        actualMaxHealth = maxHealth;
        for (float i = actualMaxHealth; i > 0; i -= 10)
        {
            Transform health = Instantiate(healthBar);
            health.SetParent(GameObject.Find("Panel").transform);
            health.gameObject.SetActive(true);
            health.transform.position = startPos;
            startPos.x += 90;
        }
        dynamicHealthBar = GameObject.FindGameObjectsWithTag("HealthBar");
    }
    void updateScore()
    {
        score.text = NumberScore.ToString();
    }

    void deadCheck()
    {
        if(currentHealth <= 0)
        {
            loseGame();
        }
    }
    void Update()
    {
        updateScore();
        movement();
        abilityCheck();
        fireCheck();
        deadCheck();
    }

    void fireCheck()
    {
        if (Time.time > nextFire && Input.GetKey("up") || Time.time > nextFire && Input.GetKey("down") || Time.time > nextFire && Input.GetKey("left") || Time.time > nextFire && Input.GetKey("right"))
        {
            nextFire = Time.time + fireRate;
            Fire();
        }
    }
    void Fire()
    {
        misses++;
        Vector2 SwordPos;
        if (Input.GetKey("left") && Input.GetKey("up"))
        {
            SwordPos = new Vector3(gameObject.transform.position.x - 2.0f, gameObject.transform.position.y + 2f, 0);
            Instantiate(SecondSword, SwordPos, Quaternion.Euler(0, 0, 45));
        }
        else if (Input.GetKey("left") && Input.GetKey("down"))
        {
            SwordPos = new Vector3(gameObject.transform.position.x - 2.0f, gameObject.transform.position.y - 2f, 0);
            Instantiate(SecondSword, SwordPos, Quaternion.Euler(0, 0, 135));
        }
        else if (Input.GetKey("right") && Input.GetKey("up"))
        {
            SwordPos = new Vector3(gameObject.transform.position.x + 2.0f, gameObject.transform.position.y + 2f, 0);
            Instantiate(SecondSword, SwordPos, Quaternion.Euler(0, 0, -45));
        }
        else if (Input.GetKey("right") && Input.GetKey("down"))
        {
            SwordPos = new Vector3(gameObject.transform.position.x + 2.0f, gameObject.transform.position.y - 2f, 0);
            Instantiate(SecondSword, SwordPos, Quaternion.Euler(0, 0, -135));
        }
        else if (Input.GetKey("left"))
        {
            SwordPos = new Vector3(gameObject.transform.position.x - 2.7f , gameObject.transform.position.y , 0);
            Instantiate(SecondSword, SwordPos, Quaternion.Euler(0, 0, 90));
        }
        else if (Input.GetKey("right"))
        {
            SwordPos = new Vector3(gameObject.transform.position.x + 2.7f , gameObject.transform.position.y, 0);
            Instantiate(SecondSword, SwordPos, Quaternion.Euler(0, 0, -90));
        }
        else if (Input.GetKey("up"))
        {
            SwordPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.7f, 0);
            Instantiate(SecondSword, SwordPos, Quaternion.Euler(0, 0, 0));
        }
        else if (Input.GetKey("down"))
        {
            SwordPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 2.7f, 0);
            Instantiate(SecondSword, SwordPos, Quaternion.Euler(0, 0, 180));
        }
    }
    void abilityCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && countable == true)
        {
            if (chosen == false)
            {
                cooldown = cooldown + 10f;
                chosen = true;
            }
            ability();
            CountUp();
        }

        if (count == true)
        {
            if (currentAmmount < 100)
            {
                currentAmmount += (cooldown * 2) * Time.deltaTime;
                abilityBar.GetComponent<Image>().fillAmount = currentAmmount / 100;
            }
        }

        if (currentAmmount >= 100)
        {
            count = false;
            countable = true;
            currentAmmount = 0;
        }
    }
    void ability()
    {
        if (MyspriteRenderer.flipX)
        {
            BigSwordPos = new Vector3(gameObject.transform.position.x + -3f, gameObject.transform.position.y, 0);
            GameObject obj = Instantiate(abilityWeapon, BigSwordPos, Quaternion.Euler(0, 0, 90));
            obj.transform.parent = gameObject.transform;
        }
        else if (!MyspriteRenderer.flipX)
        {
            BigSwordPos = new Vector3(gameObject.transform.position.x + 3f, gameObject.transform.position.y, 0);
            GameObject obj = Instantiate(abilityWeapon, BigSwordPos, Quaternion.Euler(0, 0, -90));
            obj.transform.parent = gameObject.transform;
        }
    }
    void CountUp()
    {
        countable = false; // disable multiple presses
        count = true;
    }
    void movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            MyspriteRenderer.flipX = true;
            my_animator.SetBool("IsWalking", true);
            transform.Translate(new Vector3(-3, 0, 0) * Time.deltaTime * Mspeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            MyspriteRenderer.flipX = false;
            my_animator.SetBool("IsWalking", true);
            transform.Translate(new Vector3(3, 0, 0) * Time.deltaTime * Mspeed);
        }

        if (Input.GetKey(KeyCode.W))
        {

            my_animator.SetBool("IsWalking", true);
            transform.Translate(new Vector3(0, 3, 0) * Time.deltaTime * Mspeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            my_animator.SetBool("IsWalking", true);
            transform.Translate(new Vector3(0, -3, 0) * Time.deltaTime * Mspeed);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            my_animator.SetBool("IsWalking", true);
            transform.Translate(new Vector3(-1f, 1f, 0) * Time.deltaTime * Mspeed);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            my_animator.SetBool("IsWalking", true);
            transform.Translate(new Vector3(1f, 1f, 0) * Time.deltaTime * Mspeed);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            my_animator.SetBool("IsWalking", true);
            transform.Translate(new Vector3(1f, -1f, 0) * Time.deltaTime * Mspeed);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            my_animator.SetBool("IsWalking", true);
            transform.Translate(new Vector3(-1f, -1f, 0) * Time.deltaTime * Mspeed);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            my_animator.SetBool("IsWalking", false);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            my_animator.SetBool("IsWalking", false);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            my_animator.SetBool("IsWalking", false);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            my_animator.SetBool("IsWalking", false);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && hitable == true)
        {
            hitable = false;
            //print("Hit Enemy");
            Vector3 heading = new Vector3(0, 0, 0);
            heading = collision.transform.position - transform.position;
            gameObject.GetComponent<Rigidbody2D>().AddForce(-heading * collision.gameObject.GetComponent<EHealth>().knockBackStrength, ForceMode2D.Impulse); 
            currentHealth = currentHealth - collision.gameObject.GetComponent<EHealth>().enemyDamage;
            damageTaken = actualMaxHealth - currentHealth;
          
            if (currentHealth <= 0)
            {
                loseGame();
            }
            UpdateVisualHealth();
            StartCoroutine("waitForInvTime");
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EProjectile")
        {
            Vector3 heading = new Vector3(0, 0, 0);
            heading = collision.transform.position - transform.position;
            if (collision.gameObject.name == "Bone(Clone)")
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(-heading * collision.gameObject.GetComponent<Bone>().strength, ForceMode2D.Impulse);
                currentHealth = currentHealth - collision.gameObject.GetComponent<Bone>().damage;
            } else if (collision.gameObject.name == "Sponge(Clone)" || collision.gameObject.name == "SpongeV(Clone)")
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(-heading * collision.gameObject.GetComponent<EnemyProjectile>().strength, ForceMode2D.Impulse);
                currentHealth = currentHealth - collision.gameObject.GetComponent<EnemyProjectile>().damage;
            }
            damageTaken = actualMaxHealth - currentHealth;

            if (currentHealth <= 0)
            {
                loseGame();
            }
            UpdateVisualHealth();
            StartCoroutine("waitForInvTime");
        }
    }

    IEnumerator waitForInvTime()
    {
        yield return new WaitForSeconds(graceTime);
        hitable = true;
    }
    void UpdateVisualHealth()
    {
        if(damageTaken >= actualMaxHealth)
        {
            for (int i = 0; i < dynamicHealthBar.Length; i++)
            {
                dynamicHealthBar[i].gameObject.GetComponent<Image>().fillAmount = 0;
            }
        } else
        {
            float threshold = 10;
            while (damageTaken > threshold)
            {
                dynamicHealthBar[dynamicHealthBar.Length - (int)threshold / 10].gameObject.GetComponent<Image>().fillAmount = (threshold - damageTaken) / 10;
                threshold += 10;
            }
            dynamicHealthBar[dynamicHealthBar.Length - (int)threshold/10].gameObject.GetComponent<Image>().fillAmount = (threshold - damageTaken) / 10;
        }
    }
    
    public float advTimes()
    {
        float j = 0;
        for(int i = 0; i < times.Count; i++)
        {
            j+=times[i];
        }
        j = j / times.Count;
        times.Clear();
        return j;
    }
    void printDiff()
    {
        if (rooms.roomDiff <= 50)
        {
            print("Easy: " + rooms.roomDiff);
        } else if (rooms.roomDiff <= 100)
        {
            print("Medium: "+ rooms.roomDiff);
        } else
        {
            print("Hard; " + rooms.roomDiff);
        }

    }
    void loseGame()
    {
        SceneManager.LoadScene("CharacterSelect");
        maxHealth = actualMaxHealth;
        //Print what diff they where playing at
        if (PlayerPrefs.GetInt("Difficulty") == 0) 
        {
            printDiff();
        }
    }
}
