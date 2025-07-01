using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public Mole[] moles; // Assign your Mole instances in Inspector
    public float delayBetweenMoles = 2f;

    public static GameManager Instance;


    void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        StartCoroutine(MolePopUpCycle());
    }

    IEnumerator MolePopUpCycle()
    {
        while (true)
        {
            int index = Random.Range(0, moles.Length);
            yield return moles[index].PopUpAndWait(); // Let only one mole pop up at a time
            yield return new WaitForSeconds(delayBetweenMoles);
        }
    }

    public int AddScore(int original, int amount)
    {
        // Handle score
        original += amount;
        Debug.Log("Add score: " + amount);
        return original;
    }
}
