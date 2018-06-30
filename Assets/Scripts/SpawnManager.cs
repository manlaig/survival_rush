using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemy, explosive, expandWeapon;

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

        timerSpawnCircle = 0f;
        spawning = false;
	}

    void Update()
    {
        if (gameManager.isGameStarted() && !spawning)
            StartSpawning();

        if (!player.activeInHierarchy && gameManager.isGameStarted())
            StopSpawning();
        
        if (gameManager.isGameStarted() && !gameManager.isGameOver() && difficultyManager.GetDifficulty() == (int)Difficulty.HARD)
            timerSpawnCircle += Time.deltaTime;

        if (timerSpawnCircle >= 10f)
        {
            timerSpawnCircle = 0f;
            StartCoroutine("spawnCircle");  // call this coroutine to spawn enemies in a circle
        }
    }

    void StartSpawning()
    {
        spawning = true;
        float enemySpawnDelay = 0.5f, explosiveSpawnDelay = 10.0f, expandSpawnDelay = 7.0f;
        if (difficultyManager.GetDifficulty() == (int)Difficulty.EASY)
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
        Debug.Log("Spawning");
        Vector3 pos = new Vector3(Random.Range(walls.left.position.x, walls.right.position.x), 0f, Random.Range(walls.top.position.z, walls.bottom.position.z));
        GameObject spawnedEnemy =  Instantiate(enemy, pos, Quaternion.Euler(Vector3.zero));

        if(difficultyManager.GetDifficulty() == (int) Difficulty.EASY)
            spawnedEnemy.GetComponent<EnemyMovement>().setSpeed(5f);
    }

    void spawnKinematicEnemy(Vector3 pos)
    {
        GameObject staticEnemy = Instantiate(enemy, pos, Quaternion.Euler(Vector3.zero));
        staticEnemy.GetComponent<EnemyMovement>().enabled = false;
        staticEnemy.AddComponent<StaticEnemyMover>();
    }

    void instantSpawnEnemies(int countToSpawn)
    {
        for (int i = 0; i < countToSpawn; i++)
            spawnEnemy();
    }

    void spawnExplosive()
    {
        Vector3 pos = new Vector3(Random.Range(walls.left.position.x, walls.right.position.x), 0f, Random.Range(walls.top.position.z, walls.bottom.position.z));
        Instantiate(explosive, pos, Quaternion.Euler(Vector3.zero));
    }

    void spawnExpandWeapon()
    {
        Vector3 pos = new Vector3(Random.Range(walls.left.position.x, walls.right.position.x), 0f, Random.Range(walls.top.position.z, walls.bottom.position.z));
        Instantiate(expandWeapon, pos, Quaternion.Euler(Vector3.zero));
    }

    void spawnVerticalWall()
    {
        float topEdge = walls.top.position.z;
        float bottomEdge = walls.bottom.position.z;
        float leftEdge = walls.left.position.x;

        for (float i = bottomEdge + 1f; i < topEdge; i++)
            spawnKinematicEnemy(new Vector3(leftEdge + 0.5f, 0f, i));
    }

    IEnumerator spawnCircle()
    {
        // this method can spawn enemies outside the walls, be careful of that
        if (player.activeInHierarchy)
        {
            float radius = 8.0f;
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
}
