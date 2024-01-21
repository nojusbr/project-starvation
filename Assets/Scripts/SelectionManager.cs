using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SelectionManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private string treeTag = "Tree";
    [SerializeField] private string rockTag = "Rock";
    [SerializeField] private float acquireDmg = 1f;
    [SerializeField] private Transform playerModel;
    public bool IsAcquiring = false;
    public GameObject axe;
    public GameObject pickaxe;

    public ToolManager toolManager;

    public AudioSource audioSource;
    public AudioClip pickUpSound;

    private void Start()
    {
        axe.SetActive(false);
        toolManager = GetComponent<ToolManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ChopTree();
        MineRock();
    }

    public void ChopTree()
    {
        if (playerModel == null)
            return;

        Ray ray = new Ray(playerModel.position, playerModel.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform selection = hit.transform;
            if (selection.CompareTag(treeTag))
            {
                if (hit.distance < 2f)
                {
                    if (Input.GetKey(KeyCode.E) && toolManager.isAxeEquipped)
                    {
                        selection.GetComponent<ResourceManager>().ObtainResource(acquireDmg);
                        StartCoroutine(AcquireRoutine(5f));

                    }
                }
            }
        }
    }

    public void MineRock()
    {
        if (playerModel == null)
            return;

        Ray ray = new Ray(playerModel.position, playerModel.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform selection = hit.transform;
            if (selection.CompareTag(rockTag))
            {
                if (hit.distance < 2f)
                {
                    if (Input.GetKey(KeyCode.E) && toolManager.isPickaxeEquipped)
                    {
                        selection.GetComponent<ResourceManager>().ObtainResource(acquireDmg);
                        StartCoroutine(AcquireRoutine(5f));

                    }
                }
            }
        }
    }

    private IEnumerator AcquireRoutine(float duration)
    {
        IsAcquiring = true;
        yield return new WaitForSeconds(duration);
        IsAcquiring = false;
    }
}
