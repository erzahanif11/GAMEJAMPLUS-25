using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueLine1
{
    public string characterName;
    [TextArea(3, 10)] 
    public string dialogue;
    public Sprite backgroundSprite; 
    public Sprite characterSprite; 
    public Sprite bubbleSprite; 
    public AudioClip soundEffect; // Slot audio per baris
}

public class PrologueManager1 : MonoBehaviour
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

    private List<DialogueLine1> lines = new List<DialogueLine1>();
    private int index;
    private bool isTyping;

    void Start()
    {
        if (textContainer != null)
        {
            defaultTextX = textContainer.anchoredPosition.x;
        }

        if (normalBubbleSprite == null && bubbleImage != null)
            normalBubbleSprite = bubbleImage.sprite;

        // Ambil AudioSource otomatis jika belum di-assign
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // --- DATA DIALOG ---
        AddLine("-", "'The sound of the door being opened'", BlackBG, null, noiseBubbleSprite, doorOpenSfx);
        AddLine("-", "'Heavy rain pouring outside'", BlackBG, null, noiseBubbleSprite, rainSfx); // Added text for rain context
        
        AddLine("THE CAT", "Damn, sky is crying buckets out there. I ain't dealing with wet fur today.", BlackBG, catSprite, null, null);
        AddLine("THE CAT", "At least it's dry in here.", BlackBG, catSprite);
        AddLine("THE CAT", "Smells like dust and... old plastic?", BlackBG, catSprite);
        AddLine("THE CAT", "Whoa. Check this out. It’s like a graveyard in here.", warehouseBg, catSprite);
        AddLine("THE CAT", "Tetris... Tetris 2... Super Tetris... Man, someone was really obsessed with stacking blocks.", warehouseBg, catSprite);
        AddLine("THE CAT", "And what do we have here?", warehouseBg, catSprite);
        AddLine("THE CAT", "\"Tetris Hell\"? Heh. Edgy name.", warehouseBg, catSprite);
        AddLine("THE CAT", "You know what? I'm bored. Let's see if this bad boy still works.", warehouseBg, catSprite);

        // Mesin nyala
        AddLine("SYSTEM", "*CLICK* [Machine turns on violently]", BlackBG, null, noiseBubbleSprite, machineTurnOnSfx);
        
        AddLine("THE CAT", "Yeah! That’s what I’m talking about!", warehouseBg, catSprite, null, null);
        AddLine("THE CAT", "...", warehouseBg, catSprite);
        AddLine("THE CAT", "Wait...", warehouseBg, catSprite);
        AddLine("THE CAT", "Why is the screen swirling like that?", Hypno, catSprite, null, hypnoSfx);
        AddLine("THE CAT", "Whoa! Hey! Back off! Let go of my tail!", Hypno, catSprite, null, hypnoSfx);
        AddLine("THE CAT", "Oh crap, oh crap, OH CRAAAAAP—!", Hypno, catSprite);
        AddLine("-", "'-'", BlackBG, null, null, null);

        // Scene 2: Digital World
        AddLine("THE CAT", "Ugh...", digitalWorldBg, catSprite, null, null);
        AddLine("THE CAT", "Note to myself never play around in abandoned warehouses AGAIN.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "F*ck… where the hell am I?", digitalWorldBg, catSprite);
        AddLine("THE CAT", "(Looks down)", digitalWorldBg, catSprite,null, LavaSfx);
        AddLine("THE CAT", "Oh my.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "This place looks really unfriendly. One slip and I’m Cooked", digitalWorldBg, catSprite);
        
        AddLine("-", "'A Anonymous Entity appears'", digitalWorldBg, null, noiseBubbleSprite, RoboSfx);
        
        AddLine("?", "HEY! Hold it right there!", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Jeez!", digitalWorldBg, catSprite);
        AddLine("THE CAT", "You always pop in like that or is today special?", digitalWorldBg, catSprite);
        
        // Karakter "?" muncul di kanan (sama seperti System)
        AddLine("?", "Whoa, whoa. Easy", digitalWorldBg, systemSprite);
        AddLine("?", "Didn’t mean to scare you.", digitalWorldBg, systemSprite);
        
        AddLine("SYSTEM", "I am Administrator of this place but just call me System", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "SYSTEM, huh.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "Sounds official.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "So… you gonna tell me why your thing just swallowed me here ?", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Uh… yeah.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "That’s not supposed to happen.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Funny.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "That’s exactly what everyone says when things go wrong.", digitalWorldBg, catSprite);
        
        AddLine("SYSTEM", "'Scans'", digitalWorldBg, systemSprite, noiseBubbleSprite, ScanSfx);
        
        AddLine("SYSTEM", "Wait… ", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "You’re biological? You’re a CAT?", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Last I checked it. What, I’m not your usual pickup thing?", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Yeah but not a living thing.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Lucky me. Alright, TV HEAD. I’m leaving this shitty hole", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Yeah, about that", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "You can’t just leave.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Oh? And why not?", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Because… you’re in the Digital World now.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Digital World?", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "This is Tetris Hell. See that lava?", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "One wrong move and you’re toast.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Right Cool. Gotta Love places with instant death", digitalWorldBg, catSprite);
        AddLine("THE CAT", "So, how do I get out of here?", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "Listen there is one way out. The Core", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "But it’s heavily guarded by a nasty virus.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "I can guide you.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Hmm. Sounds risky.", digitalWorldBg, catSprite);
        AddLine("SYSTEM", "You can use your claws to attack the virus. You are not bound by our rules.", digitalWorldBg, systemSprite);
        AddLine("THE CAT", "Well, when you put it that way...", digitalWorldBg, catSprite);
        AddLine("THE CAT", "Alright System, you got a deal. Let’s get me out of this Tetris hellhole.", digitalWorldBg, catSprite);

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
            Debug.Log("Prologue 1 Selesai! Pindah Scene.");
            // SceneManager.LoadScene("Level1");
        }
    }

    void UpdateUI()
    {
        DialogueLine1 currentLine = lines[index];
        RectTransform groupRect = dialogueGroup.GetComponent<RectTransform>();
        RectTransform bubbleRect = bubbleImage.GetComponent<RectTransform>();

        // Mainkan Suara Spesifik (Jika Ada)
        if (currentLine.soundEffect != null && audioSource != null)
        {
            audioSource.pitch = 1f; 
            audioSource.PlayOneShot(currentLine.soundEffect);
        }

        nameText.text = currentLine.characterName;
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

        // --- PERBAIKAN LOGIKA IF ELSE DI SINI ---
        if (currentLine.characterSprite != null)
        {
            // === ADA KARAKTER YANG BICARA ===

            // Jika SYSTEM bicara
            if (currentLine.characterName == "SYSTEM")
            {
                // === SYSTEM (KANAN) ===
                SetRightSide(currentLine, groupRect, bubbleRect);
            }
            // Jika karakter misterius "?" bicara (Perlakuan sama kayak SYSTEM)
            else if (currentLine.characterName == "?")
            {
                // === ? (KANAN) ===
                SetRightSide(currentLine, groupRect, bubbleRect);
            }
            // Jika THE CAT bicara
            else
            {
                // === THE CAT (KIRI) ===
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

    // Fungsi helper biar kode UpdateUI lebih rapi dan tidak duplikat
    void SetRightSide(DialogueLine1 line, RectTransform group, RectTransform bubble)
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

    void SetLeftSide(DialogueLine1 line, RectTransform group, RectTransform bubble)
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
        DialogueLine1 newLine = new DialogueLine1();
        newLine.characterName = name;
        newLine.dialogue = text;
        newLine.backgroundSprite = bg;
        newLine.characterSprite = portrait; 
        newLine.bubbleSprite = bubble; 
        newLine.soundEffect = sfx; 
        lines.Add(newLine);
    }
}