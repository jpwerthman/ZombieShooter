using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Vector2 inputVec;
    public float health;
    public float maxHealth;
    //public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    public Vector2 movement;
    public Vector2 mousePos;
    public float speed;
    public Rigidbody2D rb;
    public Camera cam;
    private SpriteRenderer sprite; 
    public static Player instance;

    public Slider slider;
    public Text text;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable() //different character anim
    {
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>(); //Get the SpriteRenderer component
        health = maxHealth;
    }
        

    public void takeDamage(int damage){
        health -= damage;
        Debug.Log(health);
        if(health<=0){  
        SceneManager.LoadScene("Game Over Screen");
        }
    
    
    }
    void HealthUpdate() {
        slider.value = health;
        text.text = $"{health} / {maxHealth}";
        Debug.Log("slider value");
        Debug.Log(slider.value);
    }


    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos-rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        if( lookDir.x<0){
            sprite.flipY = true;
        }
        else{
            sprite.flipY = false;
        }
    }   
    

    public IEnumerator Knockback(float knockbackDuration, float knockbackPower, Transform obj){
        HealthUpdate();
        float timer = 0;
        Debug.Log(knockbackPower);
        Debug.Log(knockbackDuration);
        while(knockbackDuration>timer){
            timer+= Time.deltaTime;
            Vector2 direction = (obj.transform.position - this.transform.position).normalized;
            rb.AddForce(-direction * knockbackPower);
        }
        yield return 0;
    }

    void OnCollisionStay2D(Collision2D collision) // when player die
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            for (int index=2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead"); // Dead animation

            GameManager.instance.GameOver();
        }
    }
}
