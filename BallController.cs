using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool onGround;
    private Rigidbody rb;
    [SerializeField] AudioClip shotSound;
    [SerializeField] AudioClip cupSound;
    private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Terrain")
        {
            onGround = true; // 接地
        }
        else if(other.gameObject.name == "Head9"
            || other.gameObject.name == "Head5"
            || other.gameObject.name == "Head1")
        {
            audioSource.clip = shotSound;
            audioSource.Play();
        }
        else if(other.gameObject.name == "Flag")
        {
            audioSource.clip = cupSound;
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Terrain")
        {
            onGround = false; // 離地
        }
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (onGround)
        {
            rb.AddForce(-rb.linearVelocity.normalized * 0.2f); // 減速
        }
    }
}
