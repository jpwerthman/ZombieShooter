using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision){
         GameObject otherGameObject = collision.gameObject;
        // Debug.Log("Collided with: " + otherGameObject.name);
        // call enemy take damage function
        if (otherGameObject.CompareTag("Enemy")){  
                otherGameObject.GetComponent<Enemy>().takeDamage(50);
            }
        Destroy(gameObject);
    }
}
