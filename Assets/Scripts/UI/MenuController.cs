using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeatMayhem.UI
{
    public class MenuController : MonoBehaviour
    {
        public void StartGame(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void Restart()
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }

        public void Quit()
        {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
        }
    }
}
