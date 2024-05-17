using Unity.Burst;
using UnityEngine.UI;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public class TextUITranslater : Translater
    {
        private Text text;

        protected override void OnEnable()
        {
            text = text ? text : GetComponent<Text>();
            base.OnEnable();
        }
        public override void ChangeElement()
        {
            text.text = TranslationString;
        }
    }
}
