using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject Tile;
    public GameObject WinLine;
    public float tileDistance = 1;
    private int mapSize = 3;
    public bool xTurn = true;
    public bool gameEnded = false;
    private string[,] mapArray;
    // Start is called before the first frame update
    void Start()
    {
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
            // trigger win
            gameEnded = true;
        } else{
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
    private void SpawnWinLine(WinLineDirection drection ,int start = 1)
    {
        if (drection == WinLineDirection.Horizonal)
        {
            Vector3 spawnPosition = new Vector3((start / mapSize) + 1, (start % mapSize), -2) * tileDistance;
            GameObject winLine = Instantiate(WinLine, spawnPosition, Quaternion.identity);
        }
        if (drection == WinLineDirection.Vertical)
        {
            Vector3 spawnPosition = new Vector3((start / mapSize), (start % mapSize) + 1, -2) * tileDistance;
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

    public void printMap()
    { 
        string print = "\n[\n";
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            print += " [";
            for (int j = 0; j < mapArray.GetLength(1); j++) {
                print += mapArray[i, j] != "" ? mapArray[i, j] : "-";
                if (j+1 < mapArray.GetLength(1)) print += ", ";
            }
            print += "]\n";
        }
        print += "]";
        Debug.Log(print);
    }
}
