using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    static AdManager adManager;

	void Start ()
    {
        if (adManager == null)
            adManager = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        if(!Advertisement.isInitialized)
        {
            Debug.Log("Initializing");
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                Advertisement.Initialize("2679683", true);
            else if (Application.platform == RuntimePlatform.Android)
                Advertisement.Initialize("2679684", true);
        }
	}
	
	public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
            Debug.Log("Showing ad");
        }
        else
        {
            Debug.Log("Failed to show ad");
        }
    }
}
