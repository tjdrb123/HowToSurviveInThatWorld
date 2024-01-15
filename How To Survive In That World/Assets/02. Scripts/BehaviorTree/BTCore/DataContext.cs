using UnityEngine;

// BT를 이용한 AI를 만들 때, 필요한 Component들의 저장소
public class DataContext
{
    public GameObject GameObject;  // this.GameObject
    public Transform Transform;  // target.Transform
    public Animator Animator;     // this.Animator
    public Rigidbody Rigidbody;   // this.RigidBody
    public RaycastHit RaycastHit;  // Target & Obstacle Check 
    public Collider OverlapColliders; // Target Information Bring
    
    // 추후 필요한 데이터 추가

    public static DataContext CreatDataContext(GameObject gameObject)
    {
        DataContext dataContext = new DataContext
        {
            GameObject = gameObject,
            Transform = gameObject.transform,
            Animator = gameObject.GetComponent<Animator>(),
            Rigidbody = gameObject.GetComponent<Rigidbody>(),
            RaycastHit = gameObject.GetComponent<RaycastHit>(),
            OverlapColliders = gameObject.GetComponent<Collider>()
        };

        // 추후 필요한 데이터 추가
        
        return dataContext;
    }
}
