using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enum만 모아놓은 스크립트

public enum UIState
{
    Lobby,
    OptionUI,
    CollectionUI,
    StateUI,
    StoreUI,
    MapUI,
    SynergyUI,
    TowerUI,
    PopUpUI,
}

public enum POPUP_STATE
{
    StageStart,
    GameWin,
    GameLose,
    Option,
    LackGold,
    None,
}

public enum SYNERGY_STATE
{
    UP,
    DOWN,
}
[System.Serializable]
public enum Difficulty : int
{
    Easy,
    Nomal,
    Hard
}

//enum.tostring을 통해 string값으로 변환한 뒤에 줘야함.

public enum ParsingDataSet : int
{
    MonsterData,
    TowerData,
    StoreTowerData,
    StoreData,
    SynergyData,
    MonsterAssociationData,
    FitTimeListData
}
public enum TILE_DATA
{
    TOWER,
    ROAD,
    WALL
}

public enum TOWER_STATE
{
    SearchTarget,
    Attack,
}

[System.Serializable]
public enum SYNERGY : int
{
    Asia = 0,
    Africa = 1,
    NorthAmerica = 2,
    SouthAmerica = 3,
    Oceania = 4,
    Europe = 5,
    Island = 6,
    Peninsula = 7,
    Continent = 8,
}

public enum MonsterAssociationData
{
    Nomal,
    Amount,
    Speed,
    HP,
    Armor,
    Boss1,
    Boss2,
}