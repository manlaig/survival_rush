using UnityEngine;

public class ExpandWeaponController : MonoBehaviour
{
    [SerializeField] GameObject expandObject;
    bool spawned;

    void Start()
    {
        spawned = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !spawned)
        {
            spawned = true;
            Instantiate(expandObject, transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(gameObject);
        }
    }
}
