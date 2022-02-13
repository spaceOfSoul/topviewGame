using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargingEfect : MonoBehaviour
{
    void Update()
    {
        float scale = 3;
        scale += Time.deltaTime;
        if (scale <= 5)
            transform.localScale = new Vector3(scale, scale, 0);
        else
            scale = 5;
    }
}
