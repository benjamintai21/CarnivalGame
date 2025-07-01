using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class LaunchProjectile : MonoBehaviour
{
    public Transform launcherTip = null;
    public GameObject projectilePrefab = null;
    public float launchForce = 5.0f;
    public float reloadTime = 1.0f;

    public Transform restingPlace;
    public GameObject launcherPrefab; // Reference to the launcher prefab itself

    private bool canShoot = true;

    public void Launch()
    {
        if (!canShoot)
        {
            Debug.Log("Reloading!");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, launcherTip.position, launcherTip.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(launcherTip.forward * launchForce);

        canShoot = false;
        Invoke(nameof(ResetShot), reloadTime);
    }

    void ResetShot()
    {
        canShoot = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            launcherPrefab.GetComponent<Collider>().enabled = false;


            // Move object to starting position
            if (restingPlace != null)
            {
                transform.SetParent(null);
                transform.position = restingPlace.position;
            }
            else
            {
                Debug.LogWarning("Resting place not assigned!");
            }

            Debug.Log("Resetting to: " + restingPlace.position);
            // Reset weapon to resting place
            launcherPrefab.transform.SetPositionAndRotation(restingPlace.position, restingPlace.rotation);

            // Re-enable after short delay
            StartCoroutine(ReenableAfterDelay(0.5f));
        }
    }

    IEnumerator ReenableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        launcherPrefab.GetComponent<Collider>().enabled = true;
    }
}
