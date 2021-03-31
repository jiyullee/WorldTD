using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameData
{
    public static class SystemConfig
    {
        private static Dictionary<Type, Func<string, object>> DecodeByBaseCollection =
            new Dictionary<Type, Func<string, object>>
            {
                {typeof(string), (valueString) => valueString},
                {typeof(byte), (valueString) => ChangeType<byte>(valueString)},
                {typeof(sbyte), (valueString) => ChangeType<sbyte>(valueString)},
                {typeof(Int16), (valueString) => ChangeType<Int16>(valueString)},
                {typeof(Int32), (valueString) => ChangeType<Int32>(valueString)},
                {typeof(Int64), (valueString) => ChangeType<Int64>(valueString)},
                {typeof(UInt16), (valueString) => ChangeType<UInt16>(valueString)},
                {typeof(UInt32), (valueString) => ChangeType<UInt32>(valueString)},
                {typeof(UInt64), (valueString) => ChangeType<UInt64>(valueString)},
                {typeof(float), (valueString) => ChangeType<float>(valueString)},
                {typeof(double), (valueString) => ChangeType<double>(valueString)},
                {typeof(bool), (valueString) => ChangeType<bool>(valueString)},
            };


        /// <summary>
        /// 지정된 타입으로 변환하는 제네릭 메소드
        /// </summary>
        public static T ChangeType<T>(object obj)
        {
            return (T) Convert.ChangeType(obj, typeof(T));
        }

        public static object DecodeType(this string value, Type type)
        {
            if (DecodeByBaseCollection.ContainsKey(type))
                return DecodeByBaseCollection[type](value);
            else
                return null;
        }
    }

}