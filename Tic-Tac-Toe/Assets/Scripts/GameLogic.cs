using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private int mapSize = 3;
    private bool XTurn = true;
    private bool gameEnded = false;
    private string[,] Map;
    // Start is called before the first frame update
    void Start()
    {
        Map = GenerateEmptyMapArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
    public void UpdateMap(int row, int column){
        Map[row, column] = GetTurn();
        if (TestWin()){
            // trigger win
        }
    }

    public string GetTurn() {
        return XTurn? "X" : "O";
    }

    private bool TestWin(){
        bool shouldWin = false;
        for (int i = 0; i < mapSize; i++)
        {
            if (TestSingleRowAndColumnWin(i)) shouldWin = true;
        }
        if (TestOtherDiagonWin()) shouldWin = true;
        return shouldWin;
    }
    private bool TestSingleRowAndColumnWin(int start){
        int row = 0;
        int column = 0;
        int diagonal = 0;
        for (int i = 0; i < mapSize; i++)
        {
            if (Map[start, 0]==Map[start, i]) row++;
            if (Map[0, start]==Map[i, start]) column++;
            // using the loop to also check the diagonal line from top left to bottom right
            if (start == 0){
                if (Map[0, 0]==Map[i, i]) diagonal++;
            }
        }
        return row == mapSize || row == mapSize || diagonal == mapSize;
    }
    private bool TestOtherDiagonWin(){
        int diagonal = 0;
        for (int row = mapSize - 1; row >= 0 ; row--)
        {
            for (int column = 0; column < mapSize; column++)
            {
                if (Map[0, mapSize]==Map[row, column]) diagonal++;

            }
        }
        return  diagonal == mapSize;
    }
}
