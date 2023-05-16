using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private bool hasLanded;
    [SerializeField]
    private bool thrown;
    private Vector3 initPosition;
    [SerializeField]
    private int diceValue;
    bool IsHeroDice;

    public DiceSide[] diceSides;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        initPosition = transform.position;
        rb.useGravity = false;
    }

    private void Update()
    {
        if (rb.IsSleeping() && !hasLanded && thrown)
        {
            SideValueCheck();
            hasLanded = true;
            rb.useGravity = false;
        }
    }

    public void RollDice()
    {
        if (!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(-100,100)*1000,Random.Range(-100,100)*1000,Random.Range(-100,100)*1000, ForceMode.Impulse);
            rb.AddForce(Vector3.up *500);
        } else if (thrown && hasLanded)
        {
            Reset();
        }
    }

    private void Reset()
    {
        transform.position = initPosition;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
    }

    private void SideValueCheck()
    {
        foreach (DiceSide side in diceSides)
        {
            if (side.getOnGround())
            {
                diceValue = side.sideValue;
            }
        }
        
    }
    public bool GetEnd()
    {
        return (hasLanded && thrown);
    }
    public void Setovner(bool hero)
    {
        this.IsHeroDice = hero;
    }

    public void MoveDice(Vector3 v3)
    {
        this.transform.position = v3 + new Vector3(0.6f,1f,0);
        this.GetComponent<Rigidbody>().useGravity = false;
    }
    public int getEndSide()
    {
        return diceValue;
    }


}
