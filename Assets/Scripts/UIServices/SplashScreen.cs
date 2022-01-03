using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIServices
{ 
    // To handle custom splash screen behaviour.
    public class SplashScreen : MonoBehaviour
    {
        // Splash screen image.
        public void play()
        {
            SceneManager.LoadScene("Game");
        }

        public void credits()
        {
            SceneManager.LoadScene("Credits");
        }

        public void back()
        {
            SceneManager.LoadScene("StartScreen");
        }

        public void quit()
        {
            Application.Quit();
        }
    }
}
