using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public SelectionManager selectionManager;
    public bool isAxeEquipped = false;
    public ParticleSystem poofEffect;

    private void Start()
    {
        selectionManager = GetComponent<SelectionManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!isAxeEquipped)
            {
                EquipAxe();
            }
            else
            {
                UnequipAxe();
            }
        }
    }

    public void EquipAxe()
    {
        selectionManager.axe.SetActive(true);
        isAxeEquipped = true;
        poofEffect.Play();
    }

    public void UnequipAxe()
    {
        poofEffect.Play();
        selectionManager.axe.SetActive(false);
        isAxeEquipped = false;
    }
}
