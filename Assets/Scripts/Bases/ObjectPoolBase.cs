using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 상속용 ObjectPoolBase 스크립트입니다.
// 자주 생성 / 제거 되는 오브젝트들을 Object Pooling 방식으로 사용하여 메모리 효율을 증가시킵니다.
public abstract class ObjectPoolBase : MonoBehaviour
{
    [SerializeField] protected GameObject objectPrefab; // 대상이 될 Object Prefab입니다.
    protected readonly Queue<GameObject> pool = new(); // Object들을 Queue로 관리합니다.
    protected readonly List<GameObject> activeObj = new(); // 활성화된 Object들을 List로 관리합니다.
    protected int poolSize; //생성될 Pool의 Size 즉, Object의 갯수입니다.
    protected virtual void Awake()
    {
        DIContainer.Register(this);
    }
    protected void SetPoolSize(int mySize)
    {
        // 필요한 만큼 Pool의 크기를 조절합니다.
        poolSize = mySize;
    }
    public virtual void Initialize()
    {
        //풀의 크기만큼 반복합니다.
        for (int i = 0; i < poolSize; i++)
        {
            // Object를 생성합니다.
            GameObject obj = Instantiate(objectPrefab, transform);
            // 비활성화 상태로 대기시킵니다.
            obj.SetActive(false);
            //pool에 추가합니다.
            pool.Enqueue(obj);
        }
    }
    public virtual GameObject Get(Vector3 pos, Quaternion rot)
    {
        //pool이라는 Queue에서 꺼냅니다. Queue가 비어있다면, 새로 생성합니다.
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(objectPrefab, transform);
        // Object의 Position을 pos로, Rotation을 rot으로 보정합니다.
        obj.transform.SetPositionAndRotation(pos, rot);
        // pool에서 꺼낸 Object들을 활성화시킵니다.
        obj.SetActive(true);
        // activeObj라는 활성화 리스트에 추가합니다.
        activeObj.Add(obj);
        //해당 오브젝트를 반환합니다.
        return obj;
    }
    public virtual void Release(GameObject obj)
    {
        //파라미터로 들어온 obj를 비활성화시킵니다.
        obj.SetActive(false);
        //활성화 리스트에서 제거합니다.
        activeObj.Remove(obj);
        //pool에서 제거합니다.
        pool.Enqueue(obj);
    }
    public virtual void ReleaseAll()
    {
        //활성화 리스트에 존재하는 모든 활성화 오브젝트를 비활성화 시킵니다.
        foreach (GameObject obj in activeObj.ToList()) Release(obj);
        // 활성화 리스트를 모두 비웁니다.
        activeObj.Clear();
    }
}
