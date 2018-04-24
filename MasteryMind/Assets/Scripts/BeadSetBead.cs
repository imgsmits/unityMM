using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadSetBead : MonoBehaviour
{
    public int index = 0;

    public int color = -1;

    void OnMouseDown()
    {
        transform.parent.GetComponent<BeadSet>().OnBeadClicked( index );
    }
}
