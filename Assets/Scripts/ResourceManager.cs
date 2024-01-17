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

    public void ObtainResource(float damage)
    {
        resourceHealth -= damage;
        if (resourceHealth <= 0f)
        {
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
}
