using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/*
 * Iversen-Krampitz, Ian 
 * 04/19/2024
 * Controls player movement. 
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

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.Enable();
        moveType = Walk;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (controls.Movement.QuickTurn.IsPressed() && canQuickTurn)
        {
            StartCoroutine(QuickTurnCooldown());
        }

        if (canMove || isQuickTurning)
        {
            moveType();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CameraChange"))
        {
            Camera.transform.position = other.transform.parent.position;
            Camera.transform.rotation = other.transform.parent.rotation;
            print("changed camera.");
        }
    }

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

    private void QuickTurn()
    {
            transform.Rotate(Vector3.up * turnSpeed * 4 * Time.deltaTime);
    }

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
}
