using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueLine3
{
    public string characterName;
    [TextArea(3, 10)] 
    public string dialogue;
    public Sprite backgroundSprite; 
    public Sprite characterSprite; 
    public Sprite bubbleSprite; 
    public AudioClip soundEffect; // Slot audio per baris
}

public class PrologueManager3 : MonoBehaviour
{
    public static PrologueManager3 instance;

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
    public AudioClip machineTurnOnSfx; // Bisa dipakai untuk Rumble
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

    private List<DialogueLine3> lines = new List<DialogueLine3>();
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

        // --- DATA DIALOG PROLOGUE 3 (BOSS INTRO) ---
        
        // (LAVA FADES AWAY) - Narasi/SFX
        AddLine("", "'LAVA FADES AWAY'", BlackBG, null, noiseBubbleSprite, LavaSfx);

        AddLine("SYSTEM", "Huh.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "I’ve never seen this area like this.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "You’re the system.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "What do you mean, never?", digitalWorldBg, catSprite);

        AddLine("SYSTEM", "Before the corruption,", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "this place didn’t exist.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "You’re the first one here.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "Lucky me.", digitalWorldBg, catSprite);

        // (A LOUD RUMBLE. YELLOW BULLETS BEGIN TO FORM.)
        // Menggunakan noiseBubbleSprite untuk efek tegang dan machineTurnOnSfx untuk suara gemuruh
        AddLine("-", "(A LOUD RUMBLE. YELLOW BULLETS BEGIN TO FORM.)", digitalWorldBg, null, noiseBubbleSprite, machineTurnOnSfx);

        AddLine("THE CAT", "…", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "…", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "So.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "That’s the boss.", digitalWorldBg, catSprite);

        AddLine("SYSTEM", "Yes.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "It’s not moving.", digitalWorldBg, catSprite);

        AddLine("SYSTEM", "It doesn’t need to.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "Alright here we go !!", digitalWorldBg, catSprite);

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
            Debug.Log("Prologue 3 Selesai! START BOSS FIGHT!");
            isEnd = true;
            // SceneManager.LoadScene("BossLevel");
        }
    }

    void UpdateUI()
    {
        DialogueLine3 currentLine = lines[index];
        RectTransform groupRect = dialogueGroup.GetComponent<RectTransform>();
        RectTransform bubbleRect = bubbleImage.GetComponent<RectTransform>();

        // Mainkan Suara Spesifik
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

        // Set Bentuk Awan
        if (currentLine.bubbleSprite != null)
            bubbleImage.sprite = currentLine.bubbleSprite;
        else
            bubbleImage.sprite = normalBubbleSprite;

        // --- LOGIKA POSISI & GAMBAR KARAKTER ---
        if (currentLine.characterSprite != null)
        {
            // === ADA KARAKTER YANG BICARA ===

            if (currentLine.characterName == "SYSTEM")
            {
                SetRightSide(currentLine, groupRect, bubbleRect);
            }
            else if (currentLine.characterName == "?")
            {
                SetRightSide(currentLine, groupRect, bubbleRect);
            }
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

            bubbleRect.localScale = new Vector3(1, 1, 1);
            groupRect.anchoredPosition = new Vector2(centerPositionX, groupRect.anchoredPosition.y);
            textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
        }
    }

    // Fungsi helper
    void SetRightSide(DialogueLine3 line, RectTransform group, RectTransform bubble)
    {
        portraitLeft.gameObject.SetActive(false);
        portraitRight.gameObject.SetActive(true);
        portraitRight.sprite = line.characterSprite;

        bubble.localScale = new Vector3(-1, 1, 1);
        group.anchoredPosition = new Vector2(systemPositionX, group.anchoredPosition.y);
        
        float newX = -defaultTextX + textFlipOffset;
        textContainer.anchoredPosition = new Vector2(newX, textContainer.anchoredPosition.y);
    }

    void SetLeftSide(DialogueLine3 line, RectTransform group, RectTransform bubble)
    {
        portraitLeft.gameObject.SetActive(true);
        portraitRight.gameObject.SetActive(false);
        portraitLeft.sprite = line.characterSprite;

        bubble.localScale = new Vector3(1, 1, 1);
        group.anchoredPosition = new Vector2(catPositionX, group.anchoredPosition.y);
        textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
    }

    void AddLine(string name, string text, Sprite bg, Sprite portrait, Sprite bubble = null, AudioClip sfx = null)
    {
        DialogueLine3 newLine = new DialogueLine3();
        newLine.characterName = name;
        newLine.dialogue = text;
        newLine.backgroundSprite = bg;
        newLine.characterSprite = portrait; 
        newLine.bubbleSprite = bubble; 
        newLine.soundEffect = sfx; 
        lines.Add(newLine);
    }

    public bool IsFinished()
    {
        return isEnd;
    }
}