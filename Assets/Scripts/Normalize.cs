using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Normalize : MonoBehaviour
{
    TextMeshProUGUI tmPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmPro = gameObject.GetComponent<TextMeshProUGUI>();
        Vector3 vec = Vector3.zero;
        Vector3 vec1 = vec;
        print(NormalizeInPlace(ref vec1));
        Vector3 vec2 = vec;

        tmPro.text = "1: " + vec1 + "\n2: " + vec2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    float NormalizeInPlace(ref Vector3 v)
    {
        float length = v.magnitude; // + 1e-10f
        float invLength = 1 / length;
        v.x = v.x * invLength;
        v.y = v.y * invLength;
        v.z = v.z * invLength;
        return length * invLength;
    }
}
