using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int remainingFruits = GameObject.FindGameObjectsWithTag("Collectible").Length;
            if (remainingFruits == 0)
            {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(currentSceneIndex + 1);
                }
                else
                {
                    Debug.Log("You've completed all levels!");
                }
            }
            else
            {
                Debug.Log("Collect all fruits before finishing the level!");
            }
        }
    }
}
