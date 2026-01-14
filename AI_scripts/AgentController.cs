using UnityEngine;
using System.Collections.Generic; // List hatasını çözer
using System.IO; // Dosya işlemleri için

public class AgentController : MonoBehaviour
{
    private NeuralNetwork brain;
    private Player_sc playerScript;

    // 3 Girdi (Benim X, Düşman X, Düşman Y), 5 Gizli Nöron, 1 Çıktı (Yön)
    private int[] layers = new int[] { 3, 5, 1 }; 

    [Header("Durum")]
    public bool trainingMode = false; 
    public bool aiActive = false;

    void Start()
    {
        playerScript = GetComponent<Player_sc>();
        
        // Beyni oluştur
        brain = new NeuralNetwork(layers);

        // --- DURUM KONTROLÜ ---
        // GameData scriptinin var olduğundan emin ol
        if (GameData.isAILoaded)
        {
            Debug.Log("AI Hafızası Yüklendi. Pilot koltuğunda AI var.");
            aiActive = true;
            trainingMode = false;
            LoadBrain(GameData.jsonBrainData);
        }
        else if (GameData.isTrainingMode)
        {
            Debug.Log("EĞİTİM MODU! Sen oyna, AI öğrensin.");
            aiActive = false;
            trainingMode = true;
        }
        else
        {
            Debug.Log("Dosya yok, Eğitim yok. Rastgele mod.");
            aiActive = true; 
        }
    }

    void Update()
    {
        // 1. GİRDİLERİ AL (GÖZLEM)
        float[] inputs = GetInputs();

        // --- EĞER OYUNCU OYNUYORSA VE EĞİTİM AÇIKSA ---
        if (trainingMode)
        {
            // Senin ne bastığını alıyoruz (-1: Sol, 0: Dur, 1: Sağ)
            float manualInput = Input.GetAxisRaw("Horizontal");
            
            // AI eğitiliyor
            brain.Train(inputs, new float[] { manualInput });
            
            // "K" tuşuna basarsan o anki beyni kaydeder
            // #if UNITY_EDITOR kodu, bu kısmın SADECE Unity Editöründe çalışmasını sağlar.
            // WebGL build aldığında bu satırlar otomatik olarak silinir, böylece hata vermez.
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.K))
            {
                SaveBrainToFile();
            }
            #endif
        }
        // --- EĞER AI OYNUYORSA ---
        else if (aiActive)
        {
            float[] outputs = brain.FeedForward(inputs);
            float decision = outputs[0]; 

            // Kararı direkt Player scriptine gönderiyoruz.
            // X ekseni için karar veriyor, Y ekseni için 0 (hareket yok) gönderiyoruz.
            if (decision > 0.3f) playerScript.cubeMovement(1f, 0f);      // Sağa git
            else if (decision < -0.3f) playerScript.cubeMovement(-1f, 0f); // Sola git
            else playerScript.cubeMovement(0f, 0f);                        // Dur
        }
    }

    float[] GetInputs()
    {
        GameObject closestThreat = FindClosestThreat();
        
        float threatX = 0;
        float threatY = 0;

        if (closestThreat != null)
        {
            threatX = closestThreat.transform.position.x / 8f; 
            threatY = closestThreat.transform.position.y / 10f;
        }

        float myX = transform.position.x / 8f;
        return new float[] { myX, threatX, threatY };
    }

    GameObject FindClosestThreat()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                closest = t;
                minDist = dist;
            }
        }
        return closest;
    }

    // --- KAYDETME VE YÜKLEME (Unity JsonUtility Kullanarak) ---

    public void SaveBrainToFile()
    {
        // Veriyi düzleştirip paketliyoruz
        BrainData data = new BrainData();
        data.SetWeights(brain.weights);
        
        // Unity'nin kendi JSON dönüştürücüsü
        string json = JsonUtility.ToJson(data);

        string path = Application.dataPath + "/ai_weights.json";
        File.WriteAllText(path, json);
        Debug.Log("BEYİN KAYDEDİLDİ: " + path);
    }

    public void LoadBrain(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString)) return;

        BrainData data = JsonUtility.FromJson<BrainData>(jsonString);
        this.brain.weights = data.GetWeights(layers);
    }
}

// JSON Serileştirme için yardımcı sınıf (En alta ekle veya ayrı dosyada tut)
[System.Serializable]
public class BrainData
{
    public List<float> flatWeights; // 3 Boyutlu diziyi tek boyuta indirip saklayacağız

    // 3 Boyutlu diziyi (float[][][]) alır ve List<float>'a çevirir
    public void SetWeights(float[][][] weights3D)
    {
        flatWeights = new List<float>();
        for(int i=0; i<weights3D.Length; i++)
        {
            for(int j=0; j<weights3D[i].Length; j++)
            {
                for(int k=0; k<weights3D[i][j].Length; k++)
                {
                    flatWeights.Add(weights3D[i][j][k]);
                }
            }
        }
    }

    // List<float>'ı geri 3 Boyutlu diziye çevirir
    public float[][][] GetWeights(int[] layers)
    {
        List<float[][]> wList = new List<float[][]>();
        int count = 0;
        
        for (int i = 0; i < layers.Length - 1; i++)
        {
            List<float[]> layerList = new List<float[]>();
            int rows = layers[i + 1];
            int cols = layers[i];
            
            for (int j = 0; j < rows; j++)
            {
                float[] neuronWeights = new float[cols];
                for (int k = 0; k < cols; k++)
                {
                    if (count < flatWeights.Count)
                        neuronWeights[k] = flatWeights[count++];
                    else
                        neuronWeights[k] = 0f;
                }
                layerList.Add(neuronWeights);
            }
            wList.Add(layerList.ToArray());
        }
        return wList.ToArray();
    }
}