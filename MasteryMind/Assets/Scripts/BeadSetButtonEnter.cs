using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadSetButtonEnter : MonoBehaviour
{
    void OnMouseDown()
    {
        transform.parent.GetComponent<BeadSet>().OnEnterClicked();
    }
}
