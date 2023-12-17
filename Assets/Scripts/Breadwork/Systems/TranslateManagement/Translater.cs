using Unity.Burst;
using UnityEngine;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public abstract class Translater : MonoBehaviour
    {
        [field: SerializeField]
        public int Index { get; protected set; } 

        protected virtual void OnEnable() => TranslateManager.GameLanguageChanged += ChangeElement;
        protected virtual void Start() => ChangeElement();
        protected virtual void OnDisable() => TranslateManager.GameLanguageChanged -= ChangeElement;

        public abstract void ChangeElement();
    }
}
