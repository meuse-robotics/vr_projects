using System.Linq;
using UnityEngine;
using UnityEngine.AI;

// Terrain コンポーネントが必須
[RequireComponent(typeof(Terrain))]
public class ExtractTreeCollidersFromTerrain : MonoBehaviour
{
    // コンテキストメニューから "Extract" を呼び出せるようにする
    [ContextMenu("Extract")]
    public void Extract()
    {
        Debug.Log("ExtractTreeCollidersFromTerrain::Extract");

        // Terrain を取得
        Terrain terrain = GetComponent<Terrain>();

        // Terrain の子オブジェクト（以前作られたカプセルなど）を取得
        Transform[] transforms = terrain.GetComponentsInChildren<Transform>();

        // 最初の要素は Terrain 自身なのでスキップ
        for (int i = 1; i < transforms.Length; i++)
        {
            // 既に作成済みのカプセルなどを削除
            DestroyImmediate(transforms[i].gameObject);
        }

        Debug.Log("Tree prototypes count: " + terrain.terrainData.treePrototypes.Length);

        // 各 TreePrototype（= 各種類の木）ごとに処理
        for (int i = 0; i < terrain.terrainData.treePrototypes.Length; i++)
        {
            TreePrototype tree = terrain.terrainData.treePrototypes[i];

            // 同じ種類の木インスタンスをすべて取得
            TreeInstance[] instances = terrain.terrainData.treeInstances
                .Where(x => x.prototypeIndex == i).ToArray();

            Debug.Log("Tree prototypes[" + i + "] instance count: " + instances.Length);

            // 各インスタンスに対して処理
            for (int j = 0; j < instances.Length; j++)
            {
                // 木の位置をワールド座標に変換
                instances[j].position = Vector3.Scale(instances[j].position, terrain.terrainData.size);
                instances[j].position += terrain.GetPosition();

                // 木のプレハブに NavMeshObstacle コンポーネントが付いているか確認
                NavMeshObstacle nav_mesh_obstacle = tree.prefab.GetComponent<NavMeshObstacle>();
                if (!nav_mesh_obstacle)
                {
                    Debug.LogWarning("Tree with prototype[" + i + "] instance[" + j + "] did not have a NavMeshObstacle component, skipping!");
                    continue;
                }

                // NavMeshObstacle のサイズを使用（Capsule の場合は radius を使う）
                Vector3 primitive_scale = nav_mesh_obstacle.size;
                if (nav_mesh_obstacle.shape == NavMeshObstacleShape.Capsule)
                {
                    primitive_scale = nav_mesh_obstacle.radius * Vector3.one;
                }

                // カプセル型の障害物を作成
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                obj.name = tree.prefab.name + j;

                // レイヤーを元のプレハブに合わせるか、Terrain のレイヤーを使うか
                if (terrain.preserveTreePrototypeLayers)
                    obj.layer = tree.prefab.layer;
                else
                    obj.layer = terrain.gameObject.layer;

                // スケール・位置・親子関係を設定
                obj.transform.localScale = primitive_scale;
                obj.transform.position = instances[j].position;
                obj.transform.parent = terrain.transform;

                // 静的オブジェクトに設定（NavMesh baking の対象にするため）
                obj.isStatic = true;
            }
        }
    }
}
