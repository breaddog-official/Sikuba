using TMPro;
using Unity.Burst;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public class TextMeshProTranslater : Translater
    {
        private TMP_Text text;

        protected override void OnEnable()
        {
            text = text ? text : GetComponent<TMP_Text>();
        }
        public override void ChangeElement()
        {
            text.SetText(TranslateManager.Translation[Index]);
        }
    }
}
