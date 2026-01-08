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
    public AudioClip soundEffect; 
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
    public Image portraitMiddle; // BARU: Gambar Tengah Atas

    [Header("Character Animators")] 
    public PortraitAnimator systemAnimator; 
    public PortraitAnimator bossAnimator; 

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
    public AudioClip machineTurnOnSfx; 
    public AudioClip doorOpenSfx;      
    public AudioClip explosionSfx;     
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
    public Sprite bossSprite; // BARU: Sprite Boss untuk ditampilkan

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
        if (dialogueGroup != null) 
            dialogueGroup.SetActive(false);
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

        // --- DATA DIALOG ---
        AddLine("", "'LAVA FADES AWAY'", BlackBG, null, noiseBubbleSprite, LavaSfx);

        AddLine("SYSTEM", "Huh.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "I’ve never seen this area like this.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "You’re the system.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "What do you mean, never?", digitalWorldBg, catSprite);

        AddLine("SYSTEM", "Before the corruption,", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "this place didn’t exist.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "You’re the first one here.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "Lucky me.", digitalWorldBg, catSprite);

        // (A LOUD RUMBLE...) -> Boss muncul di tengah (menggunakan bossSprite)
        AddLine("-", "(A LOUD RUMBLE. YELLOW BULLETS BEGIN TO FORM.)", digitalWorldBg, bossSprite, noiseBubbleSprite, machineTurnOnSfx);

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
                StopCharacterAnimation(); 
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
        PlayCharacterAnimation();

        foreach (char c in lines[index].dialogue.ToCharArray())
        {
            dialogueText.text += c;
            if (audioSource != null && typingSfx != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f); 
                audioSource.PlayOneShot(typingSfx);
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        
        isTyping = false;
        StopCharacterAnimation();
    }

    void PlayCharacterAnimation()
    {
        string currentChar = lines[index].characterName;
        
        if ((currentChar == "SYSTEM" || currentChar == "?") && systemAnimator != null)
        {
            systemAnimator.Play();
        }
        else if (currentChar == "BOSS" && bossAnimator != null)
        {
            bossAnimator.Play();
        }
    }

    void StopCharacterAnimation()
    {
        if (systemAnimator != null) systemAnimator.Stop();
        if (bossAnimator != null) bossAnimator.Stop(); 
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
        }
    }

    void UpdateUI()
    {
        DialogueLine3 currentLine = lines[index];
        RectTransform groupRect = dialogueGroup.GetComponent<RectTransform>();
        RectTransform bubbleRect = bubbleImage.GetComponent<RectTransform>();

        // Safety Reset
        if (systemAnimator != null && systemAnimator.gameObject != portraitRight.gameObject) 
            systemAnimator.gameObject.SetActive(false);
        
        if (bossAnimator != null && bossAnimator.gameObject != portraitRight.gameObject) 
            bossAnimator.gameObject.SetActive(false);

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

        if (currentLine.backgroundSprite != null)
            backgroundImage.sprite = currentLine.backgroundSprite;

        if (currentLine.bubbleSprite != null)
            bubbleImage.sprite = currentLine.bubbleSprite;
        else
            bubbleImage.sprite = normalBubbleSprite;

        if (currentLine.characterSprite != null)
        {
            // === LOGIKA POSISI ===
            if (currentLine.characterName == "SYSTEM" || currentLine.characterName == "?")
            {
                SetRightSide(currentLine, groupRect, bubbleRect);
            }
            else if (currentLine.characterName == "BOSS")
            {
                // Kalau Boss bicara, pakai logika Kanan (atau custom bossAnimator)
                if (bossAnimator != null && bossAnimator.gameObject != portraitRight.gameObject)
                {
                    portraitLeft.gameObject.SetActive(false);
                    portraitRight.gameObject.SetActive(false);
                    if (portraitMiddle != null) portraitMiddle.gameObject.SetActive(false); // Hide middle
                    
                    bossAnimator.gameObject.SetActive(true);

                    bubbleRect.localScale = new Vector3(-1, 1, 1);
                    groupRect.anchoredPosition = new Vector2(systemPositionX, groupRect.anchoredPosition.y);
                    float newX = -defaultTextX + textFlipOffset;
                    textContainer.anchoredPosition = new Vector2(newX, textContainer.anchoredPosition.y);
                }
                else
                {
                    SetRightSide(currentLine, groupRect, bubbleRect);
                }
            }
            else if (currentLine.characterName == "-")
            {
                // LOGIKA KHUSUS: Narasi tapi ada gambarnya -> Tengah Atas
                SetMiddleSide(currentLine, groupRect, bubbleRect);
            }
            else
            {
                SetLeftSide(currentLine, groupRect, bubbleRect);
            }
        }
        else
        {
            // Narasi Polos
            portraitLeft.gameObject.SetActive(false);
            portraitRight.gameObject.SetActive(false);
            if (portraitMiddle != null) portraitMiddle.gameObject.SetActive(false);

            bubbleRect.localScale = new Vector3(1, 1, 1);
            groupRect.anchoredPosition = new Vector2(centerPositionX, groupRect.anchoredPosition.y);
            textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
        }
    }

    // Fungsi helper baru untuk posisi Tengah
    void SetMiddleSide(DialogueLine3 line, RectTransform group, RectTransform bubble)
    {
        portraitLeft.gameObject.SetActive(false);
        portraitRight.gameObject.SetActive(false);
        
        if (portraitMiddle != null)
        {
            portraitMiddle.gameObject.SetActive(true);
            portraitMiddle.sprite = line.characterSprite;
        }

        bubble.localScale = new Vector3(1, 1, 1);
        group.anchoredPosition = new Vector2(centerPositionX, group.anchoredPosition.y);
        textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
    }

    void SetRightSide(DialogueLine3 line, RectTransform group, RectTransform bubble)
    {
        portraitLeft.gameObject.SetActive(false);
        if (portraitMiddle != null) portraitMiddle.gameObject.SetActive(false);
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
        if (portraitMiddle != null) portraitMiddle.gameObject.SetActive(false);
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