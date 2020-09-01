using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAdapter : MonoBehaviour
{
    Vector3 storedPlatformPosition;
    Vector3 platformVelocity;
    [SerializeField] GameObject inertiaItemPrefab = null;
    GameObject inertiaItemInstance;
    Vector3 inertiaSpeed;
    PlayerVertMovement vertMovement;

    void FixedUpdate()
    {
        CheckForPlatform();

        if (transform.parent != null && vertMovement.onGround()) //If parent is a moving platform
        {
            GetPlatformSpeed();
        }

        if (vertMovement.onGround() || Time.timeScale < 1) //If any slow motion effect happens OR hits ground
        {
            KillInertiaItem();
        }

        if (inertiaItemInstance != null) { inertiaItemInstance.transform.position += inertiaSpeed; }
    }

    void CheckForPlatform()
    {
        Vector3[] positionsToStartRay = { new Vector3(-0.5f, 0, -0.5f), new Vector3(0, 0, -0.5f), new Vector3(0.5f, 0, -0.5f), new Vector3(-0.5f, 0, 0), new Vector3(0.5f, 0, 0), new Vector3(-0.5f, 0, 0.5f), new Vector3(0, 0, 0.5f), new Vector3(0.5f, 0, 0.5f) };

        for (int i = 0; i < positionsToStartRay.Length; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + positionsToStartRay[i], Vector3.down, out hit, 1.05f))
            {
                if (hit.collider.CompareTag("Moving Platform"))
                {
                    transform.parent = hit.transform;
                    return;
                }
            }
        }

        DoAirStuff();
    }

    void DoAirStuff()
    {
        if (transform.parent != null && inertiaItemInstance == null) //If first frame off platform
        {
            inertiaItemInstance = Instantiate(inertiaItemPrefab);
            inertiaSpeed = platformVelocity;
        }

        if (inertiaItemInstance != null) transform.parent = inertiaItemInstance.transform;
        else transform.parent = null;

        storedPlatformPosition = default;
    }

    void KillInertiaItem()
    {
        if (inertiaItemInstance != null)
        {
            transform.parent = null;
            Destroy(inertiaItemInstance);
            inertiaItemInstance = null;
            inertiaSpeed = Vector3.zero;
        }
    }

    void GetPlatformSpeed()
    {
        if (storedPlatformPosition != default)
        {
            platformVelocity = transform.parent.position - storedPlatformPosition;
        }

        storedPlatformPosition = transform.parent.position;
    }

    void Start()
    {
        vertMovement = GetComponent<PlayerVertMovement>();
    }
}
