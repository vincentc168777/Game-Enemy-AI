using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Required so the 2 variables show up in editor
[System.Serializable]
public class RegionType
{
    [SerializeField] private LayerMask regionLayer;
    [SerializeField] private int movePenalty;
    
    public void setRegionLayer(LayerMask l)
    {
        regionLayer = l;
    }

    public void setMovePenalty(int num)
    {
        movePenalty = num;  
    }

    public int getMovePenalty()
    {
        return movePenalty;
    }

    public LayerMask getRegionLayer()
    {
        return regionLayer;
    }
   
}
