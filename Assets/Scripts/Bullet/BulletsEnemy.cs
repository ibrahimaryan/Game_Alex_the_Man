using UnityEngine;

public class BulletsEnemy : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private float direction = 1f;

    public void ActivateProjectile()
    {
        lifetime = 0;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Jalankan logika dari parent script dulu

        if (collision.CompareTag("Player"))
        {
            DeactivateBullet(); // Nonaktifkan hanya jika tag-nya "Player Bullet"
        }
    }


    public void SetDirection(float _direction)
    {
        direction = _direction;
        lifetime = 0f; // reset waktu hidup peluru

        // Pastikan collider aktif
        if (TryGetComponent(out Collider2D col))
            col.enabled = true;

        // Flip arah kalau perlu
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != Mathf.Sign(direction))
            transform.localScale = new Vector3(-localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Pastikan collider aktif
        if (TryGetComponent(out Collider2D col))
            col.enabled = true;

        // Reset velocity jika ada Rigidbody2D
        if (TryGetComponent(out Rigidbody2D rb))
            rb.velocity = Vector2.zero;
    }

}