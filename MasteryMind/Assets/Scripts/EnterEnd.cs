using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterEnd : MonoBehaviour
{
	void Start ()
    {
        GetComponent<DataLogger>().Log( DataLogger.EVENT_SESSION_END, 0, 0, 0, "", 0.0f );
    }
	
}
