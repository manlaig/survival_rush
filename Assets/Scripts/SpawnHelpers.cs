using UnityEngine;

public static class SpawnHelpers
{
    public static int GetRangeForInstantSpawn()
    {
        int[] ranges = { Random.Range(7, 13), Random.Range(10, 15), Random.Range(12, 20) };
        return ranges[DifficultyManager.difficulty];
    }


    public static float GetInitialExplosiveDelay()
    {
        float[] delays = { 0f, 5f, 10f };
        return delays[DifficultyManager.difficulty];
    }


    public static float GetInitialExpandDelay()
    {
        float[] delays = { 0f, 0f, 4f };
        return delays[DifficultyManager.difficulty];
    }


    public static float GetInitialShieldDelay()
    {
        float[] delays = { 0f, 10f, 15f };
        return delays[DifficultyManager.difficulty];
    }


    public static bool InsideTheWall(Vector3 pos, WallColliderPosition walls)
    {
        bool inside = true;
        if (pos.x > walls.right.position.x - 1f || pos.x < walls.left.position.x + 1f)
            inside = false;
        if (pos.z > walls.top.position.z - 1f || pos.z < walls.bottom.position.z + 1f)
            inside = false;
        return inside;
    }


    // this function will return a vector that is not too close to the player and not outside the wall
    public static Vector3 GetRandomLocation(WallColliderPosition walls, GameObject player)
    {
        Vector3 pos = RandomVector(walls);
        while (VectorTooClose(pos, player))
            pos = RandomVector(walls);
        return pos;
    }


    public static Vector3 RandomVector(WallColliderPosition walls)
    {
        return new Vector3(Random.Range(walls.left.position.x + 2f, walls.right.position.x - 2f),
                           0f, Random.Range(walls.top.position.z - 2f, walls.bottom.position.z + 2f));
    }


    // check whether a position is too close to the player
    public static bool VectorTooClose(Vector3 pos, GameObject player)
    {
        return (Vector3.Distance(pos, player.transform.position) > 5f) ? false : true;
    }
}
