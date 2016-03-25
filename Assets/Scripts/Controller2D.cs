using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticleRayCount = 4;

    private float horizontalRaySpacing;
    private float verticleRaySpacing;

    private BoxCollider2D collider;
    private Player player;
    private RaycastOrigins raycastOrigins;
    private CollisionInfo collisions;
    public CollisionInfo GetCollisions
    {
        get { return collisions; }
    }

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        player = GetComponent<Player>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }

        if (velocity.y != 0)
        {
            VerticleCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    private void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLenght = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLenght, Color.red);

            if (hit)
            {
                if (hit.collider.gameObject.tag != "Platform")
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLenght = hit.distance;

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;

                    if (directionX == -1)
                    {
                        collisions.gLeft = hit.collider.gameObject;
                    }
                    else if (directionX == 1)
                    {
                        collisions.gRight = hit.collider.gameObject;
                    }
                }
            }
        }
    }

    private void VerticleCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLenght = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticleRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticleRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, collisionMask);

            if (hit)
            {
                if(hit.collider.gameObject.tag == "Platform")
                {
                    if(directionY == 1 || hit.distance == 0f)
                    {
                        continue;
                    }
                }

                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLenght = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;

                if (directionY == -1)
                {
                    collisions.gBelow = hit.collider.gameObject;
                }
                else if (directionY == 1)
                {
                    collisions.gAbove = hit.collider.gameObject;
                }
            }
        }
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticleRayCount = Mathf.Clamp(verticleRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticleRaySpacing = bounds.size.x / (verticleRayCount - 1);
    }

    private struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public GameObject gAbove, gBelow;
        public GameObject gLeft, gRight;

        public void Reset()
        {
            above = below = false;
            left = right = false;

            GameObject nullObject = null;
            gAbove = gBelow = nullObject;
            gLeft = gRight = nullObject;
        }
    }
}