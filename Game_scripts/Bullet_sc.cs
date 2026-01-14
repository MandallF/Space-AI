using UnityEngine;

public class Bullet_sc : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxY = 5.34f;
    
    [Header("Hasar Ayarlari")]
    [SerializeField] private int damage = 10;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Ekranın üstünden çıkarsa yok et
        if (transform.position.y > maxY)
        {
            Destroy(gameObject);
        }  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Durum: BOSS İLE ÇARPIŞMA
        // Çarptığımız objede "Enemy_sc" var mı diye güvenli bir şekilde bakıyoruz.
        Enemy_sc boss = other.GetComponent<Enemy_sc>();

        if (boss != null) 
        {
            // Boss bulundu, hasar ver
            boss.TakeDamage(damage);
            // Mermiyi yok et
            Destroy(gameObject);
            return; // İşlem bitti, fonksiyondan çık
        }

        // 2. Durum: MINION İLE ÇARPIŞMA
        // Buraya kod yazmamıza gerek yok!
        // Çünkü bir önceki adımda yazdığımız "Minion_sc" scriptinde,
        // Minion mermiyi algılayıp (OnTriggerEnter2D ile) hem XP veriyor hem de mermiyi yok ediyordu.
        // Eğer mermiyi burada yok etmeye çalışırsak, Minion scripti çalışmadan mermi silinebilir ve XP kazanamayız.
    }
}