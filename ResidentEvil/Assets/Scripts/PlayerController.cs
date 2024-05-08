using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/*
 * Iversen-Krampitz, Ian 
 * 05/07/2024
 * Controls player movement and camera changing. 
 */

public class PlayerController : MonoBehaviour
{
    public delegate void MoveType();
    public MoveType moveType;
    public Controls controls;
    public float turnSpeed;
    public float walkSpeed;
    public float runSpeed;
    public bool canMove;
    public bool canQuickTurn;
    public bool isQuickTurning;
    public GameObject Camera;
    public GameObject tempCamera;
    public CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.Enable();
        moveType = Walk;
        canMove = true;
    }

    void FixedUpdate()
    {
        //only moves if not quickturning 
        if (!isQuickTurning)
        {
            if (controls.Movement.Run.IsPressed())
            {
                moveType = Run;
            }
            else
            {
                moveType = Walk;
            }
        }

        //quickturns 
        if (controls.Movement.QuickTurn.IsPressed() && canQuickTurn)
        {
            StartCoroutine(QuickTurnCooldown());
        }

        if (canMove || isQuickTurning)
        {
            moveType();
        }

        //checks if the player is above ground, snaps down to ground
        RaycastHit hit; 

        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            transform.Translate(Vector3.down * 3 * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CameraChange"))
        {
            //checks if this camera is already set, changes and does cooldown if not
            if (Camera.transform.position != other.transform.parent.position)
            {
                tempCamera = other.gameObject;
                StartCoroutine(CameraCooldown());
                print("changed camera.");
            }
            else
            {
                print("already using this camera.");
            }
        }
    }

    /// <summary>
    /// walk movement 
    /// </summary>
    private void Walk()
    {
        if (controls.Movement.TurnLeft.IsPressed())
        {
            transform.Rotate(Vector3.down * turnSpeed * Time.deltaTime);
            canQuickTurn = false;
        }
        if (controls.Movement.TurnRight.IsPressed())
        {
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            canQuickTurn = false;
        }
        if (controls.Movement.MoveForward.IsPressed())
        {
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
            canQuickTurn = false;
        }
        if (controls.Movement.MoveBackward.IsPressed())
        {
            transform.Translate(Vector3.back * walkSpeed * Time.deltaTime);
            canQuickTurn = false;
        }
        else
        {
            canQuickTurn = true;
        }
    }
    /// <summary>
    /// run movement 
    /// </summary>
    private void Run()
    {

        if (controls.Movement.TurnLeft.IsPressed())
        {
            transform.Rotate(Vector3.down * turnSpeed * Time.deltaTime);
            canQuickTurn = false;
        }
        if (controls.Movement.TurnRight.IsPressed())
        {
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            canQuickTurn = false;
        }
        if (controls.Movement.MoveForward.IsPressed())
        {
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
            canQuickTurn = false;
        }
        if (controls.Movement.MoveBackward.IsPressed())
        {
            transform.Translate(Vector3.back * walkSpeed * Time.deltaTime);
            canQuickTurn = false;
        }
        else
        {
            canQuickTurn = true;
        }
    }

    /// <summary>
    /// quickturn movement 
    /// </summary>
    private void QuickTurn()
    {
        //355 is the closest number i could find to make it a 180 degree turn 
            transform.Rotate(Vector3.up * 355 * Time.deltaTime);
    }

    /// <summary>
    /// cooldown for quickturning 
    /// </summary>
    /// <returns></returns>
    private IEnumerator QuickTurnCooldown()
    {
        moveType = QuickTurn;
        isQuickTurning = true;
        canQuickTurn = false;
        canMove = false;
        yield return new WaitForSeconds(.5f);
        isQuickTurning = false;
        canQuickTurn = true;
        canMove = true;
    }

    /// <summary>
    /// creates a delay between camera movement to emulate load times 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CameraCooldown()
    {
        canMove = false;
        yield return new WaitForSeconds(.2f);
        Camera.transform.SetPositionAndRotation(tempCamera.transform.parent.position, tempCamera.transform.parent.rotation);
        canMove = true;
    }
}
