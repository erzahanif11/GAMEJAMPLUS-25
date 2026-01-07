using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueLine2
{
    public string characterName;
    [TextArea(3, 10)] 
    public string dialogue;
    public Sprite backgroundSprite; 
    public Sprite characterSprite; 
    public Sprite bubbleSprite; 
    public AudioClip soundEffect;
}

public class PrologueManager2 : MonoBehaviour
{
    public static PrologueManager2 instance;

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
    public float centerPositionX = 0f; 
    
    [Header("Fine Tuning")]
    [Tooltip("Geser teks KHUSUS saat System bicara. Isi angka Negatif (-) untuk geser ke Kiri.")]
    public float textFlipOffset = 0f; 
    
    private float defaultTextX; 

    [Header("Typing Settings")]
    public float typingSpeed = 0.04f;     

    [Header("Audio Settings")] 
    public AudioSource audioSource; 
    public AudioClip typingSfx;     

    [Header("Story Assets - SFX")] 
    public AudioClip doorOpenSfx;
    public AudioClip machineTurnOnSfx;
    public AudioClip hypnoSfx;
    public AudioClip rainSfx;
    public AudioClip ScanSfx;
    public AudioClip RoboSfx;
    public AudioClip LavaSfx;

    [Header("Story Assets - Backgrounds")]
    public Sprite warehouseBg;            
    public Sprite digitalWorldBg;
    public Sprite BlackBG;       
    public Sprite Hypno;  

    [Header("Story Assets - Characters")] 
    public Sprite catSprite;
    public Sprite systemSprite;

    [Header("Story Assets - Bubbles")] 
    public Sprite normalBubbleSprite; 
    public Sprite noiseBubbleSprite;  

    private List<DialogueLine2> lines = new List<DialogueLine2>();
    private int index;
    private bool isTyping;
    private bool isEnd = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (textContainer != null)
        {
            defaultTextX = textContainer.anchoredPosition.x;
        }

        if (normalBubbleSprite == null && bubbleImage != null)
            normalBubbleSprite = bubbleImage.sprite;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        AddLine("", "'After Phase 1'", BlackBG, null, noiseBubbleSprite, LavaSfx);
        AddLine("THE CAT", "", digitalWorldBg, catSprite);
        AddLine("THE CAT", "Okay… why are there more of these assholes, and why is the lava faster?", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Well… Spawn rate just spiked.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Of course it is.", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Uhm…. Be careful of more enemies. They’re close now.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "So they’re trying to corner me.", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Exactly.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Alright then…. Let’s do this.", digitalWorldBg, catSprite);
        AddLine("", "'Enter phase 2'", BlackBG, null, noiseBubbleSprite, LavaSfx);

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
            
            // Suara ketikan
            if (audioSource != null && typingSfx != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f); 
                audioSource.PlayOneShot(typingSfx);
            }

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
            dialogueGroup.SetActive(false);
            Debug.Log("Prologue 2 Selesai! Pindah Scene.");
            isEnd = true;
            // SceneManager.LoadScene("Prologue3");
        }
    }

    void UpdateUI()
    {
        DialogueLine2 currentLine = lines[index];
        RectTransform groupRect = dialogueGroup.GetComponent<RectTransform>();
        RectTransform bubbleRect = bubbleImage.GetComponent<RectTransform>();

        // Mainkan Suara Spesifik (Jika Ada)
        if (currentLine.soundEffect != null && audioSource != null)
        {
            audioSource.pitch = 1f; 
            audioSource.PlayOneShot(currentLine.soundEffect);
        }

        nameText.text = currentLine.characterName;
        // Kalau System atau "?" bicara, warna Cyan. Kalau kucing, warna Kuning.
        if (currentLine.characterName == "THE CAT")
            nameText.color = Color.yellow;
        else
            nameText.color = Color.cyan;

        // Set Background
        if (currentLine.backgroundSprite != null)
            backgroundImage.sprite = currentLine.backgroundSprite;

        // Set Bentuk Awan (Bubble Sprite)
        if (currentLine.bubbleSprite != null)
            bubbleImage.sprite = currentLine.bubbleSprite;
        else
            bubbleImage.sprite = normalBubbleSprite;

        // --- LOGIKA POSISI & GAMBAR KARAKTER ---
        if (currentLine.characterSprite != null)
        {
            // === ADA KARAKTER YANG BICARA ===

            // Jika SYSTEM bicara
            if (currentLine.characterName == "SYSTEM")
            {
                SetRightSide(currentLine, groupRect, bubbleRect);
            }
            // Jika karakter misterius "?" bicara
            else if (currentLine.characterName == "?")
            {
                SetRightSide(currentLine, groupRect, bubbleRect);
            }
            // Jika THE CAT bicara
            else
            {
                SetLeftSide(currentLine, groupRect, bubbleRect);
            }
        }
        else
        {
            // === NARASI / SFX (TIDAK ADA WAJAH) ===
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

    // Fungsi helper
    void SetRightSide(DialogueLine2 line, RectTransform group, RectTransform bubble)
    {
        portraitLeft.gameObject.SetActive(false);
        portraitRight.gameObject.SetActive(true);
        portraitRight.sprite = line.characterSprite;

        // Flip Awan & Posisi Kanan
        bubble.localScale = new Vector3(-1, 1, 1);
        group.anchoredPosition = new Vector2(systemPositionX, group.anchoredPosition.y);
        
        float newX = -defaultTextX + textFlipOffset;
        textContainer.anchoredPosition = new Vector2(newX, textContainer.anchoredPosition.y);
    }

    void SetLeftSide(DialogueLine2 line, RectTransform group, RectTransform bubble)
    {
        portraitLeft.gameObject.SetActive(true);
        portraitRight.gameObject.SetActive(false);
        portraitLeft.sprite = line.characterSprite;

        // Reset Awan & Posisi Kiri
        bubble.localScale = new Vector3(1, 1, 1);
        group.anchoredPosition = new Vector2(catPositionX, group.anchoredPosition.y);
        textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
    }

    void AddLine(string name, string text, Sprite bg, Sprite portrait, Sprite bubble = null, AudioClip sfx = null)
    {
        DialogueLine2 newLine = new DialogueLine2();
        newLine.characterName = name;
        newLine.dialogue = text;
        newLine.backgroundSprite = bg;
        newLine.characterSprite = portrait; 
        newLine.bubbleSprite = bubble; 
        newLine.soundEffect = sfx; // Simpan SFX
        lines.Add(newLine);
    }

    public bool IsFinished()
    {
        return isEnd;
    }
}