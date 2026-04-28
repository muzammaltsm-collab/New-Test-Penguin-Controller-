using UnityEngine;

public class PointMover : MonoBehaviour
{
    [Header("Points")]
    public Transform[] points;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float rotateSpeed = 8f;
    public float reachDistance = 0.1f;

    [Header("Loop Settings")]
    public bool loop = true;
    public bool reverseOnLoop = true;

    private int currentPointIndex = 0;
    private bool movingForward = true;
    private bool isMoving = true;

    void Update()
    {
        if (!isMoving || points == null || points.Length == 0)
            return;

        MoveToPoint();
    }

    void MoveToPoint()
    {
        Transform targetPoint = points[currentPointIndex];

        Vector3 direction = targetPoint.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPoint.position) <= reachDistance)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (movingForward)
        {
            currentPointIndex++;

            if (currentPointIndex >= points.Length)
            {
                if (!loop)
                {
                    isMoving = false;
                    currentPointIndex = points.Length - 1;
                    return;
                }

                if (reverseOnLoop)
                {
                    movingForward = false;
                    currentPointIndex = points.Length - 2;
                }
                else
                {
                    currentPointIndex = 0;
                }
            }
        }
        else
        {
            currentPointIndex--;

            if (currentPointIndex < 0)
            {
                if (!loop)
                {
                    isMoving = false;
                    currentPointIndex = 0;
                    return;
                }

                movingForward = true;
                currentPointIndex = 1;
            }
        }
    }
}