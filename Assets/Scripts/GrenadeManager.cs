using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    public int grenadeCount; // Keeps track of collected grenades

    void Start()
    {
        // Optional: initialize or show grenade count
    }

    void Update()
    {
        // You can handle grenade throwing here, e.g.:
        // if (Input.GetKeyDown(KeyCode.G)) ThrowGrenade();
    }

    public void ThrowGrenade()
    {
        if (grenadeCount > 0)
        {
            // Instantiate grenade prefab here
            // e.g. Instantiate(grenadePrefab, throwPoint.position, Quaternion.identity);
            grenadeCount--;
        }
    }
}
