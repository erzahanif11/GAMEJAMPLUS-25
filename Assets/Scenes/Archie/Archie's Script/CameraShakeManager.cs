using UnityEngine;
using Unity.Cinemachine;
public class CameraShakeManager : MonoBehaviour
{
    [SerializeField] float shakeForce;
    public static CameraShakeManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
    }
}
