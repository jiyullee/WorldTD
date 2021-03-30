using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enum만 모아놓은 스크립트

public enum UIState
{
    Lobby,
    OptionUI,
    CollectionUI,
    GameUI,
    StateUI,
    StoreUI,
    GameOptionUI,
}

[System.Serializable]
public enum Difficulty
{
    Easy,
    Nomal,
    Hard
}
//enum.tostring을 통해 string값으로 변환한 뒤에 줘야함.
public enum ParsingDataSet
{
    MonsterData,
    TowerData,
    ShopData
}
public enum TILE_DATA
{
    TOWER,
    ROAD,
    WALL
}
