using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMinimapIcon : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
