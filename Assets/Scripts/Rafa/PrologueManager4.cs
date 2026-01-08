using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueLine4
{
    public string characterName;
    [TextArea(3, 10)] 
    public string dialogue;
    public Sprite backgroundSprite; 
    public Sprite characterSprite; 
    public Sprite bubbleSprite; 
    public AudioClip soundEffect;
    public Color textColor = Color.black; // UBAH: Default jadi Hitam
}

public class PrologueManager4 : MonoBehaviour
{
    public static PrologueManager4 instance;

    [Header("UI Components - Text")]
    public TextMeshProUGUI nameText;      
    public TextMeshProUGUI dialogueText;  
    
    [Header("UI Components - Images")]
    public Image backgroundImage;         
    public Image portraitLeft;   
    public Image portraitRight;  

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
    [Tooltip("Geser teks KHUSUS saat System/Boss bicara. Isi angka Negatif (-) untuk geser ke Kiri.")]
    public float textFlipOffset = 0f; 
    
    private float defaultTextX; 

    [Header("Typing Settings")]
    public float typingSpeed = 0.04f;     

    [Header("Audio Settings")] 
    public AudioSource audioSource; 
    public AudioClip typingSfx;     

    [Header("Story Assets - SFX")] 
    public AudioClip collapseSfx;
    public AudioClip winSfx;
    public AudioClip glitchSfx;
    public AudioClip doorOpenSfx;
    public AudioClip machineTurnOnSfx;

    [Header("Story Assets - Backgrounds")]
    public Sprite warehouseBg;            
    public Sprite digitalWorldBg;
    public Sprite BlackBG;       
    public Sprite Hypno;  

    [Header("Story Assets - Characters")] 
    public Sprite catSprite;
    public Sprite systemSprite;
    public Sprite bossSprite;

    [Header("Story Assets - Bubbles")] 
    public Sprite normalBubbleSprite; 
    public Sprite noiseBubbleSprite;  

    private List<DialogueLine4> lines = new List<DialogueLine4>();
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

        // --- DATA DIALOG EPILOGUE ---

        AddLine("", "The boss collapses. The arena goes silent.", BlackBG, null, noiseBubbleSprite, collapseSfx);

        AddLine("SYSTEM", "Target neutralized.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "Nice work.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "…Finally.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "So we’re done?", digitalWorldBg, catSprite);

        AddLine("SYSTEM", "Yes.", digitalWorldBg, systemSprite);

        AddLine("SYSTEM", "CONGRATULATION !! You completed the tutorial.", digitalWorldBg, systemSprite, null, winSfx);

        AddLine("THE CAT", "Wait. what ?!", digitalWorldBg, catSprite);

        AddLine("SYSTEM", "This was only the beginning.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "The real core lies beyond this area.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "You’re kidding me.", digitalWorldBg, catSprite);

        AddLine("-", "(A faint glitch. The boss stirs.)", digitalWorldBg, null, noiseBubbleSprite, glitchSfx);

        AddLine("BOSS", "…Thank you.", digitalWorldBg, bossSprite,noiseBubbleSprite);

        AddLine("THE CAT", "What?", digitalWorldBg, catSprite);

        AddLine("BOSS", "The corruption is gone.", digitalWorldBg, bossSprite, noiseBubbleSprite);
        AddLine("BOSS", "You freed me.", digitalWorldBg, bossSprite, noiseBubbleSprite);

        AddLine("SYSTEM", "How this is Impossible.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "You were classified as hostile.", digitalWorldBg, systemSprite);

        AddLine("BOSS", "Because I was trapped by something beyond this place.", digitalWorldBg, bossSprite, noiseBubbleSprite);
        AddLine("BOSS", "Not because I was evil.", digitalWorldBg, bossSprite, noiseBubbleSprite);

        AddLine("THE CAT", "…Great.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "I punched the wrong guy.", digitalWorldBg, catSprite);

        AddLine("BOSS", "There are others like me.", digitalWorldBg, bossSprite, noiseBubbleSprite);
        AddLine("BOSS", "Still corrupted.", digitalWorldBg, bossSprite, noiseBubbleSprite);
        AddLine("BOSS", "They’ll need help.", digitalWorldBg, bossSprite, noiseBubbleSprite);

        AddLine("SYSTEM", "This exceeds expected parameters.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "You knew this was just a tutorial", digitalWorldBg, catSprite);
        AddLine("THE CAT", "and didn’t think to mention it?", digitalWorldBg, catSprite);

        AddLine("SYSTEM", "It seemed… unnecessary.", digitalWorldBg, systemSprite);
        AddLine("SYSTEM", "And honestly, I didn’t expect you to survive.", digitalWorldBg, systemSprite);

        AddLine("THE CAT", "You f*cking Tv Head.", digitalWorldBg, catSprite);

        AddLine("THE CAT", "Alright.", digitalWorldBg, catSprite);
        AddLine("THE CAT", "One more round.", digitalWorldBg, catSprite);

        AddLine("THE CAT", "Here we go again.", digitalWorldBg, catSprite);

        AddLine("SYSTEM", "Preparing next sequence.", digitalWorldBg, systemSprite, noiseBubbleSprite);

        AddLine("", "TO BE CONTINUED", BlackBG, null, noiseBubbleSprite, machineTurnOnSfx, Color.red);

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
        
        if (currentChar == "SYSTEM" && systemAnimator != null)
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
            Debug.Log("EPILOGUE SELESAI! Terima kasih sudah bermain.");
            isEnd = true;
        }
    }

    void UpdateUI()
    {
        DialogueLine4 currentLine = lines[index];
        RectTransform groupRect = dialogueGroup.GetComponent<RectTransform>();
        RectTransform bubbleRect = bubbleImage.GetComponent<RectTransform>();

        // 1. Matikan semua animator dulu
        if (systemAnimator != null && systemAnimator.gameObject != portraitRight.gameObject) 
            systemAnimator.gameObject.SetActive(false);
        if (bossAnimator != null && bossAnimator.gameObject != portraitRight.gameObject) 
            bossAnimator.gameObject.SetActive(false);

        // 2. Play Audio
        if (currentLine.soundEffect != null && audioSource != null)
        {
            audioSource.pitch = 1f; 
            audioSource.PlayOneShot(currentLine.soundEffect);
        }

        // 3. Set Teks & Warna Nama
        nameText.text = currentLine.characterName;
        if (currentLine.characterName == "THE CAT")
            nameText.color = Color.yellow;
        else
            nameText.color = Color.cyan;

        // 4. Set Warna Teks Dialog (Hitam default, atau Merah kalau diset)
        dialogueText.color = currentLine.textColor;

        // 5. Set Background & Bubble
        if (currentLine.backgroundSprite != null)
            backgroundImage.sprite = currentLine.backgroundSprite;

        if (currentLine.bubbleSprite != null)
            bubbleImage.sprite = currentLine.bubbleSprite;
        else
            bubbleImage.sprite = normalBubbleSprite;

        // 6. Logika Karakter & Posisi
        if (currentLine.characterSprite != null)
        {
            if (currentLine.characterName == "SYSTEM")
            {
                if (systemAnimator != null && systemAnimator.gameObject != portraitRight.gameObject)
                {
                    portraitLeft.gameObject.SetActive(false);
                    portraitRight.gameObject.SetActive(false); 
                    systemAnimator.gameObject.SetActive(true); 

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
            else if (currentLine.characterName == "BOSS")
            {
                if (bossAnimator != null && bossAnimator.gameObject != portraitRight.gameObject)
                {
                    portraitLeft.gameObject.SetActive(false);
                    portraitRight.gameObject.SetActive(false);
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
            else
            {
                SetLeftSide(currentLine, groupRect, bubbleRect);
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

    void SetRightSide(DialogueLine4 line, RectTransform group, RectTransform bubble)
    {
        portraitLeft.gameObject.SetActive(false);
        portraitRight.gameObject.SetActive(true);
        portraitRight.sprite = line.characterSprite;

        bubble.localScale = new Vector3(-1, 1, 1);
        group.anchoredPosition = new Vector2(systemPositionX, group.anchoredPosition.y);
        
        float newX = -defaultTextX + textFlipOffset;
        textContainer.anchoredPosition = new Vector2(newX, textContainer.anchoredPosition.y);
    }

    void SetLeftSide(DialogueLine4 line, RectTransform group, RectTransform bubble)
    {
        portraitLeft.gameObject.SetActive(true);
        portraitRight.gameObject.SetActive(false);
        portraitLeft.sprite = line.characterSprite;

        bubble.localScale = new Vector3(1, 1, 1);
        group.anchoredPosition = new Vector2(catPositionX, group.anchoredPosition.y);
        textContainer.anchoredPosition = new Vector2(defaultTextX, textContainer.anchoredPosition.y);
    }

    void AddLine(string name, string text, Sprite bg, Sprite portrait, Sprite bubble = null, AudioClip sfx = null, Color? txtColor = null)
    {
        DialogueLine4 newLine = new DialogueLine4();
        newLine.characterName = name;
        newLine.dialogue = text;
        newLine.backgroundSprite = bg;
        newLine.characterSprite = portrait; 
        newLine.bubbleSprite = bubble; 
        newLine.soundEffect = sfx; 
        
        // UBAH: Jika warna tidak diisi (null), pakai Hitam.
        newLine.textColor = txtColor ?? Color.black; 

        lines.Add(newLine);
    }

    public bool IsFinished()
    {
        return isEnd;
    }
}