using UnityEngine;

[ExecuteInEditMode]
public class SetZPos : MonoBehaviour
{
    void Update()
    {
        Vector3 transformPosition = new Vector3(transform.position.x, transform.position.y, transform.position.y*0.1f);
        transform.localPosition = transformPosition;
    }
}
