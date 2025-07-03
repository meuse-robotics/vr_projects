using UnityEditor;
using UnityEngine;

public class AlignBottoms : MonoBehaviour
{
    [MenuItem("Tools/Align Selected Objects Bottoms to Y=0")]
    static void AlignBottomToYZero()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            // Bounds を取得（Renderer のある場合のみ）
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend == null) continue;

            Bounds bounds = rend.bounds;

            float bottomY = bounds.min.y;
            float deltaY = 0f - bottomY;

            obj.transform.position += new Vector3(0, deltaY, 0);
        }
    }
}
