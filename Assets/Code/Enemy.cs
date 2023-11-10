using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    bool isLive = true;

    public EnemySpawner spawner;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;

    public GameObject healthbar;
    public Slider slider;
    public Text text;
    private RectTransform canvasTransform;
    private RectTransform healthBarRectTransform;

    public float knockbackPower;
    public float knockbackDuration;


    public void SetSpawner(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }


    public void takeDamage(int damage){
        health-=damage;
        HealthUpdate();
        if(health<=0){
            Destroy(gameObject);
            Destroy(slider.gameObject);
            spawner.enemiesLeft -=1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
            GameObject otherGameObject = collision.gameObject;
            // Debug.Log("Collided with: " + otherGameObject.name);
            // call enemy take damage function
            
            if (otherGameObject.CompareTag("Player")){     
                    otherGameObject.GetComponent<Player>().takeDamage(10);
                    StartCoroutine(Player.instance.Knockback(knockbackDuration, knockbackPower, this.transform));
                }
        }



    void Awake()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        GameObject canvasObject = GameObject.Find("Canvas");
        if (playerObject != null)
        {
            // Get the Rigidbody2D component of the player object and assign it to the target variable.
            target = playerObject.GetComponent<Rigidbody2D>();
        }
        if(canvasObject != null) {
            canvasTransform = canvasObject.GetComponent<RectTransform>();
        }
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();

        GameObject healthBarInstance = Instantiate(healthbar);
        if(canvasTransform != null) {
            healthBarInstance.transform.SetParent(canvasTransform);
        }

        slider = healthBarInstance.GetComponentInChildren<Slider>();
        text = healthBarInstance.GetComponentInChildren<Text>();

        healthBarRectTransform = healthBarInstance.GetComponent<RectTransform>();

        slider.maxValue = maxHealth;

        HealthUpdate();
    }

    void FixedUpdate()
    {
        if (!isLive)
            return;


        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        Vector3 enemyScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 screenPos;

        if(canvasTransform != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, enemyScreenPos, null, out screenPos)){
            if(healthBarRectTransform != null) {
                healthBarRectTransform.localPosition = screenPos + new Vector2(0f, 30f);
            }
            slider.value = health;
            text.text = (health.ToString() + " / " + maxHealth.ToString());
            
        }
    }

    void LateUpdate()
    {
        if (!isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void HealthUpdate() {
        slider.value = health;
        text.text = $"{health} / {maxHealth}";
    }
    // private void OnEnable()
    // {
    //     target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    //     isLive = true;
    //     health = maxHealth;
    // }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }





}
