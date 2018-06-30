using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemy, explosive, expandWeapon;

    // number of active weapons in the scene, this number is reduced in other classed as the player hits them
    public static int weaponsActiveCount;

    GameObject player;
    GameManager gameManager;
    WallColliderPosition walls;
    DifficultyManager difficultyManager;

    float timerSpawnCircle;
    bool spawning;


	void Start ()
    {
        player = GameObject.Find("Player");
        walls = GameObject.FindGameObjectWithTag("Walls").GetComponent<WallColliderPosition>();
        difficultyManager = GameObject.Find("Difficulties").GetComponent<DifficultyManager>();
        gameManager = GetComponent<GameManager>();

        weaponsActiveCount = 0;
        timerSpawnCircle = 0f;
        spawning = false;
        spawnVerticalWall();
        //instantSpawnEnemies(10);
	}


    void Update()
    {
        if (gameManager.isGameStarted() && !spawning)
            StartSpawning(difficultyManager.GetDifficulty());

        if (!player.activeInHierarchy && gameManager.isGameStarted())
            StopSpawning();
        
        /*
        if (gameManager.isGameStarted() && !gameManager.isGameOver() && difficultyManager.GetDifficulty() == (int)Difficulty.HARD)
            timerSpawnCircle += Time.deltaTime;

        if (timerSpawnCircle >= 10f)
        {
            timerSpawnCircle = 0f;
            StartCoroutine(spawnCircle(Random.Range(5f, 8f));  // call this coroutine to spawn enemies in a circle
        }
        */
    }


    void StartSpawning(int difficulty)
    {
        spawning = true;

        float enemySpawnDelay = 0.5f, explosiveSpawnDelay = 10.0f, expandSpawnDelay = 7.0f;
        if (difficulty == (int)Difficulty.EASY)
        {
            enemySpawnDelay = 2f;
            explosiveSpawnDelay = 7f;
            expandSpawnDelay = 5f;
        }
        InvokeRepeating("spawnEnemy", 1.0f, enemySpawnDelay);
        InvokeRepeating("spawnExplosive", 3.0f, explosiveSpawnDelay);
        InvokeRepeating("spawnExpandWeapon", 2.0f, expandSpawnDelay);
    }


    void StopSpawning()
    {
        CancelInvoke("spawnEnemy");
        CancelInvoke("spawnExplosive");
        CancelInvoke("spawnExpandWeapon");
    }


    void spawnEnemy()
    {
        //Debug.Log("Spawning");
        Vector3 pos = GetRandomLocation();
        GameObject spawnedEnemy =  Instantiate(enemy, pos, Quaternion.Euler(Vector3.zero));

        if(difficultyManager.GetDifficulty() == (int) Difficulty.EASY)
            spawnedEnemy.GetComponent<EnemyMovement>().setSpeed(5f);
    }


    void instantSpawnEnemies(int countToSpawn)
    {
        for (int i = 0; i < countToSpawn; i++)
            spawnEnemy();
    }


    void spawnExplosive()
    {
        if (weaponsActiveCount < 3)
        {
            weaponsActiveCount++;
            Vector3 pos = GetRandomLocation();
            Instantiate(explosive, pos, Quaternion.Euler(Vector3.zero));
        }
    }


    void spawnExpandWeapon()
    {
        if (weaponsActiveCount < 3)
        {
            weaponsActiveCount++;
            Vector3 pos = GetRandomLocation();
            Instantiate(expandWeapon, pos, Quaternion.Euler(Vector3.zero));
        }
    }


    void spawnVerticalWall()
    {
        float topEdge = walls.top.position.z;
        float bottomEdge = walls.bottom.position.z;
        float leftEdge = walls.left.position.x;

        for (float i = bottomEdge + 1f; i < topEdge; i++)
            spawnKinematicEnemy(new Vector3(leftEdge + 0.5f, 0f, i));
    }


    void spawnKinematicEnemy(Vector3 pos)
    {
        GameObject staticEnemy = Instantiate(enemy, pos, Quaternion.Euler(Vector3.zero));
        staticEnemy.GetComponent<EnemyMovement>().enabled = false;
        staticEnemy.AddComponent<StaticEnemyMover>();
    }


    IEnumerator spawnCircle(float radius = 8.0f)
    {
        // this method can spawn enemies outside the walls, be careful of that
        if (player.activeInHierarchy)
        {
            Vector3 pos;

            for (int i = 0; i < 360; i += 45)
            {
                pos.x = player.transform.position.x + radius * Mathf.Sin(i * Mathf.Deg2Rad);
                pos.y = 0f;
                pos.z = player.transform.position.z + radius * Mathf.Cos(i * Mathf.Deg2Rad);

                Instantiate(enemy, pos, Quaternion.Euler(Vector3.zero));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }


    // this function will return a vector that is not too close to the player and not outside the wall
    Vector3 GetRandomLocation()
    {
        Vector3 pos = RandomVector();
        while (VectorTooClose(pos))
            pos = RandomVector();
        return pos;
    }


    Vector3 RandomVector()
    {
        return new Vector3(Random.Range(walls.left.position.x + 0.5f, walls.right.position.x - 0.5f), 0f, Random.Range(walls.top.position.z - 0.5f, walls.bottom.position.z + 0.5f));
    }


    // check whether a vector is too close to the player
    bool VectorTooClose(Vector3 pos)
    {
        Vector3 res = player.transform.position - pos;
        if (Mathf.Abs(res.x) > 2f || Mathf.Abs(res.z) > 2f)
            return false;
        return true;
    }
}
