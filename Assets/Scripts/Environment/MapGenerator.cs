using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
 
/**
 * generating the map randomly
 */

public class MapGenerator : MonoBehaviour
{

    public class Cell {

        public bool visited = false;
        public bool[] status = new bool[4];
    
    }

    [Header ("Map Setting")]
    public int startPos = 0;
    public Vector2 size;
    public Vector2 offset;

    [Header ("Number of Object")]
    public int numOfEnemy = 3;
    public int numOfMachine = 5;
    public int numOfBox = 5;

    [Header ("Spawn Object List")]
    public GameObject[] room; // 0: exit, n: other rooms
    public GameObject[] mySpawnObject; // 0: enemy, 1: machine, 2: first aid box
    public GameObject[] ghost; // 0: blue, 1: green, 2: yellow

    private MyData mData;

    // total number of object
    private int enemyNum;
    private int machineNum;
    private int boxNum;
    private int ghostIndex;

    // environment
    private NavMeshSurface[] surfaces;
    private List<Cell> board;
    private bool isExistPlayer = false;
    private int totalRoom = 0;
    private int genNum = 0;
    private List<CollisionChecking> escapeAreas;

    void Awake()
    {

        mData = SavingSystem.LoadData();
        if (mData != null)
            ghostIndex = mData.GetCharacterIndex();
        else
            mData = new MyData();

        // generate map until player exists
        while (true)
        {
            if (isExistPlayer)
                break;
            ClearMap();
            GenerateMaze();
            genNum++;
        }

        // generate the nav mesh 
        surfaces = transform.GetComponentsInChildren<NavMeshSurface>();
        for (int i = 0; i < surfaces.Length; i++)
            surfaces[i].BuildNavMesh();

    }

    // calculate the probability of spawning the specific item
    bool SpawnProbaility(int num, int total) {

        int p = (int)(((float)num / total) * 100);

        return p == 1f ? true : p == 0f ? false: Random.Range(0, 100) <= p ? true : false;
    
    }

    /**
     * generate room and its objects within the game board
     */

    void GenerateMap() {

        for (int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {

                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
                if (currentCell.visited)
                {

                    Room newRoom = null;

                    if (i == 0 && j == 0)
                    {

                        // first exits
                        newRoom = Instantiate(room[0], new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<Room>();
                        newRoom.UpdateRoom(currentCell.status, true);
                        escapeAreas.Add(newRoom.GetEscapeArea());
                    }
                    else if (i == size.x - 1 && j == size.y - 1)
                    {
                        // last exits
                        newRoom = Instantiate(room[0], new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<Room>();
                        newRoom.UpdateRoom(currentCell.status, false);
                        escapeAreas.Add(newRoom.GetEscapeArea());
                    }
                    else
                    {

                        // generate a random room
                        int randomRoom = Random.Range(1, room.Length);
                        newRoom = Instantiate(room[randomRoom], new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<Room>();
                        newRoom.UpdateRoom(currentCell.status);

                        if (enemyNum > 0 && SpawnProbaility(enemyNum, totalRoom))
                        {
                            // generate enemy
                            newRoom.SpawnCharacter(mySpawnObject[0]);
                            enemyNum--;
                        }
                        
                        if (!isExistPlayer && SpawnProbaility(1, totalRoom))
                        {
                            // generate player
                            newRoom.SpawnCharacter(ghost[ghostIndex]);
                            isExistPlayer = true;
                        }

                        if (machineNum > 0 && SpawnProbaility(machineNum, totalRoom))
                        {
                            // generate machine
                            newRoom.SpawnMachine(mySpawnObject[1]);
                            machineNum--;
                        }

                        if (boxNum > 0 && SpawnProbaility(boxNum, totalRoom))
                        {
                            // generate first aid box
                            newRoom.SpawnBox(mySpawnObject[2]);
                            boxNum--;
                        }

                    }

                    // rename the room and calculate total room
                    newRoom.name += " " + i + "-" + j + "-" + genNum;
                    totalRoom--;

                }


            }
        }

    }

    /**
     * generate a game board randomly
     */

    void GenerateMaze() {

        board = new List<Cell>();

        // generate a board base on the size
        for (int i = 0; i < size.x; i++)
            for(int j = 0; j < size.y; j++)
                board.Add(new Cell());

        // init the position
        int currentCell = startPos;
        Stack<int> path = new Stack<int>();

        while (true)
        {
            board[currentCell].visited = true;

            // check whether visited all cell
            if(currentCell == board.Count - 1)
                break;

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                    break; // no available cell within the path
                else
                    currentCell = path.Pop(); // back to previous cell
            }
            else
            {

                path.Push(currentCell); // record the cell into the path
                totalRoom++;
                int newCell = neighbors[Random.Range(0, neighbors.Count)];
                if (newCell > currentCell)
                {
                    
                    if (newCell - 1 == currentCell)
                    {
                        // goto right
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        // goto down
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        // goto left
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        // goto up
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }

                }
            }
        }

        GenerateMap();
    
    }

    /**
     * get the available neighbors
     */

    List<int> CheckNeighbors(int cell) {

        List<int> neighbors = new List<int>();

        // room on the top
        if(cell - size.x >= 0 && !board[Mathf.FloorToInt(cell-size.x)].visited)
            neighbors.Add(Mathf.FloorToInt(cell - size.x));

        // room on the down
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
            neighbors.Add(Mathf.FloorToInt(cell + size.x));

        // room on the right
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
            neighbors.Add(Mathf.FloorToInt(cell + 1));

        // room on the left
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
            neighbors.Add(Mathf.FloorToInt(cell - 1));

        return neighbors;
    
    }

    /**
     * reset the map generator
     */

    void ClearMap()
    {

        startPos = 0;

        enemyNum = numOfEnemy;
        machineNum = numOfMachine;
        boxNum = numOfBox;

        surfaces = null;

        board = null;
        totalRoom = 0;
        isExistPlayer = false;

        escapeAreas = new List<CollisionChecking>();

        foreach (Transform child in transform)
            Destroy(child.gameObject);


    }

    public int GetTotalMachineNum() {

        return numOfMachine;
    
    }

    public List<CollisionChecking> GetEscapeArea() {

        return escapeAreas;
    
    }

}
