using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private ScoreManager scoreManager;
    private FrogTextScript frog;
    [SerializeField] private UITileBehaviour UITile;
    public GameObject Tile;
    public GameObject WinLine;
    public GameEndedScript winScreen;
    [SerializeField] private float tileDistance = 1;
    private int mapSize = 3;
    public bool xTurn = true;
    public bool gameEnded = false;
    //!@! I personally don't like the array being of type string. A better fitting type can be used (byte for example)
    private string[,] mapArray;

    // private UITileBehaviour uiTile;
    void Start()
    {
        // I left this Comment in to remember to ask you the best practice for this.
        // If I have a GameObject that I use once sould I keep it as private and get access to it like so?
        // Should I just Give it public access and Import it from Unity?
        // Or maybe use the single line of code I used in line 67? 
        // Is it even "acceptable" use a GameObject as a UI element?
        // uiTile = GameObject.FindGameObjectWithTag("UITile").GetComponent<UITileBehaviour>();

        //!@! I didn't completely understand the question. If you want to have a reference to an object
        // you should set it in Unity using either a public reference or [SerializeField], depending on your needs.
        // If you must find an object you can use the FindGameObjectWithTag() once like you did here.
        // Generally either way is fine as long as you don't make too many calls to any type of FindGameObject (FindAnyObjectOfType, GameObject.Find etc.)
        // which can be pretty memory intensive while also being inconsistent due to having to be active in the scene hierarchy in order to be found.
        // Another thing to take into consideration is the order of operations. Say you have a gameController type object that calls a lot of functions on
        // views for example. We cannot be sure in which order the Start() functions will take place. So this might be a problem. For example:
        /*
         gameController:
        [SerializeField] private ViewType view;
        private void Start() {
            view.PrintSetting();
        }

        view:
        private Settings settings;
        private void Start() {
            settings = FindGameObjectWithTag("Settings").GetComponent<Settings>();
        }

        public void PrintSetting() {
            Debug.Log(settings.MAX_NUMBER_OF_PLAYERS);
        }
         */
        // In this example we might call the gameController Start() function first and this will cause a null reference error on the view.PrintSetting() function
        // since it hasn't been initialized properly. To circumvent this we can either set the settings as a [SerializeField] or add an Init() function called by
        // the gameController before any calls to view.PrintSetting(). Personally I like having only one class that has an Awake() or Start() method that calls
        // Init() methods on all other classes. Hierarchy is a beautiful thing in code.
        scoreManager = GetComponent<ScoreManager>();
        frog = GetComponent<FrogTextScript>();
        StartGame();
    }
    // Start Game
    [ContextMenu("Restart Game")]
    private void StartGame()
    {
        ClearGame();
        mapArray = GenerateEmptyMapArray();
        SpawnTiles();
    }
    private void ClearGame()
    {
        GameObject Tiles = GameObject.Find("Tiles");
        if (Tiles != null)
        {
            Debug.Log("clear game");
            Destroy(Tiles);
        }
        else
        {
            Debug.Log("no game found");
        }
        Debug.Log("generating new game");
    }
    private string[,] GenerateEmptyMapArray()
    {
        string[,] map = new string[mapSize, mapSize];
        for (int r = 0; r < mapSize; r++)
        {
            for (int c = 0; c < mapSize; c++)
            {
                map[r, c] = "";
            }
        }
        return map;
    }
    private void SpawnTiles()
    {
        GameObject tiles = new GameObject("Tiles");
        tiles.transform.parent = this.transform;
        for (int i = 0; i < mapSize * mapSize; i++)
        {
            GameObject tile = Instantiate(Tile, GetTileSpawnPosition(i), Quaternion.identity, tiles.transform);
            tile.GetComponent<TileBehaviour>().index = i;
        }
    }
    private Vector3 GetTileSpawnPosition(int index)
    {
        int row = index % mapSize;
        int column = index / mapSize;
        return new Vector3(column, row, 0) * tileDistance;

    }

    // Play
    //!@! I don't like that this function is being called by the TileBehaviour script, it confuses the hierarchy between the objects.
    // You should look into events/actions, it's a cleaner way to setup View->Controller commands.
    public void PlayTurn(int index)
    {
        int row = index % mapSize;
        int column = index / mapSize;
        mapArray[row, column] = GetTurn();
        if (TestWin())
        {
            scoreManager.IncreaseScore(xTurn);
            frog.Win();
            winScreen.ShowWinScreen(GetTurn() + " Wins!");
            gameEnded = true;
        }
        else if (TestTie())
        {
            scoreManager.IncreaseTieScore();
            frog.Tie();
            winScreen.ShowWinScreen("It's a Tie!");
            gameEnded = true;
        }
        else
        {
            UITile.RotateUITile(xTurn);
            xTurn = !xTurn;
        }
    }

    public string GetTurn()
    {
        return xTurn ? "X" : "O";
    }

    // End Game
    enum WinLineDirection
    {
        Vertical, Horizonal, Diagonal, OtherDiagonal
    }
    private bool TestWin()
    {
        bool shouldWin = false;
        for (int i = 0; i < mapSize; i++)
        {
            if (TestSingleRowAndColumnWin(i)) shouldWin = true;
        }
        if (TestDiagonalWin()) shouldWin = true;
        return shouldWin;
    }
    private bool TestSingleRowAndColumnWin(int start)
    {
        int row = 0;
        int column = 0;
        for (int i = 0; i < mapSize; i++)
        {
            if (mapArray[start, 0] == mapArray[start, i] && mapArray[start, i] != "") row++;
            if (mapArray[0, start] == mapArray[i, start] && mapArray[i, start] != "") column++;
        }

        if (row == mapSize)
        {
            SpawnWinLine(WinLineDirection.Horizonal, start);
            return true;
        }
        if (column == mapSize)
        {
            SpawnWinLine(WinLineDirection.Vertical, start);
            return true;
        }

        return false;
    }


    private bool TestDiagonalWin()
    {
        // !@! simple and nice. Please note though that it works differently from the TestSingleRowAndColumnWin that can work with an arbitrary mapSize.
        // might consider changing to different approach so as to keep that functionality
        if (mapArray[0, 0] == mapArray[1, 1] && mapArray[0, 0] == mapArray[2, 2] && mapArray[0, 0] != "")
        {
            SpawnWinLine(WinLineDirection.Diagonal);
            return true;
        }
        if (mapArray[0, 2] == mapArray[1, 1] && mapArray[0, 2] == mapArray[2, 0] && mapArray[0, 2] != "")
        {
            SpawnWinLine(WinLineDirection.OtherDiagonal);
            return true;
        }

        return false;
    }
    private bool TestTie()
    {
        // int occupiedTiles = 0;
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                if (mapArray[i, j] == "") return false;
                // if (mapArray[i, j] != "") occupiedTiles++;
            }
        }
        // return occupiedTiles == mapSize * mapSize;
        return true;
    }
    private void SpawnWinLine(WinLineDirection drection, int start = 1)
    {
        if (drection == WinLineDirection.Horizonal)
        {
            Vector3 spawnPosition = new Vector3((start / mapSize) + 1, (start % mapSize), -2) * tileDistance;
            GameObject winLine = Instantiate(WinLine, spawnPosition, Quaternion.identity);
        }
        if (drection == WinLineDirection.Vertical)
        {
            Vector3 spawnPosition = new Vector3((start % mapSize), (start / mapSize) + 1, -2) * tileDistance;
            GameObject winLine = Instantiate(WinLine, spawnPosition, Quaternion.identity);
            winLine.GetComponent<WinLineScript>().isVertical();
        }
        if (drection == WinLineDirection.Diagonal)
        {
            Vector3 spawnPosition = new Vector3(1, 1, -2) * tileDistance;
            GameObject winLine = Instantiate(WinLine, spawnPosition, Quaternion.identity);
            winLine.GetComponent<WinLineScript>().isDiagonal();
        }
        if (drection == WinLineDirection.OtherDiagonal)
        {
            Vector3 spawnPosition = new Vector3(1, 1, -2) * tileDistance;
            GameObject winLine = Instantiate(WinLine, spawnPosition, Quaternion.identity);
            winLine.GetComponent<WinLineScript>().isOtherDiagonal();
        }
    }

    // For Debugging.
    public void PrintMap()
    {
        string print = "\n";
        for (int i = mapArray.GetLength(0) - 1; i >= 0; i--)
        {
            print += " [";
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                print += mapArray[i, j] != "" ? mapArray[i, j] : "-";
                if (j + 1 < mapArray.GetLength(1)) print += ", ";
            }
            print += "]";
            if (i > 0) print += "\n";
        }
        Debug.Log(print);
    }
}
