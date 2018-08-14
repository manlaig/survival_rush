using UnityEngine;

public class ExpandWeaponController : MonoBehaviour
{
    [SerializeField] GameObject expandObject;
    bool spawned;

    void Start()
    {
        spawned = false;
        GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-6f, 6f), 0f, Random.Range(-6f, 6f));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !spawned)
        {
            spawned = true;
            GameManager.score += 10;
            SpawnManager.weaponsActiveCount--;
            Instantiate(expandObject, transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(gameObject);
        }
    }
}
