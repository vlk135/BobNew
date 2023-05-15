using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y)
        {
            // 0 - cannot spawn 1 - can spawn 2 - HAS to spawn

            if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1; // if it is obligatory returns 2
            }

            return 0;
        }

    }

    public Vector2 size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;

    List<Cell> board;
    void Start()
    {
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateDungeon()
    {
        for(int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
                if (currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for (int k = 0; k < rooms.Length; k++)
                    {
                        int p = rooms[k].ProbabilityOfSpawning(i, j);

                        if (p == 2)
                        {
                            randomRoom = k;
                            break;
                        }
                        else if (p == 1)
                        {
                            availableRooms.Add(k);
                        }
                    }

                    if (randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }

                    var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j*offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);

                    newRoom.name += " " + i + "-" + j;
                }
                
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for(int i = 0; i <size.x; i++)
        {
            for(int j = 0; j <size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 1000)
        {
            k++;
            board[currentCell].visited = true;

            if(currentCell == board.Count - 1)
            {
                break;
            }

            // check the neighbours

            List<int> neighbours = CheckNeighbours(currentCell);

            if(neighbours.Count == 0)
            {
                if(path.Count == 0) // last cell on the path
                {
                    break;
                } else
                {
                    currentCell = path.Pop(); // removes path
                }
            }
            else
            {
                path.Push(currentCell); // added cell to path

                int newCell = neighbours[Random.Range(0, neighbours.Count)]; 

                if( newCell > currentCell) // down or right
                {                   
                    if (newCell - 1 == currentCell) // new cell is on the right
                    {
                        board[currentCell].status[2] = true; 
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else //up or left
                {
                    if (newCell + 1 == currentCell) // new cell is on the left of current cell
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true; // chyba v tutor typky? 4?
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }

                }
            }
        }
        GenerateDungeon();  
    }

    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours  = new List<int>();

        //check up neighbour

        if(cell - size.x >= 0 && !board[Mathf.FloorToInt(cell-size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x));
        }
        // down
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }
        // right
        if ((cell+1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1 ));
        }
        //left
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell -1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell -1 ));
        }

        return neighbours;
    }
}
