using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color objectColor = Color.white; // �ν����Ϳ��� ������ ����

    void Start()
    {
        ChangeColor(objectColor); // ������ �������� ����
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