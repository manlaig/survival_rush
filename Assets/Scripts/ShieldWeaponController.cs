using UnityEngine;

public class ShieldWeaponController : MonoBehaviour
{
    [SerializeField] GameObject shield;
    public static bool shieldActive = false;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
    }

    void OnDestroy()
    {
        shieldActive = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            shieldActive = true;
            GameManager.score += 10;
            SpawnManager.weaponsActiveCount--;

            GameObject shieldGO = Instantiate(shield, other.gameObject.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
            shieldGO.transform.parent = other.gameObject.transform;

            Destroy(shieldGO, 5f);
            Destroy(gameObject, 5f);
            DisableComponentsToDestroy();
        }
    }

    void DisableComponentsToDestroy()
    {
        // disabling the renderer object
        GetComponent<ParticleSystem>().Stop();
        GetComponentInChildren<Transform>().gameObject.SetActive(false);

        foreach (Collider col in GetComponents<SphereCollider>())
            col.enabled = false;
    }
}
