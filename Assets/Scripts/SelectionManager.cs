using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SelectionManager : MonoBehaviour
{
    [Header("Trees")]
    [SerializeField] private string treeTag = "Tree";
    [SerializeField] private float chopDamage = 1f;
    [SerializeField] private Transform playerModel;
    public bool IsChopping = false;
    public GameObject axe;

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
        AcquireResources();
    }

    public void AcquireResources()
    {
        if (playerModel == null)
        {
            //Debug.LogError("Player model is not assigned to SelectionManager!");
            return;
        }

        Ray ray = new Ray(playerModel.position, playerModel.forward);
        //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform selection = hit.transform;
            if (selection.CompareTag(treeTag))
            {
                if (hit.distance < 2f)
                {
                    if (Input.GetKey(KeyCode.E) && toolManager.isAxeEquipped)
                    {
                        selection.GetComponent<ResourceManager>().ObtainResource(chopDamage);
                        StartCoroutine(ChopRoutine(5f));

                    }
                }
            }
        }
    }

    private IEnumerator ChopRoutine(float duration)
    {
        IsChopping = true;
        yield return new WaitForSeconds(duration);
        print("Chopping");
        IsChopping = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Resource"))
        {
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(pickUpSound);
        }
    }



}
