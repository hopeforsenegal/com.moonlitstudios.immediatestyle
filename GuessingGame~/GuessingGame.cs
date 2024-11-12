using System.Collections.Generic;
using MoonlitSystem.UI.Immediate;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitSystem
{
    public class GuessingGame : MonoBehaviour
    {
        private const string CanvasGuessingGameRestartButton637f = "/Canvas/GuessingGame/RestartButton637f";
        private const string CanvasGuessingGameExitButtonf275 = "/Canvas/GuessingGame/ExitButtonf275";
        private const string CanvasMainMenuPlay7538 = "/Canvas/MainMenu/Play7538";
        private const string CanvasMainMenuExitd391 = "/Canvas/MainMenu/Exitd391";
        private const string CanvasMainMenu8959 = "/Canvas/MainMenu8959";
        private const string CanvasGuessingGame3733 = "/Canvas/GuessingGame3733";
        private const string CanvasGuessingGameFeedback68d7 = "/Canvas/GuessingGame/Feedback68d7";
        private const string CanvasGuessingGameInputFieldLegacy1207 = "/Canvas/GuessingGame/InputField (Legacy)1207";
        private const string CanvasGuessingGameHistoryScrollViewViewportContentListingTextLegacy2cc8 = "/Canvas/GuessingGame/History/Scroll View/Viewport/Content/Listing/Text (Legacy)1ca7";
        private const string CanvasGuessingGameHistoryScrollViewViewportContentListing621f = "/Canvas/GuessingGame/History/Scroll View/Viewport/Content/GameObject788f";
        private const string CanvasGuessingGameFeedbacke989 = "/Canvas/GuessingGame/Feedbacke989";
        private const string CanvasMainMenuPlay2d73 = "/Canvas/MainMenu/Play2d73";
        private const string SFX3814 = "/SFX3814";

        private enum MainMenuEvent { Play = 1, Exit }
        private struct GameEvent
        {
            public int? Guess;
            public bool Exit;
            internal bool Restart;
        }
        private struct GameVisible
        {
            public bool IsShow;
            public float FadeStartTime;
            internal bool ShowHasWon;
            internal string FeedbackText;
            internal string InputFieldText;
            internal bool IsExitButtonHover;
        }
        public AudioClip click;
        private AudioSource m_Sfx;
        private GameVisible m_VisibleGameUI;
        private int m_GeneratedGuess;
        private GuessingGameButtonExtra[] m_GuessingGameButtonExtras;
        private Animator m_FeedbackTextAnimator;
        private readonly Dictionary<GuessingGameButtonExtra, ElementButton> m_MappingExtra = new Dictionary<GuessingGameButtonExtra, ElementButton>();
        private readonly List<int> m_PreviousGuesses = new List<int>();

        protected void Awake()
        {
            m_GuessingGameButtonExtras = FindObjectsOfType<GuessingGameButtonExtra>();
            m_FeedbackTextAnimator = Reference.Find<Animator>(this, CanvasGuessingGameFeedbacke989);
            m_Sfx = Reference.Find<AudioSource>(this, SFX3814);
            m_Sfx.clip = click;

            // We could just call GetComponent in Update which would be 10x slower on the CPU... It would work in the majority of cases.
            // Profile to actually see where your "real" bottlenecks are (its almost never the thing you suspect most)
            // See GuessingGameButtonExtra.cs for details on what this functionality is about
            foreach (var extra in m_GuessingGameButtonExtras) m_MappingExtra.Add(extra, extra.GetComponent<ElementButton>());

            // We show a little bit of callback functionality to demonstrate that its possible to mix styles
            Reference.Find<Button>(this, CanvasMainMenuPlay2d73).onClick.AddListener(call: () =>
            {
                // NOTE: onClick fires on OnPointerClick. See ElementButton.cs for details
                Debug.Log("I am from an 'onClick' callback! You can mix styles to avoid changing everything at once!");
            });
        }

        protected void Update()
        {
            MainMenuEvent mainMenu = default;
            GameEvent gameEvent = default;

            /******************************/
            /* Render and stylize UI here */
            ImmediateStyle.CanvasGroup(CanvasMainMenu8959, updateCanvasGroupInLateUpdate: lateUpdateMainMenuCanvasGroup => // In this case we always show out canvas group.. but change the fields on the component conditionally
            {
                var alpha = 1f;
                if (m_VisibleGameUI.IsShow) alpha = Mathf.Lerp(1, 0, (Time.time - m_VisibleGameUI.FadeStartTime) * 3); // Lets fade the game screen (and lets do it with a manual lerp)

                lateUpdateMainMenuCanvasGroup.interactable = !m_VisibleGameUI.IsShow;
                lateUpdateMainMenuCanvasGroup.blocksRaycasts = !m_VisibleGameUI.IsShow;
                lateUpdateMainMenuCanvasGroup.alpha = alpha;
            });

            if (!m_VisibleGameUI.IsShow) {
                mainMenu = ImmediateStyle.Button(CanvasMainMenuPlay7538).IsMouseUp/*<-- see comment*/ ? MainMenuEvent.Play : mainMenu; // NOTE: See ElementButton.cs for details as we suggest actually using IsMouseDown instead of IsMouseUp.
                mainMenu = ImmediateStyle.Button(CanvasMainMenuExitd391).IsMouseDown || Input.GetKeyDown(KeyCode.Escape) ? MainMenuEvent.Exit : mainMenu; // We can check button presses from keyboard and UI on the same line
            } else {
                var color = Color.white;
                if (m_VisibleGameUI.ShowHasWon) color = Color.green;

                ImmediateStyle.CanvasGroup(CanvasGuessingGame3733);
                ImmediateStyle.SetColor(color);
                ImmediateStyle.Text(CanvasGuessingGameFeedback68d7, m_VisibleGameUI.FeedbackText); // Has an animator on it... this is not a problem at all! :-)
                ImmediateStyle.ClearColor();

                gameEvent.Restart = ImmediateStyle.Button(CanvasGuessingGameRestartButton637f).IsMouseDown || Input.GetKeyDown(KeyCode.R); // We can check button presses from keyboard and UI on the same line

                ImmediateStyle.SetColor(m_VisibleGameUI.IsExitButtonHover ? Color.red : Color.white);
                var exitButtonInteraction = ImmediateStyle.Button(CanvasGuessingGameExitButtonf275); // We can handle button hovers ourselves if we want
                ImmediateStyle.ClearColor();
                m_VisibleGameUI.IsExitButtonHover = exitButtonInteraction.IsMouseHovering;
                gameEvent.Exit = exitButtonInteraction.IsMouseDown || Input.GetKeyDown(KeyCode.Escape);
                if (!m_VisibleGameUI.ShowHasWon) {
                    if (ImmediateStyle.InputField(CanvasGuessingGameInputFieldLegacy1207, new[] { KeyCode.Return, KeyCode.KeypadEnter }, ref m_VisibleGameUI.InputFieldText).HasSubmitted && int.TryParse(m_VisibleGameUI.InputFieldText, out var guessInt)) {
                        gameEvent.Guess = guessInt;
                    }
                }

                // We can handle things that are represent a listing of elements with a 'RootMapping' component.
                // It basically allows you to reuse Prefabs or reuse GameObject Hierarchies
                for (var i = 0; i < 4 && i < m_PreviousGuesses.Count; i++) {
                    var r = m_PreviousGuesses.Count - i - 1;
                    ImmediateStyle.CanvasGroup(i + CanvasGuessingGameHistoryScrollViewViewportContentListing621f);
                    ImmediateStyle.Text(i + CanvasGuessingGameHistoryScrollViewViewportContentListingTextLegacy2cc8, m_PreviousGuesses[r].ToString());
                }
            }

            /***************************/
            /* Handle User events here */

            // Unique "text" per button (optional and designer driven)
            // Add GuessingGameButtonExtra (with a message) to any button as many times as you want
            // No need to change or add code its all handled by this 5 line for loop
            // This is dealing with "Retained Mode" native Unity GameObjects (ElementButton & GuessingGameButtonExtra) directly,
            // so there is no need to interact with the ImmediateStyle API here.
            foreach (var extra in m_MappingExtra) {
                if (!extra.Value.IsMouseDown) continue;
                if (string.IsNullOrWhiteSpace(extra.Key.message)) continue;
                Debug.Log(extra.Key.message);
            }

            if (mainMenu != default) {
                Debug.Log("We clicked one of the two main menu buttons or hit 'Escape'!");
                m_Sfx.Play();
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

            if (gameEvent.Restart || mainMenu == MainMenuEvent.Play) { // Combined logic on starting a new game (Play has some of its own logic above ^)
                m_PreviousGuesses.Clear();
                m_GeneratedGuess = Random.Range(1, 10);
                m_FeedbackTextAnimator.Rebind();
                m_FeedbackTextAnimator.Update(0);
                m_FeedbackTextAnimator.enabled = false;
                m_VisibleGameUI.FeedbackText = string.Empty;
                m_VisibleGameUI.InputFieldText = string.Empty;
                m_VisibleGameUI.ShowHasWon = false;
            }

            if (gameEvent.Guess.HasValue) {
                m_PreviousGuesses.Add(gameEvent.Guess.Value);
                m_Sfx.Play();

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
            if (gameEvent.Exit) {
                m_VisibleGameUI.IsShow = false;
            }
        }
    }
}