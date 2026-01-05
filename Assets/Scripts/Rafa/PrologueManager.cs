using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 10)] 
    public string dialogue;
    public Sprite backgroundSprite; 
    public Sprite characterSprite; 
    public Sprite bubbleSprite; // BARU: Slot untuk bentuk awan khusus per baris
}

public class PrologueManager : MonoBehaviour
{
    [Header("UI Components - Text")]
    public TextMeshProUGUI nameText;      
    public TextMeshProUGUI dialogueText;  
    
    [Header("UI Components - Images")]
    public Image backgroundImage;         
    public Image portraitLeft;   
    public Image portraitRight;  

    [Header("Dialogue Layout & Animation")]
    public GameObject dialogueGroup;     
    public Image bubbleImage;            
    public RectTransform textContainer;  
    
    [Header("Position Settings")]
    public float catPositionX = 0f;      
    public float systemPositionX = 0f;   
    public float centerPositionX = 0f; // BARU: Posisi tengah untuk Narasi/SFX
    
    [Header("Fine Tuning")]
    [Tooltip("Geser teks KHUSUS saat System bicara. Isi angka Negatif (-) untuk geser ke Kiri.")]
    public float textFlipOffset = 0f; 
    
    private float defaultTextX; 

    [Header("Typing Settings")]
    public float typingSpeed = 0.04f;     

    [Header("Story Assets - Backgrounds")]
    public Sprite warehouseBg;            
    public Sprite digitalWorldBg;

    public Sprite BlackBG;       

    public Sprite Hypno;  

    [Header("Story Assets - Characters")] 
    public Sprite catSprite;
    public Sprite systemSprite;

    [Header("Story Assets - Bubbles")] // BARU: Aset Bubble
    public Sprite normalBubbleSprite; // Drag gambar awan biasa (bulat) ke sini
    public Sprite noiseBubbleSprite;  // Drag gambar awan tajam/kotak untuk SFX ke sini

    private List<DialogueLine> lines = new List<DialogueLine>();
    private int index;
    private bool isTyping;
    private bool cutsceneFinished = false;

    void Start()
    {
        if (textContainer != null)
        {
            defaultTextX = textContainer.anchoredPosition.x;
        }

        if (normalBubbleSprite == null && bubbleImage != null)
            normalBubbleSprite = bubbleImage.sprite;

        // --- DATA DIALOG ---
        // Scene 1: Warehouse
        AddLine("-", "'The sound of the door being opened'", BlackBG, null, noiseBubbleSprite);
        AddLine("THE CAT", "Damn, sky is crying buckets out there. I ain't dealing with wet fur today.", BlackBG, catSprite);
        AddLine("THE CAT", "At least it's dry in here.", BlackBG, catSprite);
        AddLine("THE CAT", "Smells like dust and... old plastic?", BlackBG, catSprite);
        AddLine("THE CAT", "Whoa. Check this out. It’s like a graveyard in here.", warehouseBg, catSprite);
        AddLine("THE CAT", "Tetris... Tetris 2... Super Tetris... Man, someone was really obsessed with stacking blocks.", warehouseBg, catSprite);
        AddLine("THE CAT", "And what do we have here?", warehouseBg, catSprite);
        AddLine("THE CAT", "\"Tetris Hell\"? Heh. Edgy name.", warehouseBg, catSprite);
        AddLine("THE CAT", "You know what? I'm bored. Let's see if this bad boy still works.", warehouseBg, catSprite);

        AddLine("SYSTEM", "*CLICK* [Machine turns on violently]", BlackBG, null, noiseBubbleSprite);
        AddLine("THE CAT", "Yeah! That’s what I’m talking about!", warehouseBg, catSprite);
        AddLine("THE CAT", "...", warehouseBg, catSprite);
        AddLine("THE CAT", "Wait...", warehouseBg, catSprite);
        AddLine("THE CAT", "Why is the screen swirling like that?", Hypno, catSprite);
        AddLine("THE CAT", "Whoa! Hey! Back off! Let go of my tail!", Hypno, catSprite);
        AddLine("THE CAT", "Oh crap, oh crap, OH CRAAAAAP—!", Hypno, catSprite);

        // Scene 2: Digital World
        AddLine("THE CAT", "Ugh...", digitalWorldBg, catSprite);
        AddLine("THE CAT", "note to self: never play video games in abandoned warehouses.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "Where the hell am I? A disco? Why’s it so bright?", digitalWorldBg, catSprite);
        AddLine("THE CAT", "And what is that bubbling yellow slushie down there? Looks toxic as hell.", digitalWorldBg, catSprite);
        
        AddLine("SYSTEM", "YO! INTRUDER! FREEZE RIGHT THERE! IDENTIFY YOURSELF!", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Jeez! Don't sneak up on me, you TV head!", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "I’m THE SYSTEM! Wait... scans are returning... biological? A cat?", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Like I wanted to be here? Your trash machine kidnapped me! Move it, scrap metal.", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Exit?! Look around you, genius! This is Tetris Hell. Everything is corrupted!", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Yo, watch it! Get that pixel-bug outta my face! ... Huh. Weak sauce.", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Holy sh*t... Did you just delete its source code with your claws? You’re a Glitch.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "Listen. The only exit is at The Core. We deal? Or do you wanna burn?", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Damn it. Fine! Show me the way, rust bucket! But if you trick me, I’m turning you into a litter box.", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Deal! Let's rock and roll! MOVE IT!", digitalWorldBg, systemSprite);

        dialogueGroup.SetActive(true);
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = lines[index].dialogue;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        UpdateUI();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = ""; 
        foreach (char c in lines[index].dialogue.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void NextLine()
    {
        if (index < lines.Count - 1)
        {
            index++;
            UpdateUI();
            StartCoroutine(TypeLine());
        }
        else
        {
            cutsceneFinished = true;
            dialogueGroup.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Cutscene Selesai! Pindah ke Gameplay.");
        }
    }

    void UpdateUI()
    {
        DialogueLine currentLine = lines[index];
        RectTransform groupRect = dialogueGroup.GetComponent<RectTransform>();
        RectTransform bubbleRect = bubbleImage.GetComponent<RectTransform>();

        // 1. Set Teks & Warna Nama
        nameText.text = currentLine.characterName;
        // Kalau System bicara atau SFX mesin, warna Cyan. Kalau kucing, warna Kuning.
        nameText.color = (currentLine.characterName == "THE CAT") ? Color.yellow : Color.cyan;

        // 2. Set Background
        if (currentLine.backgroundSprite != null)
            backgroundImage.sprite = currentLine.backgroundSprite;

        // 3. Set Bentuk Awan (Bubble Sprite)
        // Jika baris ini punya bubble khusus (misal noise), pakai itu. Kalau tidak, pakai normal.
        if (currentLine.bubbleSprite != null)
            bubbleImage.sprite = currentLine.bubbleSprite;
        else
            bubbleImage.sprite = normalBubbleSprite;

        // 4. LOGIKA POSISI & GAMBAR KARAKTER
        if (currentLine.characterSprite != null)
        {
            // === ADA KARAKTER YANG BICARA ===

            if (currentLine.characterName == "SYSTEM")
            {
                // === SYSTEM (KANAN) ===
                portraitLeft.gameObject.SetActive(false);
                portraitRight.gameObject.SetActive(true);
                portraitRight.sprite = currentLine.characterSprite;

                // Flip Awan & Posisi Kanan
                bubbleRect.localScale = new Vector3(-1, 1, 1);
                groupRect.anchoredPosition = new Vector2(systemPositionX, groupRect.anchoredPosition.y);
                float newX = -defaultTextX + textFlipOffset;
                textContainer.anchoredPosition = new Vector2(newX, textContainer.anchoredPosition.y);
            }
            else
            {
                // === THE CAT (KIRI) ===
                portraitLeft.gameObject.SetActive(true);
                portraitRight.gameObject.SetActive(false);
                portraitLeft.sprite = currentLine.characterSprite;

                // Reset Awan & Posisi Kiri
                bubbleRect.localScale = new Vector3(1, 1, 1);
                groupRect.anchoredPosition = new Vector2(catPositionX, groupRect.anchoredPosition.y);
                textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
            }
        }
        else
        {
            // === NARASI / SFX (TIDAK ADA WAJAH) ===
            // Matikan kedua potret
            portraitLeft.gameObject.SetActive(false);
            portraitRight.gameObject.SetActive(false);

            // Reset Awan ke Normal (Tidak Flip)
            bubbleRect.localScale = new Vector3(1, 1, 1);
            
            // Posisi Dialog di Tengah (Netral)
            groupRect.anchoredPosition = new Vector2(centerPositionX, groupRect.anchoredPosition.y);
            
            // Teks di posisi asli (Positif)
            textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
        }
    }

    // Update fungsi AddLine untuk mendukung parameter bubble optional
    void AddLine(string name, string text, Sprite bg, Sprite portrait, Sprite bubble = null)
    {
        DialogueLine newLine = new DialogueLine();
        newLine.characterName = name;
        newLine.dialogue = text;
        newLine.backgroundSprite = bg;
        newLine.characterSprite = portrait; 
        newLine.bubbleSprite = bubble; // Simpan bubble khusus jika ada
        lines.Add(newLine);
    }
}