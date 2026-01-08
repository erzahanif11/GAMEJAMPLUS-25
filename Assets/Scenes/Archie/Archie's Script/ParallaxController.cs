using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [Header("Camera Reference")]
    [SerializeField] Transform cam;

    [Header("Parallax Layers")]
    [SerializeField] ParallaxLayer[] layers;

    private Vector3 lastCamPos;

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        lastCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCamPos;

        foreach (ParallaxLayer layer in layers)
        {
            layer.layerTransform.position += 
                new Vector3(deltaMovement.x * layer.parallaxSpeed, 0f, 0f);

            HandleSeamlessLoop(layer.layerTransform);
        }

        lastCamPos = cam.position;
    }

    void HandleSeamlessLoop(Transform layer)
    {
        Transform first = layer.GetChild(0);
        Transform second = layer.GetChild(1);

        float width = first.GetComponent<SpriteRenderer>().bounds.size.x;

        if (cam.position.x - first.position.x >= width)
        {
            first.position += Vector3.right * width * 2f;
            SwapChildren(layer);
        }
    }

    void SwapChildren(Transform layer)
    {
        Transform first = layer.GetChild(0);
        first.SetAsLastSibling();
    }
}
