using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviController : MonoBehaviour
{
    [SerializeField] GameObject car;		//カーモデル
    [SerializeField] GameObject destination;    //目的地
    [SerializeField] GameObject carOnNavi;  //カーマーカー
    [SerializeField] GameObject destOnNavi; //目的地マーカー
    private Vector3[] places = new Vector3[4];

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond); // 動的なシード

        Vector3 pos = carOnNavi.GetComponent<RectTransform>().anchoredPosition;
        pos.x = 20f;
        pos.y = -52.4f;
        carOnNavi.GetComponent<RectTransform>().anchoredPosition = pos;
        pos = destOnNavi.GetComponent<RectTransform>().anchoredPosition;
        pos.x = 20f;
        pos.y = -42.2f;
        destOnNavi.GetComponent<RectTransform>().anchoredPosition = pos;

        places[0] = new Vector3(-430, 0.5f, 400);
        places[1] = new Vector3(-525, 0.5f, -117);
        places[2] = new Vector3(400, 0.5f, -300);
        places[3] = new Vector3(320, 0.5f, 400);
    }

    // Update is called once per frame
    void Update()
    {
        // Navi上の車の位置
        Vector3 carPos = car.transform.position;
        carPos.y = carPos.z;
        carOnNavi.GetComponent<RectTransform>().anchoredPosition = carPos;

        // Navi上のdestOnNaviを動かす(左スティック入力)
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        foreach (var device in leftHandDevices)
        {
            //目的地移動
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 position))
            {
                Vector3 destPos = destOnNavi.GetComponent<RectTransform>().anchoredPosition;
                destPos.x += Mathf.Round(position.x) * 2;
                destPos.y += Mathf.Round(position.y) * 2;
                destOnNavi.GetComponent<RectTransform>().anchoredPosition = destPos;
            }
            //目的地設定
            bool triggerValue;
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                Vector3 destPos = destOnNavi.GetComponent<RectTransform>().anchoredPosition;
                destPos.z = destPos.y;
                destPos.y = 0;
                destination.transform.position = destPos;
            }
        }
        // 目的地更新
        if (Vector3.Distance(car.transform.position, destination.transform.position) < 15f)
        {
            Vector3 nextDest = destination.transform.position;
            int p = Random.Range(0, 4);
            nextDest.x = places[p].x;
            nextDest.z = places[p].z;
            destination.transform.position = nextDest;
        }

    }
}
