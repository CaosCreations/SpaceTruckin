using UnityEngine;

public abstract class CollisionTriggerBehaviour : MonoBehaviour
{
    [Header("Leave all blank that aren't used in the condition")]
    [SerializeField] protected Condition[] conditions;

    [SerializeField] private Operator op;

    private BoxCollider boxCollider;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    protected virtual bool IsTriggerable(Collider other)
    {
        return other.CompareTag(PlayerConstants.PlayerTag) && (op == Operator.And ? conditions.AreAllMet() : conditions.AreAnyMet());
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsTriggerable(other))
        {
            Debug.Log($"Player collided with {GetType()}: {name}");
            TriggerBehaviour();
            gameObject.DestroyIfExists();
        }
    }

    protected abstract void TriggerBehaviour();

    protected void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
