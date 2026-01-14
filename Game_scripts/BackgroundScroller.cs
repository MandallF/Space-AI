using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // Arka planın kayma hızı (0.1f yavaş, 0.5f hızlı)
    [SerializeField] private float scrollSpeed = 0.1f;
    
    private Renderer meshRenderer;
    private Material myMaterial;

    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        myMaterial = meshRenderer.material;
    }

    void Update()
    {
        // Zamanla artan bir değer oluşturuyoruz (Y ekseninde)
        float offset = Time.time * scrollSpeed;
        
        // Materyalin texture pozisyonunu (Offset) değiştiriyoruz
        Vector2 textureOffset = new Vector2(0, offset);
        
        myMaterial.mainTextureOffset = textureOffset;
    }
}