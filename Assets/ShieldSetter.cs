using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShieldSetter : MonoBehaviour
{
    public void SetShield(string shield)
    {
        this.gameObject.GetComponent<TextMeshPro>().text = shield;
    }
}
