using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public ParticleSystem particles;
    public HealthBarController healthbar;
    public EnemyScriptableObject enemyScript;
    public float hitVelocityThreshold;

    private GameManager gm;
    private AudioManager audioManager;
    private int currentHealth;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        gm = FindObjectOfType<GameManager>();
        currentHealth = enemyScript.MaxHealth;
        healthbar.SetMaxHealth(enemyScript.MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * enemyScript.MoveSpeed);

        if (gameObject.CompareTag("Boss") && transform.position.x < 10)
        {
            audioManager.PlayOnBoss();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && collision.relativeVelocity.magnitude >= hitVelocityThreshold)
        {
            audioManager.PlayOnHit();
            currentHealth -= 100;
            healthbar.SetCurrentHealth(currentHealth);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deadline") && !isDead)
        {
            gm.GameOver();
        }
    }

    private void Die()
    {
        isDead = true;
        particles.Play();
        gm.AddScore(enemyScript.Reward);
        GetComponentInChildren<Canvas>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().isTrigger = true;
        Invoke("Kill", 2f);
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
