using Unity.Burst;
using UnityEngine;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public abstract class Translater : MonoBehaviour
    {
        [field: SerializeField]
        public string Name { get; private set; }
        public string TranslationString { get; private set; }
        
        protected virtual void OnEnable() => TranslateManager.GameLanguageChanged += ChangeElement;
        protected virtual void Start()
        {
            var field = typeof(Translation).GetField(Name);
            TranslationString = (string)field.GetValue(TranslateManager.Translation);
            ChangeElement();
        }
        protected virtual void OnDisable() => TranslateManager.GameLanguageChanged -= ChangeElement;

        public abstract void ChangeElement();
    }
}
