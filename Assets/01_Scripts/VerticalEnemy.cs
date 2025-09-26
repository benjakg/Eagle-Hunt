using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalEnemy : MonoBehaviour
{
    public float speed = 2f;
    public float minY = -2.9f;
    public float maxY = 2.55f;
    public AudioClip deathSound;
    [Range(0f, 1f)]
    public float deathVolume = 2f;
    private AudioSource audioSource;
    [Range(0f, 1f)]
    private int direction = 1;
    private Rigidbody2D rb;

    public float detectionRange = 0.5f;
    public LayerMask playerLayer;

    private void Start()
    {
        InitializeComponents();
        SetStartPosition();
    }

    private void Update()
    {
        Move();
        CollisionWithPlayer();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void SetStartPosition()
    {
        Vector3 pos = transform.position;
        pos.y = minY;
        transform.position = pos;
    }

    private void Move()
    {
        transform.Translate(Vector3.up * direction * speed * Time.deltaTime);

        if (transform.position.y >= maxY)
            direction = -1;
        else if (transform.position.y <= minY)
            direction = 1;
    }

    private void CollisionWithPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        Vector2 enemyPos = transform.position;
        float killRange = detectionRange * 0.5f;

        if (playerObj != null)
        {
            Vector2 playerPos = playerObj.transform.position;
            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            foreach (Vector2 dir in directions)
            {
                Vector2 checkPos = enemyPos + dir * detectionRange;
                Debug.DrawLine(enemyPos, checkPos, Color.green);

                if (Vector2.Distance(checkPos, playerPos) < killRange)
                {
                    Destroy(playerObj);
                    break;
                }
            }
        }

        foreach (GameObject bullet in bullets)
        {
            Vector2 bulletPos = bullet.transform.position;
            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            foreach (Vector2 dir in directions)
            {
                Vector2 checkPos = enemyPos + dir * detectionRange;
                Debug.DrawLine(enemyPos, checkPos, Color.red);

                if (Vector2.Distance(checkPos, bulletPos) < killRange)
                {
                    Die();
                    Destroy(bullet);
                    return;
                }
            }
        }
    }

    private void Die()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, deathVolume);
        }

        Destroy(gameObject, 0.05f);
    }
}
