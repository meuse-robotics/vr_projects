using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    [SerializeField] WheelCollider frontLeftW, frontRightW;
    [SerializeField] AudioSource engineSound;
    [SerializeField] GameObject steerParent;
    [SerializeField] GameObject tachParent, speedParent;
    [SerializeField] GameObject agent;
    float steeringSensitivity = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        frontLeftW.motorTorque = 0f;
        frontRightW.motorTorque = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //agentはthis(Car1)が15m以上離れたら止まって待つ
        if (Vector3.Distance(agent.transform.position, this.transform.position) > 7.5f)
        {
            agent.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
        }
        else
        {
            agent.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
        }

        //Carから見たagentの位置
        Vector3 localTarget = this.transform.InverseTransformPoint(agent.transform.position);
        //Carから見たagentの方向
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        //Carの速度（値）
        float currentSpeed = Mathf.RoundToInt(this.GetComponent<Rigidbody>().linearVelocity.magnitude);
        //操舵量
        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1);
        //曲率
        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);

        //スピードの出すぎを抑える
        if ((corner > 10f && currentSpeed > 3f) //カーブでのスピード
            || currentSpeed > 10f		     //最高速度
            || Vector3.Distance(agent.transform.position, this.transform.position) < 6f)	//agentとの距離
        {
            frontLeftW.brakeTorque = 400f;
            frontRightW.brakeTorque = 400f;
            frontLeftW.motorTorque = 0f;
            frontRightW.motorTorque = 0f;
            engineSound.pitch = 1f;
        }
        else
        {
            frontLeftW.brakeTorque = 0f;
            frontRightW.brakeTorque = 0f;
            frontLeftW.motorTorque = 100f;
            frontRightW.motorTorque = 100f;
            engineSound.pitch = 2f;
        }
        frontLeftW.steerAngle = steer * 45f;
        frontRightW.steerAngle = steer * 45f;

        //パーツのアニメーション
        steerParent.transform.localRotation = 
        Quaternion.Euler(0, 0, -steer * 100f);
        if (frontLeftW.motorTorque >= 0)
            tachParent.transform.localRotation = Quaternion.Euler(0, 0, -frontLeftW.motorTorque);
        var speed = frontLeftW.rpm * 2f * Mathf.PI * frontLeftW.radius * 60f / 1000f;
        speedParent.transform.localRotation = Quaternion.Euler(0, 0, -speed * 2f);
    }
    
}

