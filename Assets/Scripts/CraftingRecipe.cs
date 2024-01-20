using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XEntity.InventoryItemSystem
{
    [System.Serializable]
    public class CraftingRecipe
    {
        public Item inputItem1;
        public Item inputItem2;
        public Item outputItem;
    }
}
