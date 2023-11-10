using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineGun : MonoBehaviour
{
    public string gun = "auto";

    void OnCollisionStay2D(Collision2D collision) {
        Debug.Log("here");
        if (collision.gameObject.CompareTag("Player"))
        {
            shooting shootingScript = collision.gameObject.GetComponent<shooting>();
            if (shootingScript != null)
            {
                
                shootingScript.SetGun("auto");

                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
