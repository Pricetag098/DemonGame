using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
public class PlayerFootSteps : MonoBehaviour
{
    PlayerInputt playerInput;
    Rigidbody body;
    [SerializeField] float footStepDist;
    Vector3 lastFootstepPos;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInputt>();
        body = GetComponent<Rigidbody>();
        lastFootstepPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInput.touchingSurface && Vector3.Distance(lastFootstepPos, transform.position) > footStepDist)
		{
            SurfaceData surfaceData;
            if (playerInput.lastSurface.collider.TryGetComponent(out Surface s))
			{
                surfaceData = s.data;
			}
			else
			{
                surfaceData = VfxSpawner.DefaultSurfaceData;
			}
            if(playerInput.moveState == PlayerInputt.MoveStates.slide)
			{
                surfaceData.PlaySlideVfx(playerInput.lastSurface.point, body.velocity);
            }
			else
			{
                surfaceData.PlayStepVfx(playerInput.lastSurface.point, playerInput.lastSurface.normal);
            }
            lastFootstepPos = transform.position;
		}
    }
}
