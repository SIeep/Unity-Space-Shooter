﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 11.2f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.75f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private SpawnManager _spawnManager;


    void Start()
    {
        transform.position = new Vector3(-1.5f, -3, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is NULL.");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 20)
        {
            transform.position = new Vector3(-20, transform.position.y, 0);
        }
        else if (transform.position.x < -20)
        {
            transform.position = new Vector3(20, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        Debug.Log("Space Key Pressed on Player");
        _canFire = Time.time + _fireRate;
        
        

        //if space key press,

        //if tripleshotActive is true
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.08f, 0), Quaternion.identity);
        }

    }

    public void TripleShotBool()
    {
        _isTripleShotActive = true;
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            //Communicate with Spawn Manager
            _spawnManager.OnPlayerDeath();
            //Let them know to stop spawning
            Destroy(this.gameObject);
        }
    }
}
