
using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour
{

    // Sprint 1 Goal: Make basic movement and find best attacking method (game feel) 
    // make the sword rotate the direction the player is moving
    public float speed;
    public GameObject sword; //modular sword system for enemies(?)

    private Boolean isSwinging;
    private Quaternion startingSwordRotation;
    private Vector3 startingSwordPosition;
    private Vector2 movement;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sword.SetActive(false);

    }

    void FixedUpdate()
    {
        if (isSwinging)
        {
            sword.transform.RotateAround(transform.localPosition, Vector3.forward, 10f);
            Debug.Log(sword.transform.rotation.z);
            if (sword.transform.rotation.z > 0.75f)
            {
                isSwinging = false;
                sword.SetActive(false);
                sword.transform.localRotation = startingSwordRotation;
                sword.transform.localPosition = startingSwordPosition;
            }
        } else
        {
            //rotate sword where player is facing
        }
    }

    void OnMove(InputValue value)
    {
            movement = value.Get<Vector2>();
            rb.linearVelocity = movement * speed;
    }

    void OnSwing()
    {
        if (!isSwinging)
        {
            startingSwordRotation = sword.transform.localRotation;
            startingSwordPosition = sword.transform.localPosition;
            Debug.Log("player swings!");
            isSwinging = true;
            sword.SetActive(true);
        }
    }
    
}
