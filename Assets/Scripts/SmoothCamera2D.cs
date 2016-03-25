using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{
    [SerializeField]
    private float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float minHeight;
    [SerializeField]
    private float minXOffset;

    private bool onCheck = false;
    public bool OnCheck
    {
        get { return onCheck; }
        set { onCheck = value; }
    }

    void Update()
    {
        if (!onCheck)
        {
            if (target)
            {
                Vector3 point = Camera.main.WorldToViewportPoint(target.position);
                Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }

            if (transform.position.y <= minHeight)
            {
                transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
            }
            if (transform.position.x <= minXOffset)
            {
                transform.position = new Vector3(minXOffset, transform.position.y, transform.position.z);
            }
        }
    }
}