using UnityEngine;
using System.Collections;

public class Mallet : MonoBehaviour
{
    public Transform restingPlace;
    public GameObject malletPrefab; // Reference to the launcher prefab itself
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            malletPrefab.GetComponent<Collider>().enabled = false;


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
            malletPrefab.transform.SetPositionAndRotation(restingPlace.position, restingPlace.rotation);

            Rigidbody rb = malletPrefab.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }


            // Re-enable after short delay
            StartCoroutine(ReenableAfterDelay(0.5f));
        }
    }

    IEnumerator ReenableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        malletPrefab.GetComponent<Collider>().enabled = true;
    }
}
