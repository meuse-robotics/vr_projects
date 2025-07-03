using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // 左右の前輪の WheelCollider（駆動・操舵）
    [SerializeField] WheelCollider frontLeftW, frontRightW;
    // エンジン音の AudioSource
    [SerializeField] AudioSource engineSound;
    // ハンドルの親オブジェクト（見た目の回転用）
    [SerializeField] GameObject steerParent;
    // タコメーターの親オブジェクト（回転数表示）
    [SerializeField] GameObject tachParent, speedParent;

    // 最初に呼ばれる初期化処理
    void Start()
    {
        // 駆動力を 0 にして初期化
        frontLeftW.motorTorque = 0f;
        frontRightW.motorTorque = 0f;
    }

    // 毎フレーム呼ばれる更新処理
    void Update()
    {
        // 右手のVRコントローラーデバイスを取得
        var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand,rightHandDevices);

        // デバイスごとに処理
        foreach (var device in rightHandDevices)
        {
            // スティックの位置（2DAxis）を取得
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis,out Vector2 position))
            {
                if (position.y >= 0)
                {
                    // 前進操作時（前進トルクを与え、ブレーキなし）
                    frontLeftW.brakeTorque = 0f;
                    frontRightW.brakeTorque = 0f;
                    frontLeftW.motorTorque = position.y * 100f;
                    frontRightW.motorTorque = position.y * 100f;

                    // エンジン音のピッチを加速に合わせて変更
                    engineSound.pitch = 1f + position.y;
                }
                else if (position.y < 0)
                {
                    // 後退（ブレーキ）操作時（トルクなし、ブレーキ力を与える）
                    frontLeftW.brakeTorque = -position.y * 200f;
                    frontRightW.brakeTorque = -position.y * 200f;
                    frontLeftW.motorTorque = 0f;
                    frontRightW.motorTorque = 0f;

                    // エンジン音はアイドリング状態に戻す
                    engineSound.pitch = 1f;
                }

                // ステアリング角度（左右方向）を設定
                frontLeftW.steerAngle = position.x * 20f;
                frontRightW.steerAngle = position.x * 20f;

                // ステアリングの見た目を回転
                steerParent.transform.localRotation = Quaternion.Euler(0, 0, -position.x * 100f);

                // タコメーターをスティックYで回転（前進時のみ）
                if (position.y >= 0)
                    tachParent.transform.localRotation = Quaternion.Euler(0, 0, -position.y * 100f);

                // スピードの計算（RPM → m/s → km/h に変換して針を回転）
                var speed = frontLeftW.rpm * 2f * Mathf.PI * frontLeftW.radius * 60f / 1000f;
                speedParent.transform.localRotation = Quaternion.Euler(0, 0, -speed * 2f);
            }
        }
    }
}
