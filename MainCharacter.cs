using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MAinCharacter : MonoBehaviour
{
    private float playerHealth =100, playerStamina, playerDamage, playerDefense = 100;
    private float playerJumpForce, playerSpeed;
    private float acumulatedTime;
    private int jumps = 0;

    public string playerName, playerStatus;

    public float speed = 3, jumpForce = 100, damage =  3;


    public KeyCode agacharse = KeyCode.C;
    public KeyCode arriba = KeyCode.W;
    public KeyCode abajo = KeyCode.S;
    public KeyCode correr = KeyCode.LeftShift;
    public KeyCode salto = KeyCode.Space;

    public KeyCode ataque = KeyCode.J;
    public KeyCode bloquear = KeyCode.K;

    Rigidbody2D playerRb;
    CapsuleCollider2D playerCollider;
    
    void Start(){
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate() {
        // no hay prioridad en las acciones 
        // no puede haber ciclos largos
        IsShifted();
        IsGrounded();
        PlayerMove();
        PlayerJump();
        PlayerSlide();
        PlayerAtack();
        PlayerDefense();
    }

    void PlayerMove(){
        var movement = Input.GetAxis("Horizontal");
        playerRb.velocity = new Vector2(movement * playerSpeed, playerRb.velocity.y);
    } 
    void PlayerJump(){
        // si esta apoyado en una superficie  o si estando en el aire solo ha hecho un salto o ninguno
        if ((IsGrounded() || jumps<=1) && Input.GetKeyDown(salto)){
            playerJumpForce = jumps ==1 ? jumpForce * 0.4f : jumpForce;
            playerRb.AddForce(Vector2.up * playerJumpForce);
            jumps += 1;
        }
    }
    void PlayerSlide(){
        // si esta apoyado a una superficie, va corriendo shifted y se presiona la tecla
        // se rota 90° respecto a y, se baja la ubicacion en heigh/2 + widht /2
        if (IsGrounded() && IsShifted() && Input.GetKey(agacharse)) {
            playerRb.MoveRotation(90);
            Vector2 playerDimenssions = playerCollider.size;
            Vector2 newPos = new Vector2 (playerRb.position.x,playerDimenssions.x/2); // probar posicionamiento
            playerRb.MovePosition(newPos);
        }
    }
    void PlayerAtack(){
        // Si izquierdo ataque ligero si mantenido atale pesado modifica variables
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(ataque)){
            Debug.Log("Ataque ligero");
            playerDamage = damage;
        }
        if (Input.GetMouseButton(0) || Input.GetKey(ataque)){
            Debug.Log("Ataque Pesado");
            acumulatedTime += Time.deltaTime;
            playerDamage = damage * 1.5f * acumulatedTime; 
        }
    }
        void PlayerDefense(){
        //  
        if (Input.GetMouseButton(1) || Input.GetKey(bloquear)){
            Debug.Log("bloquea");
        }
    }
    bool IsShifted() {
        if (Input.GetKey(correr)){
            playerJumpForce = jumpForce * 1.4f;
            playerSpeed = speed * 2.5f;
            return true;
        }
        playerSpeed = speed;
        playerJumpForce = jumpForce;
        return false;
    }
    bool IsGrounded() { 
        jumps = 0;
        return Physics2D.Raycast(playerRb.position, Vector2.down, 1.1f); 
        //retorno tipo Raycast2D con collider name distancia  y otras vainas
    }
}