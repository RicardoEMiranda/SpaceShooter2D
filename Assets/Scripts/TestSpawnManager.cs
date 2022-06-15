using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnManager : MonoBehaviour {

    //Declare an instance of a prefab enemy object
    [SerializeField] private GameObject _enemyPrefab;
    //private Vector3[,] _spawnPointArray = new Vector3[3,3];
    private Vector3 _spawnPoint;

    //Declare an instance of the Spawn Manager
    private GameObject _spawnManager;

    //Declare an instance of the child Enemy Container
    private Transform _enemyContainer;

    // Start is called before the first frame update
    void Start() {

        //Set the instance of the SpawnManager
        _spawnManager = GameObject.Find("SpawnManager");

        //Set the instance of the child Enemy Container
        _enemyContainer = _spawnManager.transform.Find("/SpawnManager/EnemyContainer");

        //initialize the enemy prefab instance that's located 
        //in the Resources folder
        _enemyPrefab = Resources.Load<GameObject>("Enemy2");
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine() {

        while (true) {
            //Instantiate enemy prefabs in the scene, once every second
            //so that it instantiates at (0, 0) and begins to move in a random direction
            //make the enemy prefab a child of the EnemyContainer

            for (int i = 0; i<10; i++) {

                    //Set a random spawn position inside of the game scene [-42 <= x <= 42] & [-20 <= y ,= 20]
                    _spawnPoint = new Vector3(Random.Range(-42f, 42f), Random.Range(-20f, 20f), 0f);

                    //Set the newEnemy.transform.parent = _enemyContainer.transform
                    //Instantiate(_enemyPrefab, _spawnPointArray[i, j], Quaternion.identity).transform.parent = _enemyContainer.transform;
                    Instantiate(_enemyPrefab, _spawnPoint, Quaternion.identity).transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(.25f);
        }       

    }
}
