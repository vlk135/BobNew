using Assets.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Character
{
    private string CharName;
    private int MaxHP;
    private int CurrentHP;
    private int Shield;
    private bool IsHero;
    private DiceToken[] DiceTokens;
    private Color BaseColor;

    private GameObject Card;
    private GameObject Dice;

    public Character(int maxHP, string name, bool isHero, Color baseColor, DiceToken[] diceTokens)
    {
        this.MaxHP = maxHP;
        this.CharName = name;
        this.CurrentHP = maxHP;
        this.Shield = 0;
        this.IsHero = isHero;
        this.BaseColor = baseColor;
        this.DiceTokens = diceTokens;
    }
    public void SetDice(GameObject dice)
    {
        this.Dice = dice;
        Dice.GetComponent<MeshRenderer>().materials[0].color = BaseColor;

        int i = 0;
        foreach (var side in Dice.GetComponent<Dice>().diceSides)
        {
            Material resource;
            string idk = DiceTokens[i].GetAction();
            switch (idk)
            {
                case ("shield"):
                    resource = Resources.Load<Material>("Materials/shield");
                    break;
                case ("hit"):
                    resource = Resources.Load<Material>("Materials/sword");
                    break;
                case ("heal"):
                    resource = Resources.Load<Material>("Materials/heart");
                    break;
                default:
                    resource = Resources.Load<Material>("Materials/nothing");
                    break;
            }
            var renderer = side.GetComponentInChildren<MeshRenderer>();
            renderer.material = resource;

            var text = side.GetComponentInChildren<TextMeshPro>();
            text.SetText(DiceTokens[i].getPips());

            i++;

        }
    }
    public void SetCard(GameObject card)
    {
        this.Card = card;
        Card.GetComponentInChildren<MeshRenderer>().materials[0].color = BaseColor;
        Material resource;
        switch (CharName)
        {
            case ("Blacksmith"):
                resource = Resources.Load<Material>("Materials/blacksmith");
                break;
            case ("Herbalist"):
                resource = Resources.Load<Material>("Materials/herbalist");
                break;
            case ("Drunkard"):
                resource = Resources.Load<Material>("Materials/drunkard");
                break;
            case ("Hunter"):
                resource = Resources.Load<Material>("Materials/hunter");
                break;
            case ("Spider"):
                resource = Resources.Load<Material>("Materials/spider");
                break;
            case ("Wolf"):
                resource = Resources.Load<Material>("Materials/wolf");
                break;
            case ("Slime"):
                resource = Resources.Load<Material>("Materials/slime");
                break;
            case ("Bat"):
                resource = Resources.Load<Material>("Materials/bat");
                break;
            default:
                resource = Resources.Load<Material>("Materials/nothing");
                break;

        }

        var renderer = card.GetComponentInChildren<MeshRenderer>();
        renderer = renderer.transform.GetChild(0).GetComponent<MeshRenderer>();
        renderer.material = resource;
        updateShield(); 
        updateHP();
    }

    public void ThrowDice()
    {
        Dice.GetComponent<Dice>().RollDice();
    }

    public bool getHeroStatus()
    {
        return IsHero;
    }
    public Color GetDiceColor()
    {
        return this.BaseColor;
    }
    public Dice GetDice()
    {
        return Dice.GetComponent<Dice>();
    }
    public Vector3 getCardV3()
    {
        return Card.transform.position;
    }
    public GameObject GetDiceGameObject()
    {
        return Dice;
    }


    public bool GetHit(int dmg)
    {
        if (dmg < Shield)
        {
            this.Shield -= dmg;
        }
        else
        {
            this.CurrentHP -= (dmg - Shield);
            this.Shield = 0;
        }
        updateShield(); 
        updateHP();
        return true;
    }
    public bool Heal(int HealHP)
    {
        this.CurrentHP += HealHP;
        if (this.CurrentHP > MaxHP)
        {
            this.CurrentHP = MaxHP;
        }
        updateHP();
        return true;
    }
    public bool GetShield(int ShieldHP)
    {
        this.Shield += ShieldHP;
        updateShield();
        return true;
    }
    public void ChangeAllSides(int SideIndex)
    {
        foreach (var side in Dice.GetComponent<Dice>().diceSides)
        {
            Material resource;
            string idk = DiceTokens[SideIndex].GetAction();
            switch (idk)
            {
                case ("shield"):
                    resource = Resources.Load<Material>("Materials/shield");
                    break;
                case ("sword"):
                    resource = Resources.Load<Material>("Materials/sword");
                    break;
                case ("heal"):
                    resource = Resources.Load<Material>("Materials/heart");
                    break;
                default:
                    resource = Resources.Load<Material>("Materials/nothing");
                    break;
            }
            var renderer = side.GetComponentInChildren<MeshRenderer>();
            renderer.material = resource;

            var text = side.GetComponentInChildren<TextMeshPro>();
            text.SetText(DiceTokens[SideIndex].getPips());
        }
    }
    public void ChangeAllSidesBack()
    {
        int i = 0;
        foreach (var side in Dice.GetComponent<Dice>().diceSides)
        {
            Material resource;
            string idk = DiceTokens[i].GetAction();
            switch (idk)
            {
                case ("shield"):
                    resource = Resources.Load<Material>("Materials/shield");
                    break;
                case ("sword"):
                    resource = Resources.Load<Material>("Materials/sword");
                    break;
                case ("heal"):
                    resource = Resources.Load<Material>("Materials/heart");
                    break;
                default:
                    resource = Resources.Load<Material>("Materials/nothing");
                    break;
            }
            var renderer = side.GetComponentInChildren<MeshRenderer>();
            renderer.material = resource;
            var text = side.GetComponentInChildren<TextMeshPro>();
            text.SetText(DiceTokens[i].getPips());
            i++;
        }
    }
    public void updateHP()
    {
        Card.GetComponentInChildren<HpSetter>().SetHp(CurrentHP.ToString());
    }
    public void updateShield()
    {
        Card.GetComponentInChildren<ShieldSetter>().SetShield(Shield.ToString());
    }
    public DiceToken GetDiceToken(int pos)
    {
        return DiceTokens[pos];
    }
    public int getHealth()
    {
        return CurrentHP;
    }
}
