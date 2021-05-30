using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripRenderer : MonoBehaviour
{
    public Transform frontPosition;
    public Transform backPosition;
    public Transform centerPosition;

    public LineRenderer frontStrip;
    public LineRenderer backStrip;

    public GameObject mario;
    public GameObject marioAnimation;

    public float spawnTime;
    public float maxLength;
    public float force;

    private Vector3 mousePosition;
    private bool isMouseDown = false;
    private Rigidbody2D rb;
    private GameObject marioBody;
    private GameObject marioAnimationSpawned;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        SpawnMario();
        frontStrip.positionCount = 2;
        backStrip.positionCount = 2;
        frontStrip.SetPosition(0, frontPosition.position);
        backStrip.SetPosition(0, backPosition.position);
    }

    private void Update()
    {
        if (isMouseDown)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;
            mousePosition = centerPosition.position + Vector3.ClampMagnitude(mousePosition - centerPosition.position, maxLength);
            frontStrip.SetPosition(1, mousePosition);
            backStrip.SetPosition(1, mousePosition);
            Vector3 direction = mousePosition - centerPosition.position;
            rb.transform.position = mousePosition + direction.normalized * 0.2f;
            rb.transform.right = -direction.normalized;
        } else
        {
            ResetStrips();
        }
    }

    private void OnMouseDown()
    {
        if (rb == null)
        {
            SpawnMario();
        }
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        GetComponent<BoxCollider2D>().enabled = false; // disable collider to stop mouse event until next mario spawned
        isMouseDown = false;
        rb.isKinematic = false;
        ShootMario();
        StartMarioSpawnAnimation();
        Invoke("TurnOnCollider", 0.2f);
        Invoke("SpawnMario", spawnTime);
    }

    private void TurnOnCollider()
    {
        rb.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void ShootMario()
    {
        marioBody.GetComponent<Animator>().SetTrigger("Fly_trig");
        marioBody.GetComponent<MarioHandler>().isShot = true;
        audioManager.PlayOnShoot();
        Vector3 velocity = (rb.transform.position - centerPosition.position) * force * -1;
        rb.velocity = velocity;
    }

    private void ResetStrips()
    {
        frontStrip.SetPosition(1, frontStrip.GetPosition(0));
        backStrip.SetPosition(1, backStrip.GetPosition(0));
    }

    private void SpawnMario()
    {
        marioBody = Instantiate(mario);
        marioBody.GetComponent<MarioHandler>().isShot = false;
        rb = marioBody.GetComponent<Rigidbody2D>();
        rb.GetComponent<Collider2D>().enabled = false;
        rb.isKinematic = true;
        rb.transform.position = centerPosition.position;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void StartMarioSpawnAnimation()
    {
        marioAnimationSpawned = Instantiate(marioAnimation);
        Invoke("KillMarioAnimation", 2);
    }

    private void KillMarioAnimation()
    {
        Destroy(marioAnimationSpawned);
        marioAnimationSpawned = null;
    }
}
