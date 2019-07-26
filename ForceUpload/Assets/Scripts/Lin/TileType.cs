using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType{

    public string name; //棋盤名稱，可省略
    public GameObject tileVisualPrefab; // 棋盤造型，可省略
    public bool isWalkable = true;  //棋盤是否可走
    public int TileType_Num;    //棋盤種類編號
}
