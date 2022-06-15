using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] powerUpPrefab = new GameObject[3];
    [SerializeField] private GameObject _spawnManager;
    [SerializeField] private Transform _enemyContainer;
    private bool _playerIsDead;

    // Start is called before the first frame update
    void Start() {
        _enemyPrefab = Resources.Load<GameObject>("Enemy");
        powerUpPrefab[0] = Resources.Load<GameObject>("PowerUp_TrippleShot");
        powerUpPrefab[1] = Resources.Load<GameObject>("PowerUp_Speed");
        powerUpPrefab[2] = Resources.Load<GameObject>("PowerUp_Shield");
        //_powerUpPrefab = Resources.Load<GameObject>("PowerUp_TrippleShot");
        //_spawnManager = Resources.Load<GameObject>("SpawnManager");
        _spawnManager = GameObject.Find("SpawnManager"); //GameObject.Find(string name) for parent objects
        _enemyContainer = _spawnManager.transform.Find("/SpawnManager/EnemyContainer");

    }

    // Update is called once per frame
    void Update() {
 

    }

    public void ContinueSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnTrippleShotPowerUp());
    }

    IEnumerator SpawnEnemyRoutine() {

        yield return new WaitForSeconds(3.0f);
        //yield return null; //wait 1 frame
        //then will go to this line

        //yield return new WaitForSeconds(5.0f); //wait 5 seconds
        //then goes to this line

        while (!_playerIsDead) {
            float xPos = Random.Range(-8f, 8f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(xPos, 8, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            float delay = Random.Range(1, 6);
            yield return new WaitForSeconds(delay);
        }


        //Instantiate(enemy, new Vector3(-4, 8, 0), Quaternion.identity);
    }

    IEnumerator SpawnTrippleShotPowerUp() {
        yield return new WaitForSeconds(3.0f);

        while (!_playerIsDead) {
            float xPos = Random.Range(-8f, 8f);
            float delayTime = Random.Range(15, 30);

            int prefabID = Random.Range(0, 3);
            if (prefabID == 2)  {
                prefabID = Random.Range(1, 3);
            }

            GameObject newPowerUp = Instantiate(powerUpPrefab[prefabID], new Vector3(xPos, 8, 0), Quaternion.identity);
            newPowerUp.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(delayTime);
        }
    }

    public void StopSpawning() {
        _playerIsDead = true;
    }
}
