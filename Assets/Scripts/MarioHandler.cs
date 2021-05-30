using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioHandler : MonoBehaviour
{
    public ParticleSystem particle;
    public float boundaryX;
    public float timeToLive;
    public bool isShot;
    private bool startedSelfDestruction;

    private void Start()
    {
        isShot = false;
    }

    void Update()
    {
        if (isShot && !startedSelfDestruction)
        {
            StartSelfDestruction();
            startedSelfDestruction = true;
        }

        if (Mathf.Abs(transform.position.x) > boundaryX)
        {
            Kill();
        }
    }
    private void StartSelfDestruction()
    {
        if (!FindObjectOfType<GameManager>().isGameover)
            Invoke("Die", timeToLive);    
    }


    private void Die()
    {
        particle.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
        FindObjectOfType<AudioManager>().PlayOnDestroy();
        Invoke("Kill", 1.5f);
    }

    private void Kill ()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            GetComponent<Animator>().SetBool("Dead_b", true);
        }
    }
}
