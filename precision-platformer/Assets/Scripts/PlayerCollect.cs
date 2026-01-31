using UnityEngine;
using TMPro;

public class PlayerCollect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int fruits = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);

            fruits++;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Fruits: " + fruits;
        }
    }
}
