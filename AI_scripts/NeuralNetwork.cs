using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class NeuralNetwork
{
    public int[] layers; // Katman yapıları (örn: 3 girdi, 5 gizli, 1 çıktı)
    public float[][] neurons; // Nöron değerleri
    public float[][][] weights; // Ağırlıklar (Öğrenilen bilgi burası)
    public float learningRate = 0.1f; // Öğrenme hızı (Çok yüksekse sapıtır, çok düşükse öğrenmez)

    // Yapıcı Fonksiyon: Beyni oluşturur
    public NeuralNetwork(int[] layerStructure)
    {
        this.layers = new int[layerStructure.Length];
        for (int i = 0; i < layerStructure.Length; i++)
            this.layers[i] = layerStructure[i];

        InitNeurons();
        InitWeights();
    }

    // Nöronları ve Ağırlıkları başlatma
    void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);
        }
        neurons = neuronsList.ToArray();
    }

    void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = layers[i - 1];
            for (int j = 0; j < layers[i]; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    // Başlangıçta rastgele ağırlıklar veriyoruz (-0.5 ile 0.5 arası)
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }

    // İLERİ BESLEME (Karar Verme Anı)
    public float[] FeedForward(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = (float)Math.Tanh(value); // Aktivasyon fonksiyonu (-1 ile 1 arası yapar)
            }
        }
        return neurons[neurons.Length - 1]; // Çıktı katmanını döndür
    }

    // EĞİTİM (Backpropagation - Geri Yayılım)
    // AI senin yaptığın hareketi (expected) alır, kendi tahminiyle kıyaslar ve beynini düzeltir.
    public void Train(float[] inputs, float[] expectedOutputs)
    {
        float[] output = FeedForward(inputs);

        float[][] gamma; // Hata miktarları
        List<float[]> gammaList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            gammaList.Add(new float[layers[i]]);
        }
        gamma = gammaList.ToArray();

        int layerCount = layers.Length;

        // Çıktı Katmanındaki Hatayı Hesapla
        for (int i = 0; i < output.Length; i++)
        {
            float error = expectedOutputs[i] - output[i];
            gamma[layerCount - 1][i] = error * (1 - output[i] * output[i]); // Tanh türevi
        }

        // Gizli Katmanlardaki Hatayı Hesapla
        for (int i = layerCount - 2; i > 0; i--)
        {
            for (int j = 0; j < layers[i]; j++)
            {
                float gammaSum = 0;
                for (int k = 0; k < layers[i + 1]; k++)
                {
                    gammaSum += gamma[i + 1][k] * weights[i][k][j];
                }
                gamma[i][j] = gammaSum * (1 - neurons[i][j] * neurons[i][j]); // Tanh türevi
            }
        }

        // Ağırlıkları Güncelle
        for (int i = 1; i < layerCount; i++)
        {
            for (int j = 0; j < layers[i]; j++)
            {
                for (int k = 0; k < layers[i - 1]; k++)
                {
                    weights[i - 1][j][k] += learningRate * gamma[i][j] * neurons[i - 1][k];
                }
            }
        }
    }
}