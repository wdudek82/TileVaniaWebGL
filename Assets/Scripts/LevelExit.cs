using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = FindObjectOfType<PlayerMovement>();
        if (!col.CompareTag("Player") || !player.IsAlive) return;
        StartCoroutine(LoadNextLevel(1));
    }

    private IEnumerator LoadNextLevel(int delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        var nextSceneToLoad = currentSceneIndex + 1;
        if (nextSceneToLoad == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneToLoad = 0;
        }

        var scenePersist = FindObjectOfType<ScenePersist>();
        scenePersist.ResetScenePersist();
        SceneManager.LoadScene(nextSceneToLoad);
    }
}
