using UnityEngine;
using UnityEngine.UI;

namespace Editing.BotEditor.StyleComparisons
{
    public class StandardExample : MonoBehaviour
    {
        public Button left, right;
        public Text text;
        int value;
        void Start()
        {
            Debug.Assert(left != null, "Fancy message", gameObject);
            Debug.Assert(right != null);
            Debug.Assert(text != null);
            left.onClick.AddListener(LowerValue);
            right.onClick.AddListener(RaiseValue);
            text.text = $"-> {value}";
        }
        void OnDestroy()
        {
            left.onClick.RemoveListener(LowerValue);
            right.onClick.RemoveListener(RaiseValue);
        }
        private void RaiseValue()
        {
            value = value + 1;
            text.text = $"-> {value}";
        }
        private void LowerValue()
        {
            value = value - 1;
            text.text = $"-> {value}";
        }
    }
}