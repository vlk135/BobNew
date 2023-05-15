using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HpSetter : MonoBehaviour
{
    public void SetHp(string hp)
    {
        this.gameObject.GetComponent<TextMeshPro>().text = hp;
    }
}
