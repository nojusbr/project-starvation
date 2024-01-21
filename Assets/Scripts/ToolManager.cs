using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public SelectionManager selectionManager;
    public bool isAxeEquipped = false;
    public bool isPickaxeEquipped = false;
    
    public ParticleSystem poofEffect;

    private void Start()
    {
        selectionManager = GetComponent<SelectionManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!isPickaxeEquipped)
            {
                EquipPickaxe();
                UnequipAxe();
                poofEffect.Play();
            }
            else
            {
                UnequipPickaxe();
                poofEffect.Play();
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!isAxeEquipped)
            {
                EquipAxe();
                UnequipPickaxe();
                poofEffect.Play();
            }
            else
            {
                UnequipAxe();
                poofEffect.Play();
            }
        }
    }

    public void EquipPickaxe()
    {
        selectionManager.pickaxe.SetActive(true);
        isPickaxeEquipped = true;
    }

    public void UnequipPickaxe()
    {
        selectionManager.pickaxe.SetActive(false);
        isPickaxeEquipped = false;   
    }


    public void EquipAxe()
    {
        selectionManager.axe.SetActive(true);
        isAxeEquipped = true;
    }

    public void UnequipAxe()
    {
        selectionManager.axe.SetActive(false);
        isAxeEquipped = false;
    }
}
