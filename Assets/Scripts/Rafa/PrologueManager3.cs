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
    public AudioClip soundEffect; // BARU: Slot untuk suara khusus per dialog
}

public class PrologueManager3 : MonoBehaviour
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
    
    // BARU: Tempat naruh file suara kejadian khusus
    [Header("Story Assets - SFX")] 
    public AudioClip machineTurnOnSfx; // Drag suara mesin nyala
    public AudioClip doorOpenSfx;      // Drag suara pintu
    public AudioClip explosionSfx;     // Drag suara ledakan (contoh)

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

        // --- DATA DIALOG PROLOGUE 3 ---
        
        // CONTOH PENGGUNAAN:
        // Parameter terakhir adalah Sound Effect.
        
        AddLine("SYSTEM", "We are close to the Core. Prepare yourself.", digitalWorldBg, systemSprite);
        
        // Contoh: Kucing bicara sambil ada suara pintu terbuka (pakai doorOpenSfx)
        AddLine("THE CAT", "I was born ready. And hungry.", digitalWorldBg, catSprite, null, doorOpenSfx);
        
        // Contoh: System bicara sambil ada suara mesin keras (pakai machineTurnOnSfx)
        // Dan pakai bubble noise
        AddLine("SYSTEM", "Focus! The main virus is guarding the entrance.", digitalWorldBg, systemSprite, noiseBubbleSprite, machineTurnOnSfx);
        
        AddLine("THE CAT", "Let's scratch some pixels then.", digitalWorldBg, catSprite);

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
            
            // Suara ketikan (Typing SFX)
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
            Debug.Log("Prologue 3 Selesai! Pindah ke Gameplay Utama.");
            // SceneManager.LoadScene("Level1");
        }
    }

    void UpdateUI()
    {
        DialogueLine3 currentLine = lines[index];
        RectTransform groupRect = dialogueGroup.GetComponent<RectTransform>();
        RectTransform bubbleRect = bubbleImage.GetComponent<RectTransform>();

        // --- BARU: Mainkan Suara Spesifik (Jika Ada) ---
        if (currentLine.soundEffect != null && audioSource != null)
        {
            // Reset pitch ke normal untuk SFX kejadian
            audioSource.pitch = 1f; 
            audioSource.PlayOneShot(currentLine.soundEffect);
        }
        // -----------------------------------------------

        nameText.text = currentLine.characterName;
        nameText.color = (currentLine.characterName == "THE CAT") ? Color.yellow : Color.cyan;

        if (currentLine.backgroundSprite != null)
            backgroundImage.sprite = currentLine.backgroundSprite;

        if (currentLine.bubbleSprite != null)
            bubbleImage.sprite = currentLine.bubbleSprite;
        else
            bubbleImage.sprite = normalBubbleSprite;

        if (currentLine.characterSprite != null)
        {
            if (currentLine.characterName == "SYSTEM")
            {
                portraitLeft.gameObject.SetActive(false);
                portraitRight.gameObject.SetActive(true);
                portraitRight.sprite = currentLine.characterSprite;

                bubbleRect.localScale = new Vector3(-1, 1, 1);
                groupRect.anchoredPosition = new Vector2(systemPositionX, groupRect.anchoredPosition.y);
                float newX = -defaultTextX + textFlipOffset;
                textContainer.anchoredPosition = new Vector2(newX, textContainer.anchoredPosition.y);
            }
            else
            {
                portraitLeft.gameObject.SetActive(true);
                portraitRight.gameObject.SetActive(false);
                portraitLeft.sprite = currentLine.characterSprite;

                bubbleRect.localScale = new Vector3(1, 1, 1);
                groupRect.anchoredPosition = new Vector2(catPositionX, groupRect.anchoredPosition.y);
                textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
            }
        }
        else
        {
            portraitLeft.gameObject.SetActive(false);
            portraitRight.gameObject.SetActive(false);
            bubbleRect.localScale = new Vector3(1, 1, 1);
            groupRect.anchoredPosition = new Vector2(centerPositionX, groupRect.anchoredPosition.y);
            textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
        }
    }

    // Update fungsi AddLine: Tambah parameter sfx di paling belakang
    void AddLine(string name, string text, Sprite bg, Sprite portrait, Sprite bubble = null, AudioClip sfx = null)
    {
        DialogueLine3 newLine = new DialogueLine3();
        newLine.characterName = name;
        newLine.dialogue = text;
        newLine.backgroundSprite = bg;
        newLine.characterSprite = portrait; 
        newLine.bubbleSprite = bubble; 
        newLine.soundEffect = sfx; // Simpan SFX
        lines.Add(newLine);
    }
}