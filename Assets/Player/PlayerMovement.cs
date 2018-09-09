using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 clickToDestination, clickPoint;

    [SerializeField] float walkMoveStopRadius = 0.5f;
    [SerializeField] float attackMoveStopRadius = 3f;

    bool isIndirectMode = false;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        clickToDestination = transform.position;
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))//G for gamepad, Todo add to menu later
        {
            isIndirectMode = !isIndirectMode; //toggle mode
            clickToDestination = transform.position;
        }

        if (isIndirectMode)
        {
            ProcessDirectMovement();
        }

        else
        {
            ProcessMouseMovement();  
        }    
    }

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool jump = Input.GetButtonDown("Jump");
        bool crouch = Input.GetKey(KeyCode.C); // TODO confirm later if it has bug problem. if it has, go for standard asset script in evernote.

        //calculate camera relative direction to move
        Vector3 cameraForward= Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, crouch, jump);

    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    clickToDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    clickToDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    break;
                default:
                    print("Unexpected layer found");
                    return;
            }
        }
        WalkToDestination();
        OnDrawGizmos();
    }

    private void WalkToDestination()
    {
        var playerToClickPoint = clickToDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    private Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    private void OnDrawGizmos()
    {   //Drwa movement gizmo
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, clickToDestination);
        Gizmos.DrawSphere(clickToDestination, 0.2f);
        Gizmos.DrawSphere(clickPoint, 0.1f);

        //Draw attack sphere 
        Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}

