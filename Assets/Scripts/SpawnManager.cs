using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemy, explosive, expandWeapon, shieldWeapon;

    // number of active weapons in the scene, this number is reduced in other classes as the player hits them
    public static int weaponsActiveCount;

    protected GameObject player;
    protected WallColliderPosition walls;

    int maxAllowedWeapons = 3;
    float enemySpawnDelay = 1.1f;
    float obstacleSpawnDelay = 12.5f;
    float explosiveSpawnDelay = 8f, initialSpawn = 10f;
    float expandSpawnDelay = 7f, shieldSpawnDelay = 13f;


	void Start ()
    {
        player = GameObject.Find("Player");
        walls = GameObject.FindGameObjectWithTag("Walls").GetComponent<WallColliderPosition>();

        weaponsActiveCount = 0;
	}


    public void StartSpawning()
    {
        if (DifficultyManager.difficulty == (int)Difficulty.EASY)
        {
            maxAllowedWeapons = 2;
            enemySpawnDelay = 2f;
            explosiveSpawnDelay = 10f;
            expandSpawnDelay = 9f;
            shieldSpawnDelay = 17f;
            obstacleSpawnDelay = 14.5f;
            initialSpawn = 13f;
        }
        else if (DifficultyManager.difficulty == (int)Difficulty.MEDIUM)
        {
            enemySpawnDelay = 1.5f;
            explosiveSpawnDelay = 9f;
            expandSpawnDelay = 8f;
            shieldSpawnDelay = 15f;
            obstacleSpawnDelay = 13.5f;
            initialSpawn = 12f;
            instantSpawnEnemies(Random.Range(1, 6));
        }
        else
            instantSpawnEnemies(Random.Range(5, 10));

        StartCoroutine("ExplosiveCoroutine");
        StartCoroutine("ExpandCoroutine");
        StartCoroutine("ShieldCoroutine");
        StartCoroutine("EnemySpawnCoroutine");
        StartCoroutine("RandomObstacleCoroutine");
    }


    public void StopSpawning()
    {
        StopCoroutine("ExplosiveCoroutine");
        StopCoroutine("ExpandCoroutine");
        StopCoroutine("ShieldCoroutine");
        StopCoroutine("EnemySpawnCoroutine");
        StopCoroutine("RandomObstacleCoroutine");
    }


    void SpawnRandomObstacle()
    {
        int rand = Random.Range(0, 11);
        float timeSinceStarted = Time.time - TimeElapsed.initialTime;

        if (rand == 1 || rand == 5 || rand == 8)
            StartCoroutine(spawnCircle(Random.Range(4f, 8f)));
        else if (rand == 2 && (timeSinceStarted > 60 || GameManager.score > 7000))
        {
            Debug.Log("spawned double vertical wall at time: " + timeSinceStarted + " and score: " + GameManager.score);
            spawnDoubleVerticalWall(0.7f);
            obstacleSpawnDelay += 2f;
            enemySpawnDelay += 0.5f;
            spawnExplosive();
        }
        else if (rand == 7 && (timeSinceStarted > 60 || GameManager.score > 7000))
        {
            Debug.Log("spawned double horizontal wall at time: " + timeSinceStarted + " and score: " + GameManager.score);
            spawnDoubleHorizontalWall(0.7f);
            obstacleSpawnDelay += 2f;
            enemySpawnDelay += 0.5f;
            spawnExplosive();
        }
        else if (rand == 6 && (timeSinceStarted > 90 || GameManager.score > 10000))
        {
            Debug.Log("spawned 4 walls at time: " + timeSinceStarted + " and score: " + GameManager.score);
            spawnDoubleVerticalWall(0.5f);
            spawnDoubleHorizontalWall(0.5f);
            obstacleSpawnDelay += 5f;
            enemySpawnDelay += 1f;
            spawnShield();
        }
        else if (rand == 3 && (timeSinceStarted > 45 || GameManager.score > 3000))
            spawnVerticalWall(Random.Range(0, 2) == 0 ? "right" : "left", 1.5f, (DifficultyManager.difficulty == 2) ? 1.5f : 1f);
        else if (rand == 4 && (timeSinceStarted > 45 || GameManager.score > 3000))
            spawnHorizontalWall(Random.Range(0, 2) == 0 ? "up" : "down", (DifficultyManager.difficulty == 2) ? 1.5f : 1f);
        else
            instantSpawnEnemies(SpawnHelpers.GetRangeForInstantSpawn());
    }


    void spawnDoubleVerticalWall(float speed)
    {
        float step = 1f;
        if (DifficultyManager.difficulty == 0 || DifficultyManager.difficulty == 1)
            step = 2f;
        spawnVerticalWall("right", step, speed);
        spawnVerticalWall("left", step, speed);
    }


    void spawnDoubleHorizontalWall(float speed)
    {
        float step = 1f;
        if (DifficultyManager.difficulty == 0 || DifficultyManager.difficulty == 1)
            step = 2f;
        spawnHorizontalWall("up", step, speed);
        spawnHorizontalWall("down", step, speed);
    }


    void instantSpawnEnemies(int countToSpawn)
    {
        for (int i = 0; i < countToSpawn; i++)
            spawnEnemy();
    }


    void spawnEnemy()
    {
        Vector3 pos = SpawnHelpers.GetRandomLocation(walls, player);
        GameObject spawnedEnemy =  Instantiate(enemy, pos, Quaternion.Euler(Vector3.zero));

        if(DifficultyManager.difficulty == (int) Difficulty.EASY)
            spawnedEnemy.GetComponent<EnemyMovement>().setSpeed(4f);
        if (DifficultyManager.difficulty == (int)Difficulty.MEDIUM)
            spawnedEnemy.GetComponent<EnemyMovement>().setSpeed(6f);
    }


    IEnumerator EnemySpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            spawnEnemy();
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }


    IEnumerator RandomObstacleCoroutine()
    {
        yield return new WaitForSeconds(initialSpawn);
        while (true)
        {
            SpawnRandomObstacle();
            yield return new WaitForSeconds(obstacleSpawnDelay);
        }
    }


    IEnumerator ExplosiveCoroutine()
    {
        yield return new WaitForSeconds(SpawnHelpers.GetInitialExplosiveDelay());
        while(true)
        {
            spawnExplosive();
            yield return new WaitForSeconds(explosiveSpawnDelay);
        }
    }


    IEnumerator ExpandCoroutine()
    {
        yield return new WaitForSeconds(SpawnHelpers.GetInitialExpandDelay());
        while (true)
        {
            spawnExpandWeapon();
            yield return new WaitForSeconds(expandSpawnDelay);
        }
    }


    IEnumerator ShieldCoroutine()
    {
        yield return new WaitForSeconds(SpawnHelpers.GetInitialShieldDelay());
        while (true)
        {
            spawnShield();
            yield return new WaitForSeconds(shieldSpawnDelay);
        }
    }


    void spawnExplosive()
    {
        if (weaponsActiveCount < maxAllowedWeapons)
        {
            weaponsActiveCount++;
            Vector3 pos = SpawnHelpers.GetRandomLocation(walls, player);
            Instantiate(explosive, pos, Quaternion.Euler(new Vector3(90, 0, 0)));
        }
    }


    void spawnExpandWeapon()
    {
        if (weaponsActiveCount < maxAllowedWeapons)
        {
            weaponsActiveCount++;
            Vector3 pos = SpawnHelpers.GetRandomLocation(walls, player);
            Instantiate(expandWeapon, pos, Quaternion.Euler(Vector3.zero));
        }
    }


    void spawnShield()
    {
        if (weaponsActiveCount < maxAllowedWeapons)
        {
            weaponsActiveCount++;
            Vector3 pos = SpawnHelpers.GetRandomLocation(walls, player);
            Instantiate(shieldWeapon, pos, Quaternion.Euler(new Vector3(90, 0, 0)));
        }
    }


    void spawnVerticalWall(string direction, float step = 1f, float speed = 1.5f)
    {
        float topEdge = walls.top.position.z;
        float bottomEdge = walls.bottom.position.z;
        float leftEdge = walls.left.position.x;
        float rightEdge = walls.right.position.x;
        float x = 0f;

        // be careful not to pass in direction other than right and left

        if(direction == "right")
            x = leftEdge - 0.5f;
        else if(direction == "left")
            x = rightEdge + 0.5f;

        for (float i = bottomEdge + 1f; i < topEdge; i += step)
            spawnKinematicEnemy(new Vector3(x, 0f, i), direction, speed);
    }


    void spawnHorizontalWall(string direction, float step = 1f, float speed = 1.5f)
    {
        float topEdge = walls.top.position.z;
        float bottomEdge = walls.bottom.position.z;
        float leftEdge = walls.left.position.x;
        float rightEdge = walls.right.position.x;
        float y = 0f;

        if (direction == "up")
            y = bottomEdge - 0.5f;
        else if (direction == "down")
            y = topEdge + 0.5f;

        for (float i = leftEdge + 1f; i < rightEdge; i += step)
            spawnKinematicEnemy(new Vector3(i, 0f, y), direction, speed);
    }


    void spawnKinematicEnemy(Vector3 pos, string direction, float speed)
    {
        GameObject staticEnemy = Instantiate(enemy, pos, Quaternion.Euler(Vector3.zero));
        staticEnemy.GetComponent<EnemyMovement>().enabled = false;
        staticEnemy.AddComponent<StaticEnemyMover>();
        staticEnemy.GetComponent<StaticEnemyMover>().direction = direction;
        staticEnemy.GetComponent<StaticEnemyMover>().speed = speed;
    }


    IEnumerator spawnCircle(float radius = 8.0f)
    {
        if (player.activeInHierarchy)
        {
            Vector3 pos;

            for (int i = 0; i < 360; i += 45)
            {
                pos.x = player.transform.position.x + radius * Mathf.Sin(i * Mathf.Deg2Rad);
                pos.y = 0f;
                pos.z = player.transform.position.z + radius * Mathf.Cos(i * Mathf.Deg2Rad);

                if(SpawnHelpers.InsideTheWall(pos, walls))
                    Instantiate(enemy, pos, Quaternion.Euler(Vector3.zero));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }


    public void DecreaseSpawnDelay()
    {
        if(enemySpawnDelay > 0.15f)
            enemySpawnDelay -= 0.1f;
        Debug.Log("EnemySpawnDelay: " + enemySpawnDelay);

        if (DifficultyManager.difficulty != (int)Difficulty.HARD)
            DecreaseDelayEasyMed();
        else
            DecreaseDelayHard();
    }


    void DecreaseDelayEasyMed()
    {
        float[] lowerBoundsObstacleDelay = { 12f, 11f };
        float[] upperBoundsWeaponDelay = { 20f, 16f };

        if(obstacleSpawnDelay > lowerBoundsObstacleDelay[DifficultyManager.difficulty])
            obstacleSpawnDelay -= 0.5f;
        if (explosiveSpawnDelay < upperBoundsWeaponDelay[DifficultyManager.difficulty])
            explosiveSpawnDelay += 1.2f;
        if (shieldSpawnDelay < upperBoundsWeaponDelay[DifficultyManager.difficulty])
            shieldSpawnDelay += 0.5f;
        if (expandSpawnDelay < upperBoundsWeaponDelay[DifficultyManager.difficulty])
            expandSpawnDelay += 1.1f;
        Debug.Log("ObstacleSpawnDelay: " + obstacleSpawnDelay);
        PrintWeaponDelays();
    }


    void DecreaseDelayHard()
    {
        if (obstacleSpawnDelay > 9f)
            obstacleSpawnDelay -= 0.5f;
        if (explosiveSpawnDelay > 5f)
            explosiveSpawnDelay -= 0.4f;
        if (shieldSpawnDelay > 6f)
            shieldSpawnDelay -= 0.5f;
        if (expandSpawnDelay > 4f)
            expandSpawnDelay -= 0.3f;
        Debug.Log("ObstacleSpawnDelay: " + obstacleSpawnDelay);
        PrintWeaponDelays();
    }


    public void IncreaseWeaponsAllowed()
    {
        int[] upperBoundsWeaponAllowed = { 5, 6, 7 };
        if (maxAllowedWeapons < upperBoundsWeaponAllowed[DifficultyManager.difficulty])
            maxAllowedWeapons++;
        Debug.Log("MaxWeaponsAllowed: " + maxAllowedWeapons);
    }


    void PrintWeaponDelays()
    {
        Debug.Log("ExplosiveDelay: " + explosiveSpawnDelay);
        Debug.Log("ExpandDelay: " + expandSpawnDelay);
        Debug.Log("ShieldDelay: " + shieldSpawnDelay);
    }
}
