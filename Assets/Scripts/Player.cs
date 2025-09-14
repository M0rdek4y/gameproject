using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public int maxJumps = 2; // Pulo duplo
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Rigidbody2D rig;
    private bool isGrounded;
    private int jumpsRemaining;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpsRemaining = maxJumps; // Inicializa com o número máximo de pulos
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckGrounded();
        
        // Pulo com barra de espaço (pulo duplo)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpsRemaining > 0)
        {
            // Só pula se estiver no chão OU se for pulo duplo (não está no chão mas tem pulos restantes)
            if (isGrounded || (!isGrounded && jumpsRemaining < maxJumps))
            {
                Jump();
            }
        }
    }
    
    void Move()
    {
        // Usa Input System em vez de Input.GetAxis
        float horizontal = Keyboard.current.aKey.isPressed ? -1f : (Keyboard.current.dKey.isPressed ? 1f : 0f);
        Vector3 movement = new Vector3(horizontal, 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;
        
        // Vira o sprite baseado na direção (preserva o tamanho original)
        if (horizontal > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Direita
        }
        else if (horizontal < -0.1f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Esquerda
        }
        
        // Animação de caminhada
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }
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
        
        // Se estiver no chão, reseta os pulos disponíveis e animação de pulo
        if (isGrounded)
        {
            jumpsRemaining = maxJumps;
            anim.SetBool("jump", false);
        }
    }
    
    void Jump()
    {
        // Aplica força de pulo no Rigidbody2D usando AddForce
        if (rig != null)
        {
            rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            jumpsRemaining--; // Decrementa o número de pulos disponíveis
            
            // Animação de pulo
            anim.SetBool("jump", true);
            
            Debug.Log("Pulo executado! Força: " + JumpForce + " | Pulos restantes: " + jumpsRemaining);
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
