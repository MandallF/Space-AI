using UnityEngine;

public class Minion_sc : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damageToPlayer = 10;
    [SerializeField] private int xpValue = 10;

    // Ekranın ne kadar altında ışınlanacağı ve ne kadar üstünden çıkacağı
    private float bottomLimit = -5.7f; 
    private float topStartPoint = 5.7f;

    void Update()
    {
        // Sürekli aşağı hareket
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // --- DEĞİŞEN KISIM ---
        // Eğer ekranın alt sınırını geçerse
        if (transform.position.y < bottomLimit)
        {
            // Yok etmek yerine, X eksenini koruyarak Y ekseninde en tepeye taşıyoruz
            transform.position = new Vector3(transform.position.x, topStartPoint, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer Player'a çarparsa
        if (other.CompareTag("Player"))
        {
            Player_sc player = other.GetComponent<Player_sc>();
            if (player != null)
            {
                player.TakeDamage(damageToPlayer);
            }
            Destroy(gameObject); // Player'a çarpınca yok olsun ki içinden geçip gitmesin
        }
        // Eğer Mermiye çarparsa
        else if (other.CompareTag("Bullet"))
        {
            if (LevelManager.instance != null)
            {
                LevelManager.instance.AddXP(xpValue);
            }
            // Mermiyi yok et
            Destroy(other.gameObject); 
            
            // Kendini yok et
            Destroy(gameObject);
        }
    }
}