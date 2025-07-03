using UnityEngine;

public class HitBall : MonoBehaviour
{
    Vector3 prevPosition;
    Vector3 headSpeed;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")){
            var ball = other.gameObject.GetComponent<Rigidbody>();
            var velocity = transform.forward;
            var coefficient = 0f;
            switch(gameObject.name){
                case "Head9":
                coefficient = 0.1f;
                break;
                case "Head5":
                coefficient = 0.25f;
                break;
                case "Head1":
                coefficient = 0.4f;
                break;
                case "HeadP":
                coefficient = 0.1f;
                break;
            }
            ball.AddForce(velocity*headSpeed.magnitude*coefficient, ForceMode.Impulse);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        headSpeed = (transform.position - prevPosition) / Time.deltaTime;
        prevPosition = transform.position;
    }
}
