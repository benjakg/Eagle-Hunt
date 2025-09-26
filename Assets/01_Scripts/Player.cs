using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5;
    public Rigidbody2D rb;
    public Transform firePoint;
    public bool lokingRight = true;
    public float jumpForce = 5;
    public Transform floorController;
    public Vector2 boxSize;
    public bool IsGrounded;
    public LayerMask layerMask;
    public float life = 3f;
    public float colectedItems = 0;
    public GameObject[] lifes;
    private AudioSource sound;
    public GameObject gameOverScreen;
    public AudioClip damageSound;
    public Transform shootPoint;
    public float range;
    public GameObject impact;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();
        Shoot();
    }
    public void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPoint.position, shootPoint.right, range);
            Debug.DrawRay(shootPoint.position, shootPoint.right*15f, Color.red);
            Debug.Log("Shoot");
            if (raycastHit2D)
            {
                Debug.Log("Shoot");
                if (raycastHit2D.transform.CompareTag("Enemy"))
                {
                    Instantiate(impact, raycastHit2D.point, Quaternion.identity);
                }
            }        
        
                
                
        }
    }
    private bool GetIsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1.8f, LayerMask.GetMask("Ground"));
    }
    void Movement()
    {

        float x = Input.GetAxis("Horizontal");


        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.8f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, Vector2.down * 1.8f, Color.red);

        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && GetIsGrounded()  )
        {
            Jump();
        }
        

    }
    public void Jump()
    {

        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        Debug.Log("Entro al Start");


    }
    public void Rotation()
    {
        float direccionX = Input.GetAxis("Horizontal");
        if (direccionX > 0 && !lokingRight)
        {
            Flip();
        }
        else if (direccionX < 0 && lokingRight)
        {
            Flip();
        }
    }
    public void Flip()
    {
        lokingRight = !lokingRight;
        
        transform.eulerAngles=new Vector3(0f, transform.eulerAngles.y+180, 0f);
    }
}
