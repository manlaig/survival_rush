using UnityEngine;

public class ExplosiveController : MonoBehaviour
{
    public GameObject bg;

    bool exploding = false;
    ParticleSystem explosion;

	void Start ()
    {
        explosion = GetComponent<ParticleSystem>();
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, (Random.Range(0, 2) == 0) ? 1f : -1f, 0f);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !exploding && !explosion.isPlaying)
        {
            exploding = true;
            GameManager.score += 10;
            SpawnManager.weaponsActiveCount--;
            GameObject delBg = Instantiate(bg, transform.position, transform.rotation);
            Destroy(delBg, 5f);
            Destroy(gameObject, 5f);
            explosion.Play();

            DisableAllRenderers();
        }
    }

    void DisableAllRenderers()
    {
        GetComponent<MeshRenderer>().enabled = false;
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in renderers)
            ren.enabled = false;
    }
}
