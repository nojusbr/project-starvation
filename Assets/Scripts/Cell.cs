using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isWater;

    public Cell(bool isWater)
    {
        this.isWater = isWater;
    }
}
