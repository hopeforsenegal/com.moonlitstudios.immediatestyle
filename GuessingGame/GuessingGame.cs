using System.Collections.Generic;
using MoonlitSystem.UI.Immediate;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// A simple guessing game complete with animations & fading (One with Animator and the other with Math.Lerp)
// + we show a little bit of callback functionality to demonstrate that its possible to mix styles
// And of course we show off Reference and ElementRootMapping (useful for a listing of the same type of thing)

namespace MoonlitSystem
{
    public class GuessingGame : MonoBehaviour
    {
        private const string CanvasMainMenuPlay7538 = "/Canvas/MainMenu/Play7538";
        private const string CanvasMainMenuExitd391 = "/Canvas/MainMenu/Exitd391";
        private const string CanvasMainMenu8959 = "/Canvas/MainMenu8959";
        private const string CanvasGuessingGame5cab = "/Canvas/GuessingGame5cab";
        private const string CanvasGuessingGameFeedback68d7 = "/Canvas/GuessingGame/Feedback68d7";
        private const string CanvasGuessingGameInputFieldLegacy1207 = "/Canvas/GuessingGame/InputField (Legacy)1207";
        private const string CanvasGuessingGameHistoryScrollViewViewportContentListingTextLegacy2cc8 = "/Canvas/GuessingGame/History/Scroll View/Viewport/Content/Listing/Text (Legacy)1ca7";
        private const string CanvasGuessingGameHistoryScrollViewViewportContentListing621f = "/Canvas/GuessingGame/History/Scroll View/Viewport/Content/GameObject788f";
        private const string CanvasGuessingGameFeedbacke989 = "/Canvas/GuessingGame/Feedbacke989";
        private const string CanvasMainMenuPlay2d73 = "/Canvas/MainMenu/Play2d73";

        public enum MainMenuEvent { Play = 1, Exit }
        public struct GameEvent { public int? Guess; }
        public struct GameVisible
        {
            public bool IsShow;
            public float FadeStartTime;
            internal bool ShowHasWon;
            internal string FeedbackText;
            internal string InputFieldText;
        }
        private GameVisible m_VisibleGameUI;
        private readonly List<int> m_PreviousGuesses = new List<int>();
        private int m_GeneratedGuess;
        private Animator m_FeedbackTextAnimator;

        protected void Awake()
        {
            m_FeedbackTextAnimator = Reference.Find<Animator>(this, CanvasGuessingGameFeedbacke989);
            m_GeneratedGuess = Random.Range(1, 10);
            m_FeedbackTextAnimator.enabled = false;
            // We show a little bit of callback functionality to demonstrate that its possible to mix styles
            Reference.Find<Button>(this, CanvasMainMenuPlay2d73).onClick.AddListener(call: () => { Debug.Log("I am from a callback! You can mix styles to avoid changing everything at once!"); });
        }

        protected void Update()
        {
            MainMenuEvent mainMenu = default;
            GameEvent gameEvent = default;

            // Render and stylize UI here
            ImmediateStyle.CanvasGroup(CanvasMainMenu8959);

            mainMenu = ImmediateStyle.Button(CanvasMainMenuPlay7538).IsMouseDown ? MainMenuEvent.Play : mainMenu;
            mainMenu = ImmediateStyle.Button(CanvasMainMenuExitd391).IsMouseDown ? MainMenuEvent.Exit : mainMenu;

            if (m_VisibleGameUI.IsShow) {
                var color = Color.white;
                if (m_VisibleGameUI.ShowHasWon) color = Color.green;

                ImmediateStyle.CanvasGroup(CanvasGuessingGame5cab, out var gameCanvasGroup);
                ImmediateStyle.SetColor(color);
                ImmediateStyle.Text(CanvasGuessingGameFeedback68d7, m_VisibleGameUI.FeedbackText); // Has an animator on it... this is not a problem at all! :-)
                ImmediateStyle.ClearColor();

                gameCanvasGroup.alpha = Mathf.Lerp(0, 1, (Time.time - m_VisibleGameUI.FadeStartTime) * 3); // Lets fade the game screen (and lets do it with a manual lerp)

                if (!m_VisibleGameUI.ShowHasWon) {
                    if (ImmediateStyle.InputField(CanvasGuessingGameInputFieldLegacy1207, new[] { KeyCode.Return, KeyCode.KeypadEnter }, ref m_VisibleGameUI.InputFieldText).HasSubmitted && int.TryParse(m_VisibleGameUI.InputFieldText, out var guessInt)) {
                        gameEvent.Guess = guessInt;
                    }
                }

                // We can handle things that are represent a listing of elements with a ElementRootMapping
                for (var i = 0; i < 4 && i < m_PreviousGuesses.Count; i++) {
                    var r = m_PreviousGuesses.Count - i - 1;
                    ImmediateStyle.CanvasGroup(i + CanvasGuessingGameHistoryScrollViewViewportContentListing621f);
                    ImmediateStyle.Text(i + CanvasGuessingGameHistoryScrollViewViewportContentListingTextLegacy2cc8, m_PreviousGuesses[r].ToString());
                }
            }


            // Handle User feedback here
            if (mainMenu != default) {
                Debug.Log("We clicked one of the two buttons!");
                if (mainMenu == MainMenuEvent.Play) {
                    m_VisibleGameUI = new GameVisible { IsShow = true, FadeStartTime = Time.time };
                }
                if (mainMenu == MainMenuEvent.Exit) {
#if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                }
            }

            if (gameEvent.Guess.HasValue) {
                m_PreviousGuesses.Add(gameEvent.Guess.Value);

                if (gameEvent.Guess > m_GeneratedGuess) {
                    m_VisibleGameUI.FeedbackText = "Lower...";
                } else if (gameEvent.Guess < m_GeneratedGuess) {
                    m_VisibleGameUI.FeedbackText = "Higher...";
                } else {
                    m_VisibleGameUI.FeedbackText = "You Won!";
                    m_VisibleGameUI.ShowHasWon = true;
                    m_FeedbackTextAnimator.enabled = true;  // Add some juicy pulses so the user knows they won!
                }
            }
        }
    }
}