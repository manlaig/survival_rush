using UnityEngine;

public class ExpandWeaponController : MonoBehaviour
{
    [SerializeField] GameObject expandObject;
    bool spawned;

    void Start()
    {
        spawned = false;
        GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(0f, 5f), 0f, Random.Range(0f, 5f));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !spawned)
        {
            spawned = true;
            SpawnManager.weaponsActiveCount--;
            Instantiate(expandObject, transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(gameObject);
        }
    }
}
