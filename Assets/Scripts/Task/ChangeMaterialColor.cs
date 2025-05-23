using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color objectColor = Color.white; // 인스펙터에서 설정할 색상

    void Start()
    {
        ChangeColor(objectColor); // 설정된 색상으로 변경
    }

    void ChangeColor(Color newColor)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material.color = newColor;
        }
    }
}