using UnityEngine;

public class LoadingSpinner : MonoBehaviour
{
    // Gốc của Spinner  
    public float rotationSpeed = 200f;

    private void Update()
    {
        // Xoay spinner khoảng góc theo thời gian  
        transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
    }
}