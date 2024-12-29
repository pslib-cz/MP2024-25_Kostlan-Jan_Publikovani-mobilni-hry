using UnityEngine;

public class ObjectRotationWithLimit : MonoBehaviour
{
    public float rotationSpeed = 30.0f;
    public float maxRotationAngle = 90.0f;
    public float minRotationAngle = 0.0f;

    private float currentRotationAngle = 0.0f;
    private bool rotatingForward = true;
    private Quaternion[] initialRotations;

    void Start()
    {
        int childCount = transform.childCount;
        initialRotations = new Quaternion[childCount];
        for (int i = 0; i < childCount; i++)
        {
            initialRotations[i] = transform.GetChild(i).localRotation;
        }
    }

    void Update()
    {
        float rotationIncrement = rotationSpeed * Time.deltaTime;

        if (rotatingForward)
        {
            currentRotationAngle += rotationIncrement;

            if (currentRotationAngle >= maxRotationAngle)
            {
                currentRotationAngle = maxRotationAngle;
                rotatingForward = false;
            }
        }
        else
        {
            currentRotationAngle -= rotationIncrement;

            if (currentRotationAngle <= minRotationAngle)
            {
                currentRotationAngle = minRotationAngle;
                rotatingForward = true;
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localRotation = initialRotations[i] * Quaternion.Euler(0, 0, currentRotationAngle);
        }
    }
}
