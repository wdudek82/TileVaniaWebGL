using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField]
    private AudioClip coinPickupSfx;

    [SerializeField]
    private int pointsForCoinPickup = 100;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(coinPickupSfx, Camera.main.transform.position);
        }
        Destroy(gameObject);
        var gameSession = FindObjectOfType<GameSession>();
        gameSession.IncrementScore(pointsForCoinPickup);
    }
}
