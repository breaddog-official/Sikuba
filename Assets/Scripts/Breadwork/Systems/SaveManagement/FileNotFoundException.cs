using System;
using Unity.Burst;

namespace Scripts.SaveManagement
{
    [BurstCompile]
    public class FileNotFoundException : Exception
    {
        public override string Message { get; }

        public FileNotFoundException(string message)
        {
            Message = message;
        }
    }
}
