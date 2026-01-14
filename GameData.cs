public static class GameData
{
    // Dosya yüklendi mi?
    public static bool isAILoaded = false; 
    
    // Yüklenen ağırlık verisi (JSON formatında metin)
    public static string jsonBrainData = "";
    
    // Eğitim modu aktif mi? (Main Menu'de gizli bir tuşla veya checkbox ile açabilirsin)
    public static bool isTrainingMode = false;
}