using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 20f;

    [SerializeField]
    private int pointsForEnemy = 10;

    private Rigidbody2D _rigidbody2D;
    private PlayerMovement _player;
    private float _xDirection;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerMovement>();
        _xDirection = _player.transform.localScale.x;
        bulletSpeed *= _xDirection;
    }

    void Update()
    {
        _rigidbody2D.velocity = new Vector2(bulletSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);
            var gameSession = FindObjectOfType<GameSession>();
            gameSession.IncrementScore(pointsForEnemy);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
