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
    public GameObject Tile;
    public GameObject WinLine;
    public GameEndedScript winScreen;
    [SerializeField] private float tileDistance = 1;
    private int mapSize = 3;
    public bool xTurn = true;
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
        gameEnded = false;
        mapArray = GenerateEmptyMapArray();
        SpawnTiles();
    }
    private void ClearGame()
    {
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
    public bool AvailableTile(bool clicked)
    {
        // Can also be implemented with the tile's index.
        // int row = index % mapSize;
        // int column = index / mapSize;
        // return mapArray[row, column] != "" && !gameEnded;
        // I don't know which is better, storing another boolean value or doing these calculations.
        return !clicked && !gameEnded;
    }
    private void PlayTurn(int index)
    {
        int row = index % mapSize;
        int column = index / mapSize;
        mapArray[row, column] = GetTurn();
        audioManager.PlayXOSound(xTurn);
        if (TestWin())
        {
            audioManager.PlayWinSound();
            scoreManager.IncreaseScore(xTurn);
            frog.Win();
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
