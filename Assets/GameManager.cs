using Assets.Characters;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

namespace Assets
{
    class GameManager : MonoBehaviour
    {
        public GameObject Dice;
        public GameObject PlayerCard;
        private int turn;
        private List<Character> PlayerHeroes;
        private List<Character> EnemyMonsters;
        private List<Character> CharacterList;
        private List<GameObject> Dices;
        public Vector3 DiceSpawinPoint;
        bool zacatekM = false;
        bool zacatekP = false;

        private void Update()
        {
            switch (this.turn)
            {
                case 1:
                    int i = 0;
                    MonsterPrepare();
                    int endChar = 0;
                    if(zacatekM == false)
                    {
                        zacatekM = true;
                        foreach (Character character in EnemyMonsters)
                        {
                            character.GetDice().RollDice();
                        }
                    }
                    foreach (Character character in EnemyMonsters)
                    {
                        if(!character.GetDice().GetEnd())
                        {
                            if(character.GetDiceGameObject().GetComponent<Transform>().transform.position.y <= -100 )
                            {
                                character.GetDiceGameObject().GetComponent<Transform>().transform.SetPositionAndRotation(DiceSpawinPoint + new Vector3(i*2,0,0),transform.rotation);
                            }
                            character.ThrowDice();
                        }
                        else
                        {
                            character.GetDice().MoveDice(character.getCardV3());
                            endChar++;
                        }
                    }
                    if (endChar == EnemyMonsters.Count())
                    {
                        this.turn = 2;
                        zacatekM = false;
                    }
                    break;
                case 2:
                    HeroPrepare();
                    int endChar1 = 0;
                    int i2 = 0;

                    if (zacatekP == false)
                    {
                        zacatekP = true;
                        foreach (Character character in PlayerHeroes)
                        {
                            character.GetDice().RollDice();
                        }
                    }

                    foreach (Character character in PlayerHeroes)
                    {

                        if (!character.GetDice().GetEnd())
                        {
                            if (character.GetDiceGameObject().GetComponent<Transform>().transform.position.y <= -100)
                            {
                                character.GetDiceGameObject().GetComponent<Transform>().transform.SetPositionAndRotation(DiceSpawinPoint + new Vector3(i2 * 2, 0, 0), transform.rotation);
                            }
                            character.ThrowDice();
                        }
                        else
                        {
                            character.GetDiceGameObject().GetComponent<Dice>().MoveDice(character.getCardV3());
                            endChar1++;
                        }
                        if (endChar1 == PlayerHeroes.Count())
                        {
                            zacatekP = false;
                            this.turn++;
                        }
                    }
                    break; 
                case 3:
                    foreach(Character character in PlayerHeroes)
                    {
                        character.GetDiceGameObject().tag = "drag";
                    }
                    this.turn = 4;
                    break;
                case 4:
                    int PlayerDiceUnused = PlayerHeroes.Where(c => c.GetDiceGameObject().transform.GetComponent<Transform>().position.y == -50).ToList().Count();
                    if(PlayerDiceUnused == PlayerHeroes.Count())
                    {
                        this.turn = 5;
                    }
                    else
                    {
                        foreach (Character character in PlayerHeroes)
                        {
                            
                            int selectedHero = (int)Random.Range(0, PlayerHeroes.Count());
                            int selectedEnemy = (int)Random.Range(0, EnemyMonsters.Count());
                            DiceToken heroMove = character.GetDiceToken(character.GetDiceGameObject().GetComponent<Dice>().getEndSide());

                            if (PlayerHeroes.Where(x => x.getHealth() > 0).Count() == 0 || EnemyMonsters.Where(x => x.getHealth() > 0).Count() == 0)
                            {
                                EndGame(EnemyMonsters.Where(x => x.getHealth() > 0).Count() == 0);
                            }
                            GeneralMove(heroMove, PlayerHeroes, EnemyMonsters, selectedHero, selectedEnemy);

                            character.GetDiceGameObject().transform.GetComponent<Transform>().SetPositionAndRotation(new Vector3(0, 500, 0), new Quaternion(0, 0, 0, 0));
                        }
                        this.turn = 5;
                    }
                    break;
                case 5:
                    int EnemyDiceUnused = EnemyMonsters.Where(c => c.GetDiceGameObject().transform.GetComponent<Transform>().position.y == -50).ToList().Count();
                    if (EnemyDiceUnused == EnemyMonsters.Count())
                    {
                        this.turn = 1;
                    }
                    else
                    {
                        foreach (Character character in EnemyMonsters)
                        {
                            List<Character> HeroAlive = PlayerHeroes.Where(c=> c.getHealth() > 0).ToList();
                            List<Character> EnemyAlive = EnemyMonsters.Where(c => c.getHealth() > 0).ToList();
                            if (EnemyAlive.Count() == 0 || HeroAlive.Count == 0)
                            {
                                EndGame(EnemyAlive.Count == 0);
                            }
                            int selectedHero = (int)Random.Range(0, HeroAlive.Count());
                            int selectedEnemy = (int)Random.Range(0, EnemyAlive.Count());
                            DiceToken monsterMove = character.GetDiceToken(character.GetDiceGameObject().GetComponent<Dice>().getEndSide());

                            GeneralMove(monsterMove, EnemyAlive, HeroAlive, selectedEnemy, selectedHero);

                            character.GetDiceGameObject().transform.GetComponent<Transform>().SetPositionAndRotation(new Vector3(0, 500, 0), new Quaternion(0, 0, 0, 0));
                        }
                        this.turn = 1;
                    }
                    break;

                default:

                    break;
            }
        }

        public void EndGame(bool win)
        {
            this.turn = 0;
            if(win)
            {
                //SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
                //SceneManager.UnloadScene(SceneManager.GetSceneAt(1));
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

        public int getTurn()
        {
            return turn;
        }

        private void Start()
        {
            //SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));

            this.DiceSpawinPoint = this.transform.position;
            PlayerHeroes = new List<Character>();
            EnemyMonsters = new List<Character>();
            CharacterList = new List<Character>();
            GenerateHeroesList();

            EnemyMonsters.Add(CharacterList[5]);
            EnemyMonsters.Add(CharacterList[6]);
            EnemyMonsters.Add(CharacterList[7]);
            EnemyMonsters.Add(CharacterList[8]);

            PlayerHeroes.Add(CharacterList[0]);
            PlayerHeroes.Add(CharacterList[1]);
            PlayerHeroes.Add(CharacterList[2]);
            PlayerHeroes.Add(CharacterList[3]);


            PrepareArena();
            this.turn = 1;
        }

        private void GenerateHeroesList()
        {
            // I'm sorry for this ...

            this.CharacterList.Add(new Character(12, "Blacksmith", true, new UnityEngine.Color(0.5f, 0.5f, 0.5f),
                GetDiceTokens(2, 2, 1, 1, 1, 0, "shield", "shield", "hit", "hit", "shield", "nothing")));

            this.CharacterList.Add(new Character(15, "Drunkard", true, new UnityEngine.Color(0.9f, 1, 0.258f),
                GetDiceTokens(3, 2, 1, 1, 0, 0, "taunt", "hit", "hit", "hit", "nothing", "nothing")));

            this.CharacterList.Add(new Character(9, "Herbalist", true, new UnityEngine.Color(0.51f, 0.8f, 0.79f),
                GetDiceTokens(3, 2, 0, 0, 2, 1, "heal", "hit", "nothing", "nothing", "heal", "heal")));

            this.CharacterList.Add(new Character(8, "Hunter", true, new UnityEngine.Color(0.1f, 0.52f, 0, 1f),
                GetDiceTokens(3, 1, 2, 2, 0, 0, "hit", "heal", "hit", "hit", "nothing", "nothing")));

            this.CharacterList.Add(new Character(10, "Innkeeper", true, new UnityEngine.Color(0.79f, 0.16f, 1f),
                GetDiceTokens(2, 1, 1, 1, 1, 1, "heal", "shield", "hit", "hit", "shield", "shield")));

            //Monsteur
            this.CharacterList.Add(new Character(5, "Bat", true, new UnityEngine.Color(0.9f, 0.1f, 0.1f),
                GetDiceTokens(2, 2, 1, 1, 1, 0, "hit", "hit", "hit", "hit", "hit", "nothing")));

            this.CharacterList.Add(new Character(2, "Slime", true, new UnityEngine.Color(0.9f, 0.1f, 0.3f),
                GetDiceTokens(3, 2, 1, 1, 0, 0, "hit", "hit", "hit", "hit", "nothing", "nothing")));

            this.CharacterList.Add(new Character(10, "Wolf", true, new UnityEngine.Color(0.9f, 0.3f, 0.1f),
                GetDiceTokens(3, 2, 0, 0, 2, 1, "hit", "hit", "nothing", "nothing", "hit", "hit")));

            this.CharacterList.Add(new Character(4, "Spider", true, new UnityEngine.Color(0.9f, 0.1f, 0, 2f),
                GetDiceTokens(3, 1, 2, 2, 0, 0, "hit", "hit", "hit", "hit", "nothing", "nothing")));
        }

        private DiceToken[] GetDiceTokens(int firstPip, int secondPip, int ThirdPip, int FourthPip, int FifthPip, int SixthPip,
    string firstAction, string secondAction, string ThirdAction, string FourthAction, string FifthAction, string SixthAction)
        {
            DiceToken[] diceTokens = new DiceToken[6];

            diceTokens[0] = new DiceToken(SideEnum.Front, firstPip, firstAction);
            diceTokens[1] = new DiceToken(SideEnum.Top, secondPip, secondAction);
            diceTokens[2] = new DiceToken(SideEnum.Right, ThirdPip, ThirdAction);
            diceTokens[3] = new DiceToken(SideEnum.Left, FourthPip, FourthAction);
            diceTokens[4] = new DiceToken(SideEnum.Back, FifthPip, FifthAction);
            diceTokens[5] = new DiceToken(SideEnum.Bottom, SixthPip, SixthAction);
            return diceTokens;
        }
        public void MonsterPrepare()
        {
            int i = 0;
            foreach (var character in EnemyMonsters)
            {
                if(character.GetDiceGameObject() == null)
                {
                    var dice = Instantiate(Dice, DiceSpawinPoint + new Vector3(2 * i, 0, 0), transform.rotation);
                    dice.GetComponent<Dice>().Setovner(false);
                    character.SetDice(dice);
                }
                else if(Dice.GetComponent<Transform>().transform.localPosition.y <= -100)
                {
                    character.GetDiceGameObject().transform.GetComponent<Transform>().SetPositionAndRotation(DiceSpawinPoint + new Vector3(2 * i, 0, 0), transform.rotation);
                }
                character.GetDiceGameObject().GetComponent<Rigidbody>().useGravity = true;
                i++;
            }
        }
        public void HeroPrepare()
        {
            int i = 0;
            foreach (var character in PlayerHeroes)
            {
                if (character.GetDiceGameObject() == null)
                {
                    var dice = Instantiate(Dice, DiceSpawinPoint + new Vector3(2 * i, 0, 0), transform.rotation);
                    dice.GetComponent<Dice>().Setovner(false);
                    character.SetDice(dice);
                }
                else if (Dice.GetComponent<Transform>().transform.localPosition.y == -100)
                {
                    character.GetDiceGameObject().transform.GetComponent<Transform>().SetPositionAndRotation(DiceSpawinPoint + new Vector3(0, 0, 2 * i), transform.rotation);
                }
                character.GetDiceGameObject().GetComponent<Rigidbody>().useGravity = true;
                i++;
            }
        }
        public void PrepareArena()
        {
            int i = 0;
            foreach(Character character in PlayerHeroes)
            {
                var GenCharCard = Instantiate(PlayerCard,new Vector3(-7.5f + (i*4.2f),0, -3.9f),transform.rotation);
                GenCharCard.tag = "Hero";
                character.SetCard(GenCharCard);
                i++;
            }
            i = 0;
            foreach(Character character in EnemyMonsters)
            {
                var GenCharCard = Instantiate(PlayerCard, new Vector3(7.6f - (i * 4.2f), 0f, 3.3f), transform.rotation);
                GenCharCard.tag = "Enemy";
                character.SetCard(GenCharCard);
                i++;
            }
        }
        public void GeneralMove(DiceToken tok, List<Character> Ally, List<Character> Enemy, int posAlly, int posEnemy)
        {
            Debug.Log($"{Ally.Count()} - {Enemy.Count()}");
            switch(tok.GetAction())
            {
                case "hit":
                    Enemy[posEnemy].GetHit(tok.getPipsint());
                    break;
                case "shield":
                    Ally[posAlly].GetShield(tok.getPipsint());
                    break;
                case "heal":
                    Ally[posAlly].Heal(tok.getPipsint());
                    break;
            }
        }
    }
}
