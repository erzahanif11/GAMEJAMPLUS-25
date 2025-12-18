using UnityEngine;

public class BlockSoundFlag : MonoBehaviour
{
    [HideInInspector]
    public bool hasPlayedSound = false;
    void OnEnable()
    {
        hasPlayedSound = false;
    }
}
