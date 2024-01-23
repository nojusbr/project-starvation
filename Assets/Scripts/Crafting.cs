using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XEntity.InventoryItemSystem;

public class Crafting : MonoBehaviour
{
    public ItemSlot[] inventorySlots;

    public int woodCount = 0;
    public int rockCount = 0;
    public int grassCount = 0;

    private void Update()
    {
        CraftWall();
    }

    public void CraftWall()
    {
        if (woodCount >= 5 && rockCount >= 5)
        {
            // Deduct 5 wood and 5 rocks from the inventory
            RemoveResources(5, "Wood");
            RemoveResources(5, "Rock");

            // Add your wall crafting logic here
            // For example:
            Debug.Log("Crafting Wall!");
        }
    }

    private void RemoveResources(int count, string resourceName)
    {
        foreach (ItemSlot slot in inventorySlots)
        {
            if (slot.slotItem != null && slot.slotItem.itemName == resourceName)
            {
                // Deduct the specified count from the resource in the slot
                int deduction = Mathf.Min(count, slot.itemCount);
                slot.itemCount -= deduction;

                // Update the corresponding resource count
                UpdateResourceCount(resourceName, -deduction);

                // Reduce the remaining count
                count -= deduction;

                // If count becomes zero, exit the loop
                if (count == 0)
                    break;
            }
        }
    }

    private void UpdateResourceCount(string resourceName, int delta)
    {
        switch (resourceName)
        {
            case "Wood":
                woodCount += delta;
                break;
            case "Rock":
                rockCount += delta;
                break;
            case "Grass":
                grassCount += delta;
                break;
                // Add more cases if you have additional resources
        }
    }


    public void CountResources()
    {
        foreach (ItemSlot slot in inventorySlots)
        {
            if (slot.slotItem != null)
            {
                if (slot.slotItem.itemName == "Wood")
                {
                    woodCount += slot.itemCount;
                }
                else if (slot.slotItem.itemName == "Rock")
                {
                    rockCount += slot.itemCount;
                }
                else if (slot.slotItem.itemName == "Grass")
                {
                    grassCount += slot.itemCount;
                }
            }
        }

        // Check if the player has 3 woods and 3 rocks.
        if (woodCount >= 3 && rockCount >= 3)
        {
            Debug.Log("Player has 3 woods and 3 rocks!");
        }
    }
}
