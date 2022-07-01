using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] powerUpPrefab = new GameObject[3];
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject powerupMissile;
    [SerializeField] private GameObject _spawnManager;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private GameObject ewEnemy;
    [SerializeField] private GameObject repairPrefab;
    [SerializeField] private GameObject bomber;
    [SerializeField] private GameObject boss;

    //Public variables
    public bool startGame;
    public bool stopGame;
    public bool canSpawnBoss;
    private bool canSpawnEnemy = true;
    private bool canSpawnEWenemy = true;
    private bool canSpawnBomber = true;



    //These fixed variables will need adjusting after Testing
    private float bomberLaunchTime = 60f; 
    private float bomberDelay;
    private float bomberDelayMin = 60f;
    private float bomberDelayMax = 90f;

    private float enemyLaunchTime = 5f;
    private float enemyDelay;
    private float enemyDelayMin = .5f;
    private float enemyDelayMax = 4f;
    private float enemyDelayMax2 = 2f;
    private float enemyDelayMax3 = 1.5f;
    private float enemyDelayMax4 = .75f;

    private float EWenemyLaunchTime = 20f;
    private float EWenemyDelay;
    private float EWenemyDelayMin = 45;
    private float EWenemyDelayMax = 75;

    private float ammoLaunchTime = 10f;
    private float ammoPowerupDelay;
    private float ammoPowerupDelayMin = 20f;
    private float ammoPowerupDelayMax = 40f;

    private float shieldPowerupLaunchTime =45f; 
    private float shieldPowerupDelay;
    private float shieldPowerupDelayMin = 30f; 
    private float shieldPowerupDelayMax = 60f;

    private float tpsPowerupLaunchTime = 20f;
    private float tpsPowerupDelay;
    private float tpsPowerupDelayMin = 30f;
    private float tpsPowerupDelayMax = 75f;

    private float speedPowerupLaunchTime = 30f;
    private float speedPowerupDelay;
    private float speedPowerupDelayMin = 30f;
    private float speedPowerupDelayMax = 75f;

    private float repairLaunchTime = 60f;
    private float repairDelay;
    private float repairDelayMin = 45f;
    private float repairDelayMax = 75f;

    private float bossLaunchTime = 60f;


    // Start is called before the first frame update
    void Start() {
        _enemyPrefab = Resources.Load<GameObject>("Enemy");
        powerUpPrefab[0] = Resources.Load<GameObject>("PowerUp_TrippleShot");
        powerUpPrefab[1] = Resources.Load<GameObject>("PowerUp_Speed");
        powerUpPrefab[2] = Resources.Load<GameObject>("PowerUp_Shield");
        repairPrefab = Resources.Load<GameObject>("PowerupRepair");
        ammoPrefab = Resources.Load<GameObject>("PowerupAmmo");
        powerupMissile = Resources.Load<GameObject>("Powerup_Missile");
        ewEnemy = Resources.Load<GameObject>("EWEnemy");
        bomber = Resources.Load<GameObject>("EnemyBomber");
        boss = Resources.Load<GameObject>("BossShip");
        
        _spawnManager = GameObject.Find("SpawnManager"); //GameObject.Find(string name) for parent objects
        _enemyContainer = _spawnManager.transform.Find("/SpawnManager/EnemyContainer");

    }

    private void Update()  {

        if(startGame)  {

            //Spawn Enemies
            SpawnEnemyBomber();
            SpawnEnemy();
            SpawnEWEnemy();
            SpawnBossEnemy();


            //Spawn Powerups
            SpawnAmmoPowerup();
            SpawnShieldPowerup();
            SpawnSpeedPowerup();
            SpawnTrippleshotPowerup();
            SpawnRepairPowerup();
        }

        if(stopGame)  {
            canSpawnEnemy = false;
            canSpawnBomber = false;
            canSpawnEWenemy = false;
            
        }

    }

    private void SpawnBossEnemy() {
        //set canSpawnBoss outside of this method, otherwise will override 
        //external setting
        if(canSpawnBoss && Time.time > bossLaunchTime) {
         
            //Debug.Log("Spawn Boss Enemy");
            GameObject bossEnemy = Instantiate(boss, new Vector3(0, 8, 0), Quaternion.identity);
            bossEnemy.transform.parent = _enemyContainer.transform;
            StartCoroutine(BossSpawning());
        }
 
    }

    private void SpawnEnemy() {

        if(Time.time >= enemyLaunchTime && canSpawnEnemy)  {
            float xPos = Random.Range(-8f, 8f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(xPos, 8, 0), Quaternion.identity);

            if(Time.time < 60) {
                enemyDelay = Random.Range(enemyDelayMin, enemyDelayMax);
                enemyLaunchTime = Time.time + enemyDelay;
            }

            if(Time.time >= 60 && Time.time < 120)  {
                enemyDelay = Random.Range(enemyDelayMin, enemyDelayMax2);
                enemyLaunchTime = Time.time + enemyDelay;
            }
            if(Time.time >= 120) {
                enemyDelay = Random.Range(enemyDelayMin, enemyDelayMax3);
                enemyLaunchTime = Time.time + enemyDelay;
            }

            if(Time.time >= 180) {
                enemyDelay = Random.Range(.5f, enemyDelayMax4);
                enemyLaunchTime = Time.time + enemyDelay;
            }

            if(Time.time >= 240)  {
                enemyDelay = Random.Range(.25f, enemyDelayMax4);
                enemyLaunchTime = Time.time + enemyDelay;
            }

 
        }

    }

    private void SpawnEWEnemy() {
        if (Time.time >= EWenemyLaunchTime && canSpawnEWenemy)  {
            GameObject newEnemy = Instantiate(ewEnemy, new Vector3(0, 8, 0), Quaternion.Euler(0, 0, 180f));
            newEnemy.transform.parent = _enemyContainer.transform;
            EWenemyDelay = Random.Range(EWenemyDelayMin, EWenemyDelayMax);
            EWenemyLaunchTime = Time.time + EWenemyDelay;
            StartCoroutine(EWEnemySpawning());
        }
    }


    private void SpawnEnemyBomber() {

        if(Time.time >= bomberLaunchTime && canSpawnBomber) {
            //Think about implementing all the other spawn routines inside of
            //regular methods with timers.
            //Then set canSpawn bools inside of those
            Instantiate(bomber, new Vector3(-7.75f, 8, 0), Quaternion.identity);
            Instantiate(bomber, new Vector3(7.75f, 8, 0), Quaternion.identity);
            bomberDelay = Random.Range(bomberDelayMin, bomberDelayMax);
            bomberLaunchTime = Time.time + bomberDelay;
            StartCoroutine(BomberSpawning());
        }
    }

    IEnumerator BossSpawning() {
        canSpawnEnemy = false;
        canSpawnEWenemy = false;
        canSpawnBomber = false;
        canSpawnBoss = false;
        yield return new WaitForSeconds(1);
    }

    IEnumerator EWEnemySpawning() {
        canSpawnEnemy = false;
        canSpawnBomber = false;
        canSpawnBoss = false;
        yield return new WaitForSeconds(10);
        canSpawnEnemy = true;
        canSpawnBomber = true;
        canSpawnBoss = true;
    }

    IEnumerator BomberSpawning() {
        canSpawnEnemy = false;
        canSpawnEWenemy = false;
        canSpawnBoss = false;
        yield return new WaitForSeconds(10);
        canSpawnEnemy = true;
        canSpawnEWenemy = true;
        canSpawnBoss = true;

    }

    private void SpawnRepairPowerup() {
        if (Time.time >= repairLaunchTime)
        {
            float xPos = Random.Range(-8f, 8f);
            Instantiate(repairPrefab, new Vector3(xPos, 8, 0), Quaternion.identity);
            repairDelay = Random.Range(repairDelayMin, repairDelayMax);
            repairLaunchTime = Time.time + repairDelay;
        }
    }

    private void SpawnAmmoPowerup()  {

        if (Time.time >= ammoLaunchTime)  {
            float xPos = Random.Range(-8f, 8f);
            Instantiate(ammoPrefab, new Vector3(xPos, 8, 0), Quaternion.identity);
            ammoPowerupDelay = Random.Range(ammoPowerupDelayMin, ammoPowerupDelayMax);
            ammoLaunchTime = Time.time + ammoPowerupDelay;
        }

    }

    private void SpawnShieldPowerup() {

        if (Time.time >= shieldPowerupLaunchTime) {
            float xPos = Random.Range(-8f, 8f);
            Instantiate(powerUpPrefab[2], new Vector3(xPos, 8, 0), Quaternion.identity);
            shieldPowerupDelay = Random.Range(shieldPowerupDelayMin, shieldPowerupDelayMax);
            shieldPowerupLaunchTime = Time.time + shieldPowerupDelay;
        }

    }

    private void SpawnTrippleshotPowerup() {

        if(Time.time >= tpsPowerupLaunchTime) {
            float xPos = Random.Range(-8f, 8f);
            Instantiate(powerUpPrefab[0], new Vector3(xPos, 8, 0), Quaternion.identity);
            tpsPowerupDelay = Random.Range(tpsPowerupDelayMin, tpsPowerupDelayMax);
            tpsPowerupLaunchTime = Time.time + tpsPowerupDelay;
        }

    }

    private void SpawnSpeedPowerup() {

        if (Time.time >= speedPowerupLaunchTime) {
            float xPos = Random.Range(-8f, 8f);
            Instantiate(powerUpPrefab[1], new Vector3(xPos, 8, 0), Quaternion.identity);
            speedPowerupDelay = Random.Range(speedPowerupDelayMin, speedPowerupDelayMax);
            speedPowerupLaunchTime = Time.time + speedPowerupDelay;
        }

    }


    public void StopSpawning() {
        stopGame = true;
        startGame = false;
    }

}
