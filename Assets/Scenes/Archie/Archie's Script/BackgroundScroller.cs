using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed;
    Renderer bgRenderer;

    void Awake()
    {
        bgRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime,0);
    }
}