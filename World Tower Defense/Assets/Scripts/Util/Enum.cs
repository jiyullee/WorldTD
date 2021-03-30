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
    easy,
    nomal,
    hard
}
