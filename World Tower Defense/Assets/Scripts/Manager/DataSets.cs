using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Data(T)를 지정해주고 세팅해줘야함
/// singleton으로 구현되어있고 
/// OnInitiate에서 SetDictionary를 반드시 사용해야함.
/// </summary>
/// <typeparam name="T"> 하위클래스에서 구현한 추상 클래스</typeparam>

public abstract class DataSets<T> : UnitySingleton<DataSets<T>> where T : DataSets<T>.DataClass
{
    #region variable
    /// <summary>
    /// 딕셔너리가 들어있는 테이블 List
    /// </summary>
    protected static List<Dictionary<string, object>> tableList;
    /// <summary>
    /// 파일 이름을 추가할 경우 ParsingDataSet에 Enum을 추가한 뒤에 추가한다
    /// string로 줄 경우 실패할 가능성이 크기 때문에 미리정해진 값중 하나를 사용하도록 만들었다.
    ///  = ParsingDataSet.MonsterData등으로 사용할것
    /// </summary>
    protected static ParsingDataSet fileName;
    /// <summary>
    /// 자식이 구현해야 하는 추상 데이터 클래스
    /// </summary>
    public abstract class DataClass
    {
        public Sprite sprite { get; set; }
    }
    #endregion

    #region abstractF
    /// <summary>
    /// 딕셔너리 세팅해주는 것.
    /// ParsingDataSet.MonsterData
    /// </summary>
    protected void SetDictionary(ParsingDataSet fileName)
    {
        tableList = DataManager.Read(fileName);
    }

    #endregion

    #region Function

    public Dictionary<string, object> GetTableData(int id)
    {
        return tableList[id];
    }
    #endregion
}
