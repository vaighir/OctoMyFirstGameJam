﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoController : MonoBehaviour
{
    public int lives;
    private float speed, swimSpeed, dashSpeed, dashDuration, lastDashedTime;
    private Rigidbody2D octoRigidbody;
    private Vector3 change;
    private Animator animator;
    public AudioSource hurt, collect;
    private bool dashing;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        octoRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        lastDashedTime = Time.time;
        swimSpeed = 10f;
        dashSpeed = 20f;
        speed = swimSpeed;
        dashDuration = 0.5f;
        dashing = false;
        lives = 3;

        GameObject game = GameObject.FindWithTag("GameController");
        gameController = game.GetComponent<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        ResetMovement();

        change.x = Input.GetAxis("Horizontal");
        change.y = Input.GetAxis("Vertical");

        Swim();
        Dash();
    }

    private void ResetMovement()
    {
        change = Vector3.zero;

        if (Time.time - lastDashedTime > dashDuration)
        {
            speed = swimSpeed;
            dashing = false;
            animator.SetBool("dashing", dashing);
        }
    }

    private void Swim()
    {
        
        if (change != Vector3.zero)
        {
            Move();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("swimming", true);
        }
        else
        {
            animator.SetBool("swimming", false);
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown("space") && !dashing)
        {
            speed = dashSpeed;
            dashing = true;
            animator.SetBool("dashing", dashing);
            lastDashedTime = Time.time;
        }
    }

    private void Move()
    {
        octoRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        hurt.Play();
        gameController.UpdateLives(-damage);
    }

    public void Eat(int value)
    {
        collect.Play();
        gameController.UpdateScore(value);
    }
}
