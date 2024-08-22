using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    TextMeshProUGUI text;
    Color alpha;

    public float moveSpeed;
    public float alphaSpeed;
    public float destroyTime;

    void Awake()
    {
        DamageTextInit();
    }

    void Start() 
    {
        Invoke("DestroyTextObj", destroyTime);    
    }

    void Update()
    {
        ChangeTextView();
    }

    public void DamageTextInit()
    {
        text = GetComponent<TextMeshProUGUI>();
        alpha = text.color;
    }

    public void ChangeTextView()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    public void DestroyTextObj()
    {
        Destroy(gameObject);
    }
}
