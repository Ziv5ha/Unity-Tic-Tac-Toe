using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject Tile;
    public GameObject WinLine;
    public GameEndedScript winScreen;
    public float tileDistance = 1;
    private int mapSize = 3;
    public bool xTurn = true;
    public bool gameEnded = false;
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
        mapArray = GenerateEmptyMapArray();
        SpawnTiles();
    }
// Start Game
    private string[,] GenerateEmptyMapArray() {
        string[,] map = new string[mapSize,mapSize];
        for (int r = 0; r < mapSize; r++)
        {
            for (int c = 0; c < mapSize; c++)
            {
                map[r,c] = "";
            }
        }
        return map;
    }
    private void SpawnTiles(){
        for (int i = 0; i < mapSize*mapSize; i++)
        {
            GameObject tile = Instantiate(Tile, GetTileSpawnPosition(i), Quaternion.identity);
            tile.GetComponent<TileBehaviour>().index =  i;
        }
    }
    private Vector3 GetTileSpawnPosition(int index){
        int row = index % mapSize;
        int column = index / mapSize;
        return new Vector3(column, row, 0) * tileDistance;
        
    }

// Play
    public void PlayTurn(int index){
        int row = index % mapSize;
        int column = index / mapSize;
        mapArray[row, column] = GetTurn();
        if (TestWin()){
            winScreen.ShowWinScreen(GetTurn() + " Wins!");
            gameEnded = true;
        } else if(testTie()){
            winScreen.ShowWinScreen("It's a Tie!");
            gameEnded = true;
        } else {
            FindAnyObjectByType<UITileBehaviour>().RotateUITile(xTurn);
            xTurn = !xTurn;
        }
    }

    public string GetTurn() {
        return xTurn? "X" : "O";
    }

// End Game
    enum WinLineDirection
    {
        Vertical, Horizonal, Diagonal, OtherDiagonal
    }
    private bool TestWin(){
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
        
        if (row == mapSize){
            SpawnWinLine(WinLineDirection.Horizonal, start);
            return true;
        }
        if (column == mapSize){
            SpawnWinLine(WinLineDirection.Vertical, start);
            return true;
        }

        return false;
    }


    private bool TestDiagonalWin(){
        if (mapArray[0, 0] == mapArray[1, 1] && mapArray[0, 0] == mapArray[2, 2] && mapArray[0, 0] != "" ){
            SpawnWinLine(WinLineDirection.Diagonal);
            return true;
        }
        if (mapArray[0, 2] == mapArray[1, 1] && mapArray[0, 2] == mapArray[2, 0] && mapArray[0, 2] != "" ){
            SpawnWinLine(WinLineDirection.OtherDiagonal);
            return true;
        }
        
        return  false;
    }
    private bool testTie() {
        int occupiedTiles = 0;
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++) {
                if (mapArray[i, j] != "") occupiedTiles++;
            }
        }
        return occupiedTiles == mapSize * mapSize;
    }
    private void SpawnWinLine(WinLineDirection drection ,int start = 1)
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
    public void printMap()
    { 
        string print = "\n";
        for (int i = mapArray.GetLength(0)-1; i >= 0; i--)
        {
            print += " [";
            for (int j = 0; j < mapArray.GetLength(1); j++) {
                print += mapArray[i, j] != "" ? mapArray[i, j] : "-";
                if (j+1 < mapArray.GetLength(1)) print += ", ";
            }
            print += "]";
            if (i > 0) print += "\n";
        }
        Debug.Log(print);
    }
}
