using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionType
{
    Enter,
    Stay,
    Exit
}

public class SingleCollisionPipelineMonoBehaviour : SerializedMonoBehaviour
{
    protected virtual void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollision(collision.gameObject, collision.ClosestPoint(collision.gameObject.transform.position), true, CollisionType.Enter);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision(collision.gameObject, collision.GetContact(0).point, false, CollisionType.Enter);
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        OnCollision(collision.gameObject, collision.ClosestPoint(collision.gameObject.transform.position), true, CollisionType.Stay);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        OnCollision(collision.gameObject, collision.collider.ClosestPoint(collision.gameObject.transform.position), false, CollisionType.Stay);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        OnCollision(collision.gameObject, collision.ClosestPoint(collision.gameObject.transform.position), true, CollisionType.Exit);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        OnCollision(collision.gameObject, collision.collider.ClosestPoint(collision.gameObject.transform.position), false, CollisionType.Exit);
    }
}
