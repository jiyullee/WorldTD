using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CsvPaser : MonoBehaviour
{
    //메타 문자열

    //아래의 문자들을 기준으로 라인을 나눔(주로 ,사용)
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    //라인 나눌때 쓰는 메타 문자열
    //\r\n = 맨 앞을 줄바꿈
    //\n\r = 줄바꿈 이후 맨 앞에 포인터
    //\n   = 줄바꿈
    //\r   = 커서를 줄 앞으로 밀어냄
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    //List이기 때문에 변수명[(int)stage][(string)Key] 로 쓰면됨.
    public static List<Dictionary<string, object>> Read(string file)
    {
        //반환한 리스트
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        //한줄씩 잘라서 lines에 삽입
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        //한줄 미만이면 반환
        if (lines.Length <= 1) return list;
        //제목 구분
        string[] header = Regex.Split(lines[0], SPLIT_RE);
        //1번부터 데이터 들어감
        for (var i = 1; i < lines.Length; i++)
        {
            //라인을 쪼갬
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            //값이 없으면 다음값
            if (values.Length == 0 || values[0] == "") continue;

            //각각의 객체를 저장함
            Dictionary<string, object> entry = new Dictionary<string, object>();

            //각각에 있는 단어를 넣는다.
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                //TrimStart 문자의 앞의"와 공백을 제거.
                //TrimEnd  문자의 뒤의"와 공백을 제거.
                //문자의 Replace값중 \와" 을 없애줌.
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                //TryParse 반환이 성공하면 두번째 인자에 값을 주고 실패하면 0 False를줌
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }

                //헤더에 맞는 값을 딕셔너리에 저장
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}