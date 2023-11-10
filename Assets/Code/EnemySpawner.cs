using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;
    public Transform playerTransform;
    public float spawnDistance = 20f;
    public float spawnInterval = 1.2f;
    public bool spawnOn = false;
    private Coroutine spawnCoroutine;
    public int round;
    public int enemiesLeft;
    private List<int> spawnList;
    private bool isDelaying = false;


    private void Start()
    {
        // Start spawning enemies periodically
        round = 0;
        spawnList = new List<int> { 2, 5, 8, 18, 25, 40};
        enemiesLeft = 0;
    }

    private void Update(){
        if (spawnOn && enemiesLeft == 0 && spawnCoroutine == null && !isDelaying)
        {
            StartCoroutine(DelayBeforeNextRound(8f));
        }
    }

    private IEnumerator DelayBeforeNextRound(float hold)
    {
        isDelaying = true;
        yield return new WaitForSeconds(hold);
        isDelaying = false;

        // Now you can start the next round or perform any other actions
        round += 1;
        setSpawnOn();
    }
    
    public void setSpawnOn(){
        Debug.Log("called Spawn On");
        spawnOn = true;
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    public void setSpawnOff(){
        Debug.Log("called Spawn Off");
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        
    }

    private IEnumerator SpawnEnemies()
    {   
        
        while (spawnList[round]>0)
        {   
            float innerCircleRadius = 15f;  // Adjust this value as needed
            float outerCircleRadius = spawnDistance;

            // Generate a random angle
            float randomAngle = Random.Range(0f, 360f);
            
            // Calculate the spawn point on the edge of the outer circle
            Vector2 randomSpawnPoint = new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad) * outerCircleRadius,
                Mathf.Sin(randomAngle * Mathf.Deg2Rad) * outerCircleRadius
            );

            Vector3 spawnPosition = new Vector3(randomSpawnPoint.x, 0f, randomSpawnPoint.y) + playerTransform.position;

            // Ensure the spawn point is outside the inner circle
            if (Vector2.Distance(randomSpawnPoint, Vector2.zero) >= innerCircleRadius)
            {
                if (Random.value<.2f){
                    GameObject newEnemy = Instantiate(enemyPrefab2, spawnPosition, Quaternion.identity);
                    Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
                    if (enemyComponent != null)
                {
                    enemyComponent.SetSpawner(this); // Set the spawner reference for the newEnemy
                }
                }else{
                    GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
                    if (enemyComponent != null)
                {
                    enemyComponent.SetSpawner(this); // Set the spawner reference for the newEnemy
                }
                }
                
                

                enemiesLeft = enemiesLeft + 1;
                // Wait for the specified interval before spawning the next enemy
                yield return new WaitForSeconds(spawnInterval);
                spawnList[round] = spawnList[round]-1;
            }
        }
        setSpawnOff();  
    }
}
