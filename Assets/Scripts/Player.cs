using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Rigidbody2D rig;
    private bool isGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckGrounded();
        
        // Pulo com barra de espaço (só pula se estiver no chão)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            Jump();
        }
    }
    
    void Move()
    {
        // Usa Input System em vez de Input.GetAxis
        float horizontal = Keyboard.current.aKey.isPressed ? -1f : (Keyboard.current.dKey.isPressed ? 1f : 0f);
        Vector3 movement = new Vector3(horizontal, 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;
    }
    
    void CheckGrounded()
    {
        // Verifica se está no chão usando Raycast
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        
        // Se não tiver layer configurada, usa uma verificação simples baseada na velocidade
        if (groundLayer == 0)
        {
            isGrounded = rig != null && Mathf.Abs(rig.linearVelocity.y) < 0.1f;
        }
    }
    
    void Jump()
    {
        // Aplica força de pulo no Rigidbody2D usando AddForce
        if (rig != null)
        {
            rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            Debug.Log("Pulo executado! Força: " + JumpForce + " | No chão: " + isGrounded);
        }
        else
        {
            Debug.LogError("Rigidbody2D não encontrado!");
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Desenha o círculo de verificação do chão no editor
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
        else
        {
            // Desenha o raycast se não tiver groundCheck
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1f);
        }
    }
}
