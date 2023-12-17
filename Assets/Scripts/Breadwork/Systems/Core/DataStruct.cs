using System;
using Unity.Burst;

namespace Scripts.Core
{
    [Serializable, BurstCompile]
    public class DataStruct
    {
        public const string SAVE_NAME = "data";
    }
}