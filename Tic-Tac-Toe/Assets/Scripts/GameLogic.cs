using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private ScoreManager scoreManager;
    private FrogTextScript frog;
    [SerializeField] private UITileBehaviour UITile;
    [SerializeField] private AudioManager audioManager;
    // Good to know: If you are instantiating a GameObject with a MonoBehaviour on it you can reference it as such here, this will save you from having to write GetComponent<TileBehaviour>() later.
    // example:
    // public TileBehaviour TilePrefab;
    // later ->
    // for (int i = 0; i < mapSize * mapSize; i++) {
    //      TileBehaviour newTile = Instantiate(TilePrefab, position, parent, quaternion);
    //      newTile.index = i;
    //}
    public GameObject Tile;
    // same as with tile prefab
    public GameObject WinLine;
    public GameEndedScript winScreen;
    [SerializeField] private float tileDistance = 1;
    private int mapSize = 3;
    // I don't like this being public, it shouldn't be anybody else's business.
    // Further explanation: It might cause a case one day where someone somewhere in the code changes this xTurn value not from this class. That is messy.
    public bool xTurn = true;
    // Same here as with xTurn, this shouldn't be public
    public bool gameEnded = false;
    //!@! I personally don't like the array being of type string. A better fitting type can be used (byte for example)
    private string[,] mapArray;

    void Start()
    {
        scoreManager = GetComponent<ScoreManager>();
        frog = GetComponent<FrogTextScript>();
        StartGame();
    }

    private void OnEnable()
    {
        // OnEnable can happen multiple times per session, if for some reason the GameObject holding GameLogic will be turned on and off why should we handle the events listeners here?
        // Simply putting them under Start will be better, since Start() only happens once in a GameObject's lifetime.
        EventsManager.OnPlayTurn += PlayTurn;
        EventsManager.OnPlayAgain += StartGame;
    }
    private void OnDisable()
    {
        EventsManager.OnPlayTurn -= PlayTurn;
        EventsManager.OnPlayAgain -= StartGame;
    }
    // Start Game
    [ContextMenu("Restart Game")]
    private void StartGame()
    {
        ClearGame();
        if (!xTurn) ChangeTurn();
        gameEnded = false;
        mapArray = GenerateEmptyMapArray();
        SpawnTiles();
    }
    private void ClearGame()
    {
        // why are we destroying these objects and creating them again? this is an unnecessary operation (and a pretty heavy one at that...)
        // Some of the heaviest operations in Unity are dealing with garbage collection around GameObjects. This can be easily circumvented by adding a function to the tiles: ClearTile() and iterating over them clearing them all.
        // Same with the winLine.
        // Which by the way another heavy operation: GameObject.Find. Why not hold a reference to these objects?
        // Generally: each time you use GameObject.Find ask yourself "Why am I not holding these objects as references?"
        // The only case I found that needs GameObject.Find is for editor scripts that cannot hold references to scene objects as they are not MonoBehaviours.
        GameObject Tiles = GameObject.Find("Tiles");
        GameObject WinLine = GameObject.Find("WinLine(Clone)");
        if (Tiles != null)
        {
            Destroy(Tiles);
        }
        if (WinLine != null)
        {
            Destroy(WinLine);
        }

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
        // As I mentioned before this should only be called once at the init of the game.
        GameObject tiles = new GameObject("Tiles");
        tiles.transform.parent = this.transform;
        for (int i = 0; i < mapSize * mapSize; i++)
        {
            // see comment above about instantiating and then GetComponenting
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
    public bool AvailableTile(bool clicked)
    {
        // Can also be implemented with the tile's index.
        // int row = index % mapSize;
        // int column = index / mapSize;
        // return mapArray[row, column] != "" && !gameEnded;
        // I don't know which is better, storing another boolean value or doing these calculations.
        // I think the commented out solution is better, it doesn't have to take into account what the TileBehaviour sends saying whether it is clicked or not.
        // It isn't a heavy calculation in any case, no need to worry.
        return !clicked && !gameEnded;
    }
    private void PlayTurn(int index)
    {
        // I've seen this code in a few places, maybe you should make it into a function? private int[] GetTileRowAndColumnByIndex(int index)
        // Or even split it into three functions GetTileRowByIndex(int index) GetTileColumnByIndex(int index)?
        int row = index % mapSize;
        int column = index / mapSize;
        // because then here it would be: mapArray[GetTileRowByIndex(index), GetTileColumnByIndex(index)] = GetTurn();
        mapArray[row, column] = GetTurn();
        audioManager.PlayXOSound(xTurn);
        if (TestWin())
        {
            audioManager.PlayWinSound();
            scoreManager.IncreaseScore(xTurn);
            frog.Win();
            //I think the frog should be under the WinScreen.cs, makes more sense to delegate that power to a single view class
            winScreen.ShowWinScreen(GetTurn() + " Wins!");
            gameEnded = true;
        }
        else if (TestTie())
        {
            audioManager.PlayTieSound();
            scoreManager.IncreaseTieScore();
            frog.Tie();
            winScreen.ShowWinScreen("It's a Tie!");
            gameEnded = true;
        }
        else
        {
            ChangeTurn();
        }
    }

    private void ChangeTurn()
    {
        // This makes the class into a view type class.. maybe it should be handled somewhere else?
        UITile.RotateUITile(xTurn);
        xTurn = !xTurn;
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
        int diagonal = 0;
        int otherDiagonal = 0;
        int ms = mapSize - 1;
        for (int i = 0; i < mapSize; i++)
        {
            if (mapArray[0, 0] == mapArray[i, i] && mapArray[i, i] != "") diagonal++;
            if (mapArray[0, ms] == mapArray[i, ms - i] && mapArray[i, ms - i] != "") otherDiagonal++;
        }
        if (diagonal == mapSize)
        {
            SpawnWinLine(WinLineDirection.Diagonal);
            return true;
        }
        if (otherDiagonal == mapSize)
        {
            SpawnWinLine(WinLineDirection.OtherDiagonal);
            return true;
        }

        return false;
    }
    private bool TestTie()
    {
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                if (mapArray[i, j] == "") return false;
            }
        }
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
