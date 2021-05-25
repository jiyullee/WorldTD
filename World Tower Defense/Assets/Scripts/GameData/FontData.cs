using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GameData
{
    public class FontData : DataSets<FontData, FontData.FontDataClass>
    {
        public override void OnCreated()
        {
            base.OnCreated();
            fileName = ParsingDataSet.FontData;
        }

        public class FontDataClass : DataClass
        {
            public string FontName;
            public string Path;
        }
    }
}

