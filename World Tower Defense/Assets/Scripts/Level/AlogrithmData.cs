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
            public float[] fitnessClearTimes;
            public float[] clearTimes;
            //이를 통해 성공률을 산출 할 수 있음.
            public int clearStage = 0;
            //아래를 통해 유전자를 뽑을 수 있음.
            public int[] gens;
        }

    }
}
