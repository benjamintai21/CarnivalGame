using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using NUnit.Framework.Constraints;
using TMPro;

public class Mole : MonoBehaviour
{
    private const string V = "Press button to start game";
    public static Mole Instance;
    public float popUpHeight = 3f;
    public float popUpTime = 1f;
    public float stunDuration = 3f;
    public TextMeshPro scoreText;

    //public Color normalColor;
    public GameObject molePrefab;
    public Transform[] moleHoles;
    public Material stunnedMaterial;
    public Material originalMaterial;
    public AudioClip hitSound;

    public GameObject weaponObject;         // Assign your weapon (e.g. mallet or gun)
    public Transform weaponRestingPlace;

    private int score = 0;

    private Vector3 originalPosition;
    private Renderer rend;
    private AudioSource audioSource;
    private bool isPoppedUp = false;
    private bool isStunned = false;

    private bool gameActive = false;

    void Awake()
    {
        Instance = this;
        scoreText.text = V;
    }
    void Start()
    {
        originalPosition = transform.localPosition;
        audioSource = GetComponent<AudioSource>();

        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnPoked);
        }
    }

    public void StartMoleGame()
    {
        if (gameActive)
        {
            EndMoleGame();
            return;
        }
        score = 0;
        gameActive = true;

        Debug.Log("Game started!");
        scoreText.text = "Score: 0";

        GameManager.Instance.StartGame();
    }

    void EndMoleGame()
    {
        score = 0;
        gameActive = false;


        Debug.Log("Game ended: Your scores is: " + score);

        // Drop weapon onto table
        if (weaponObject != null && weaponRestingPlace != null)
        {
            weaponObject.transform.SetPositionAndRotation(
                weaponRestingPlace.position, weaponRestingPlace.rotation
            );
        }
        scoreText.text = "Game Ended! Press button to start new game";
    }
    //IEnumerator SpawnMoles()
    //{
    //    while (gameRunning)
    //    {
    //        yield return new WaitForSeconds(Random.Range(1f, 3f));
    //        if (activeMoles.Count < maxActiveMoles)
    //        {
    //            List<GameObject> inactiveMoles = allMoles.FindAll(m => !activeMoles.Contains(m) && !m.activeSelf);
    //        }
    //    }
    //}

    void OnPoked(SelectEnterEventArgs args)
    {

        // Access the interactor that poked the mole
        GameObject interactorGO = args.interactorObject.transform.gameObject;
        Debug.Log("Hit mole!");
        // Check the tag
        if (interactorGO.CompareTag("Mallet")) // <-- change to your tag
        {

            if (!isStunned && isPoppedUp)
            {
                isStunned = true;

                Transform[] children = GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.CompareTag("Jeff"))
                    {
                        Debug.Log("found jeff");
                        Renderer sphereRenderer = child.GetComponent<Renderer>();
                        Debug.Log(sphereRenderer);
                        if (sphereRenderer != null)
                        {
                            Debug.Log("changing to stun color");
                            sphereRenderer.material = stunnedMaterial;
                            score = GameManager.Instance.AddScore(score, 1);

                            scoreText.text = "Score: " + score;
                            
                            break; // Stop after finding the first one
                        }
                    }
                }

                if (audioSource && hitSound)
                    audioSource.PlayOneShot(hitSound);

                StartCoroutine(StunMole());
            }
        }
    }
    public IEnumerator PopUpAndWait()
    {
        if (isStunned) yield break;

        transform.localPosition = originalPosition + Vector3.up * popUpHeight;
        isPoppedUp = true;

        yield return new WaitForSeconds(popUpTime);

        if (!isStunned)
        {
            transform.localPosition = originalPosition;
            isPoppedUp = false;
        }
    }

    IEnumerator StunMole()
    {
        isStunned = true;

        Debug.Log("Stunned!");

        yield return new WaitForSeconds(stunDuration);

        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.CompareTag("Jeff"))
            {
                Renderer sphereRenderer = child.GetComponent<Renderer>();
                if (sphereRenderer != null)
                {
                    Debug.Log("changing to original color");
                    sphereRenderer.material = originalMaterial;
                    break;
                }
            }
        }

        transform.localPosition = originalPosition;
        isPoppedUp = false;
        isStunned = false;
    }
}




