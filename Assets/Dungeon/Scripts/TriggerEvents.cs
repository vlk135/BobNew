using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Up Door" || other.gameObject.name == "Down Door" ||
            other.gameObject.name == "Left Door" ||
            other.gameObject.name == "Right Door" )
        {
            Debug.Log("Exit");
           
        }
    }
}
