using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bricks_", menuName = "Brick/Info", order = 0)]
public class BricksType : ScriptableObject
{
    public GameObject brick_prefab;
    public Color[] color_type;
    public int[] hit_type;

    
}
