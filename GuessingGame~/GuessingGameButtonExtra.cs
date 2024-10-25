using UnityEngine;

namespace MoonlitSystem
{
    public class GuessingGameButtonExtra : MonoBehaviour
    {
        // Commented out because we already have audio in the example but...
        // you could imagine you could play unique audio per button with this all set by a designer
        // public AudioClip clip;

        // To drive the point home the designer can "optionally" toss a component like this on to output something unique
        // This is an example but imagine analytics per button, a color, an enum, volume level, or specific audio clip.
        // Basically anything that would normally NOT be on a ScriptableObject but truly per GameObject
        public string message;
    }
}