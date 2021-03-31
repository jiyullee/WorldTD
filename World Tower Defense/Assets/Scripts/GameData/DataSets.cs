using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;


/// <summary>
/// Data(T)를 지정해주고 세팅해줘야함
/// singleton으로 구현되어있고 
/// OnInitiate에서 SetDictionary를 반드시 사용해야함.
/// </summary>
/// <typeparam name="T"> 하위클래스에서 구현한 추상 클래스</typeparam>

namespace GameData
{
    public abstract class DataSets<K, T> : Singleton<K> where T : DataSets<K, T>.DataClass, new()
        where K : DataSets<K, T>, new()
    {
        #region variable

        //메타 문자열
        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        static char[] TRIM_CHARS = {'\"'};

        /// <summary>
        /// 딕셔너리가 들어 있는 테이블
        /// </summary>
        protected static Dictionary<int, T> table;

        /// <summary>
        /// 파일 이름을 추가할 경우 ParsingDataSet에 Enum을 추가한 뒤에 추가한다
        /// string로 줄 경우 실패할 가능성이 크기 때문에 미리정해진 값중 하나를 사용하도록 만들었다.
        ///  = ParsingDataSet.MonsterData등으로 사용할것
        /// </summary>
        protected static ParsingDataSet fileName;

        #endregion

        #region Class

        /// <summary>
        /// 자식이 구현해야 하는 추상 데이터 클래스
        /// </summary>
        public abstract class DataClass
        {
            public Sprite sprite { get; set; }
            protected int KEY;
        }

        #endregion

        #region Callbacks

        public override void OnInitiate()
        {
            ReadTable();
        }

        #endregion

        #region Functions

        public static Dictionary<int, Dictionary<string, string>> Read(ParsingDataSet dataSet)
        {
            string fileName = dataSet.ToString();

            if (fileName == null)
            {
                Debug.Log("파일이 없습니다.");
                return null;
            }

            Dictionary<int, Dictionary<string, string>> list = new Dictionary<int, Dictionary<string, string>>();
            TextAsset data = Resources.Load(fileName) as TextAsset;

            var lines = Regex.Split(data.text, LINE_SPLIT_RE);
            if (lines.Length <= 1) return list;
            string[] header = Regex.Split(lines[0], SPLIT_RE);
            for (var i = 1; i < lines.Length; i++)
            {
                string[] values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "") continue;
                Dictionary<string, string> entry = new Dictionary<string, string>();

                int key = Int32.Parse(values[0]);
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out f))
                    {
                        finalvalue = f;
                    }
                    
                    entry[header[j]] = value;
                    if (!list.ContainsKey(key))
                        list.Add(key, entry);
                }

                
            }
            
            return list;
        }

        public Dictionary<int, T> GetTable() => table;

        public T GetTableData(int key) => table[key];

        public virtual void ReadTable()
        {
            if (table == null)
                table = LoadTable();
        }

        public static Dictionary<int, T> LoadTable()
        {
            var result = new Dictionary<int, T>();
            var tableDataType = typeof(T);

            var fields = tableDataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            Dictionary<int, Dictionary<string, string>> parsedTable = Read(fileName);

            foreach (var collectionPair in parsedTable)
            {
                var tableDataClass = new T();
                var tableKey = collectionPair.Key;
                var tableValue = collectionPair.Value;
                Debug.Log(tableKey);
                for (int i = 0; i < fields.Length; i++)
                {
                    var fieldInfo = fields[i];
                    var fieldName = fieldInfo.Name;
                    var fieldType = fieldInfo.FieldType;
                    
                    if (fieldName == "KEY")
                        fieldInfo.SetValue(tableDataClass, collectionPair.Key);
                    else if (tableValue.ContainsKey(fieldName))
                    {
                        var value = tableValue[fieldName];
                        fieldInfo.SetValue(tableDataClass, value.DecodeType(fieldType));
                    }

                }

                if (!result.ContainsKey(tableKey))
                    result.Add(tableKey, tableDataClass);
            }

            return result;
        }

        #endregion
    }
}
