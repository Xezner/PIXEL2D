using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    public void DisableCollisionOnDeath()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Collider2D>().attachedRigidbody.bodyType = RigidbodyType2D.Static;
    }

    public void DestroyOnDeath()
    {
        Destroy(this.gameObject);
    }
}
