using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitAnimator : MonoBehaviour
{
    [Header("Settings")]
    public Image targetImage;        // Image UI yang mau dianimasikan
    public Sprite[] animationFrames; // Masukkan semua frame sprite (Frame 1, 2, 3...) ke sini
    public float frameSpeed = 0.1f;  // Kecepatan animasi (makin kecil makin cepat)

    private int currentFrame;
    private float timer;
    private bool isPlaying = false;
    private Sprite defaultSprite;    // Menyimpan sprite asli saat diam

    void Start()
    {
        if (targetImage == null) 
            targetImage = GetComponent<Image>();
    }

    void Update()
    {
        if (isPlaying && animationFrames.Length > 0)
        {
            timer += Time.deltaTime;
            if (timer >= frameSpeed)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % animationFrames.Length;
                targetImage.sprite = animationFrames[currentFrame];
            }
        }
    }

    public void Play()
    {
        if (animationFrames.Length > 0)
        {
            // Simpan sprite saat ini sebagai default (idle) sebelum mulai animasi
            if (!isPlaying) defaultSprite = targetImage.sprite;
            
            isPlaying = true;
            currentFrame = 0;
        }
    }

    public void Stop()
    {
        isPlaying = false;
        // Kembalikan ke sprite diam (atau frame pertama animasi jika mau)
        if (animationFrames.Length > 0)
            targetImage.sprite = animationFrames[0]; 
    }
}