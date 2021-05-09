using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class AlogrithmData : DataSets<AlogrithmData, AlogrithmData.AlogrithmDataClass>
    {
        public override void OnCreated()
        {
            base.OnCreated();
            fileName = ParsingDataSet.FitTimeListData;
        }

        public class AlogrithmDataClass : DataClass
        {
            //아래를 통해 클리어 시간을 산출 가능
            public float fitnessClearTimeRate;
            // 클리어 비율 (아직 사용안됨)
            public int ClearRate = 0;
        }

    }
}
