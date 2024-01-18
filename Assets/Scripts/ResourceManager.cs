using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public float resourceHealth = 10f;
    public GameObject[] resourcePrefabs;
    public int maxRandom;
    public int minRandom;
    public AudioSource audioSource;
    public AudioClip[] sounds;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }

    public void ObtainResource(float damage)
    {
        resourceHealth -= damage;
        PlaySound(1);

        if (resourceHealth <= 0f)
        {
            PlaySound(0);
            Destroy(gameObject);
            InstantiateRandomItems(transform.position);
        }
    }


    private void InstantiateRandomItems(Vector3 position)
    {
        foreach (GameObject itemPrefab in resourcePrefabs)
        {
            int randomQuantity = Random.Range(minRandom, maxRandom);
            InstantiateItems(itemPrefab, position, randomQuantity);
        }
    }

    private void InstantiateItems(GameObject itemPrefab, Vector3 position, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            Instantiate(itemPrefab, position, Quaternion.identity);
        }
    }

    void PlaySound(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < sounds.Length)
        {
            audioSource.clip = sounds[clipIndex];
            audioSource.Play();
        }
    }
}
