using UnityEngine;
using UnityEngine.UI;

namespace Editing.BotEditor.StyleComparisons
{
    // This follows the standard way of handling UI. 
    // For the ImmediateStyle UI code with the same functionality see 'ImmediateExample.cs'
    // For the design way people write UI code with the same functionality see 'DesignExample.cs'
    // Compare and contrast to see what works best and is faster for iteration

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