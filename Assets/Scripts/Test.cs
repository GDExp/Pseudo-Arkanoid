using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform obj_1;
    public Transform obj_2;



    private void Start()
    {
        obj_1.localPosition = new Vector2(-6f, 1.25f);
        obj_2.localPosition = new Vector2(obj_2.localPosition.x, 1.15f);
    }
}
