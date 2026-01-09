using UnityEngine;

public class Grappler : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer grappleLine;
    public DistanceJoint2D distanceJoint;
    PlayerStats playerStats;
    private float staminaDrainPerSecond = 0.1f;
    public bool isGrappling;
    void Awake(){
        playerStats = GetComponent<PlayerStats>();
    }
    void Start(){
        distanceJoint.enabled = false;
    }

    void Update(){
        if (Input.GetMouseButtonDown(1)){
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(1)){
            StopGrapple();
        }
        if (isGrappling){
            DrainStamina();
        }
        if (distanceJoint.enabled){
            grappleLine.SetPosition(1, transform.position);
        }
    }

    private void StartGrapple()
    {
        if (!playerStats.HasStamina(staminaDrainPerSecond)){
            return;
        }
        Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        grappleLine.SetPosition(0, mousePos);
        grappleLine.SetPosition(1, transform.position);
        distanceJoint.connectedAnchor = mousePos;
        distanceJoint.enabled = true;
        grappleLine.enabled = true;
        isGrappling = true;
        playerStats.CanRegenerate = false;
    }
    private void StopGrapple()
    {
        distanceJoint.enabled = false;
        grappleLine.enabled = false;
        isGrappling = false;
        playerStats.CanRegenerate = true;
    }

    private void DrainStamina()
    {
        playerStats.UseStamina(staminaDrainPerSecond * Time.deltaTime * 30f);
        if (playerStats.stamina <= staminaDrainPerSecond)
        {
        StopGrapple();
        }
    }
}
