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
        if (controls.Movement.Run.IsPressed())
        {
            moveType = Run;
        }
        else
        {
            moveType = Walk;
        }

        if (controls.Movement.QuickTurn.IsPressed() && canQuickTurn)
        {
            canMove = false;
            StartCoroutine(QuickTurn());
        }

        if (canMove)
        {
            moveType();
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

    private IEnumerator QuickTurn()
    {
        transform.Rotate(Vector3.up * turnSpeed * 5 * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        canQuickTurn = true;
        canMove = true;
    }
}
