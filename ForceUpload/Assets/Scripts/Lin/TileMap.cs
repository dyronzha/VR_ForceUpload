using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0 → 一般normal
//1 → 柱子pillar
//2 → 箱子box
//3 → 輸送帶conveyor

public class TileMap : MonoBehaviour{

    public TileType[] _tiletypes;
    int[,] tiles;
    int mapSizeX = 7;
    int mapSizeZ = 7;

    void Start(){
        GenerateMapData();
        GenerateMapVisual();
    }

    void Update(){
        
    }

    void GenerateMapData() {
        //確認地圖規模
        tiles = new int[mapSizeX, mapSizeZ];
        int x, z;

        //初始化地圖
        for (x = 0; x < mapSizeX; x++) {
            for (z = 0; z < mapSizeZ; z++) {
                tiles[x, z] = 0;
            }
        }

        //柱子位置
        tiles[0, 4] = 1;
        tiles[3, 0] = 1;
        tiles[3, 4] = 1;
        tiles[4, 5] = 1;
        tiles[5, 5] = 1;
        tiles[6, 2] = 1;
        tiles[6, 6] = 1;

        //木箱位置
        tiles[1, 4] = 2;
        tiles[3, 2] = 2;
        tiles[5, 4] = 2;
        tiles[6, 3] = 2;

        //輸送帶位置
        tiles[4, 0] = 3;
        tiles[5, 1] = 3;
        tiles[5, 3] = 3;
    }

    void GenerateMapVisual() {
        for (int x = 0; x < mapSizeX; x++) {
            for (int z = 0; z < mapSizeZ; z++){
                TileType tt = _tiletypes[tiles[x, z]];
                GameObject ChessboardRoad = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, 0.0f, z), Quaternion.identity);

                //if (tiles[x, z] == 0){
                //    ChessboardRoad.transform.rotation = Quaternion.Euler(90, 0, 0);
                //}

                ChessboardRoad.transform.SetParent(transform);
            }
        }
    }

}
