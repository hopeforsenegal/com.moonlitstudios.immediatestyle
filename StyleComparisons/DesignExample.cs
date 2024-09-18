using UnityEngine;
using UnityEngine.UI;

namespace Editing.BotEditor.StyleComparisons
{
    public class DesignExample : MonoBehaviour
    {
        public Text text;
        int value;

        private void Start()
        {
            Debug.Assert(text != null, "Fancy message", gameObject);
            text.text = $"-> {value}";
        }
        public void RaiseValue()
        {
            value = value + 1;
            text.text = $"-> {value}";
        }
        public void LowerValue()
        {
            value = value - 1;
            text.text = $"-> {value}";
        }
    }
}