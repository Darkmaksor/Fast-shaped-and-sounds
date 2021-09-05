using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    [Range(5,100)]
    public int maxHealth = 50;
    [Range(1, 30)]
    [SerializeField] private int _speed = 5;
    [Range(0f,2f)]
    [SerializeField] private float _cooldown = 1f;
    private float _timer;
    
    [Range(0f,5f)]
    [SerializeField] private float _dashRange = 1f;

    private string _direction;
    private Rigidbody2D _rb;
    private Vector2 _moveVelocity;

    [HideInInspector] public bool isDashing;
    
    public Joystick joystick;
    public GameObject tail;
    public ParticleSystem dashParticle;

    private Rigidbody2D _rigidbody;
    
    [HideInInspector] public int currentHealth;

    
    void Start()
    {
        isDashing = false;
        currentHealth = maxHealth;
        _rb = GetComponent<Rigidbody2D>();
        _timer = 0f;
        isDashing = false;
        _direction = "right";
    }
    void Update()
    {
        //Character movement
        Vector2 moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        _moveVelocity = moveInput.normalized * (float)_speed;
        

        //Speed changing
        if (joystick.Vertical >= 0.5 || joystick.Vertical <= -0.5)
            _speed = 9;
        else if (joystick.Horizontal >= 0.5 || joystick.Horizontal <= -0.5)
            _speed = 9;
        else if (joystick.Vertical < 0.5 || joystick.Vertical > -0.5)
            _speed = 4;
        else if (joystick.Horizontal < 0.5 || joystick.Horizontal > -0.5)
            _speed = 4;
        
        //dashing 
        if(isDashing && _timer <= 0)
        {
            if(_direction == "right")
            {
                dashParticle.Play();
                transform.position = new Vector2(transform.position.x +_dashRange,transform.position.y);
                isDashing = false;
                _timer = _cooldown;
            } else if(_direction == "left")
            {
                dashParticle.Play();
                transform.position = new Vector2(transform.position.x -_dashRange,transform.position.y);
                isDashing = false;
                _timer = _cooldown;
            }else if(_direction == "up")
            {
                dashParticle.Play();
                transform.position = new Vector2(transform.position.x,transform.position.y + _dashRange);
                isDashing = false;
                _timer = _cooldown;
            }else if(_direction == "down")
            {
                dashParticle.Play();
                transform.position = new Vector2(transform.position.x,transform.position.y - _dashRange);
                isDashing = false;
                _timer = _cooldown;
            }
        }
        else if(_timer >= 0)
        {
            _timer -= Time.deltaTime;
        }
        
            
            
    }
    //Player Movement
    void FixedUpdate()
    {
        if(joystick.Horizontal != 0 && joystick.Vertical != 0)
        {
            _rb.MovePosition(_rb.position + _moveVelocity * Time.fixedDeltaTime);
            tail.SetActive(true);
        }
        else
        {
            StartCoroutine(turnOffParticle());
        }
    }
    IEnumerator turnOffParticle()
    {
        yield return new WaitForSeconds(0.2f);
        tail.SetActive(false);

    }
    public void TakeDamage(int damage)
    {
        if(!isDashing)
            currentHealth -= damage;
        if (currentHealth <= 0)
        {
            print("You die!");
            //sounds.Hurt();
        }
    }
    
    public void SwitchDirection(string newDirection)
    {
        _direction = newDirection;
    }
    public void Dashing()
    {
        isDashing = true;
    }
    
    

}
