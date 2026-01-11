using UnityEngine;
using Unity.Cinemachine;

public class HellBulletShake : MonoBehaviour
{
    CinemachineImpulseSource impulseSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CameraShakeManager.instance.CameraShake(impulseSource);
            AudioManager.instance.PlaySFX(AudioManager.instance.attacked);
        }
    }
}
