using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    private bool isFiring = false;
    public float bulletForce = 26f;
    private Coroutine shootCoroutine;
    private float lastShotTime;
    public float fireRate = 0.1f;
    public string gun;

    public void SetGun(string newGun)
    {
        gun = newGun;
    }

    void Update()
    {
        if (gun == "pistol"){
            if(Input.GetButtonDown("Fire1")){
            Shoot();
        }
        }
        else if (gun == "auto"){
            if(Input.GetButtonDown("Fire1")){
            isFiring = true;
            shootCoroutine = StartCoroutine(shootAuto());
            }
            if(Input.GetButtonUp("Fire1")){
                isFiring = false;
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
        }
        
    }

    void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right* bulletForce, ForceMode2D.Impulse);
        
    }
    private IEnumerator shootAuto(){
        
        while (isFiring){
            Debug.Log("here");
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right* bulletForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(.5f);
        }
        
    }
}
