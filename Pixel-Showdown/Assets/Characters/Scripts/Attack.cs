using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<GutsActions>().Hit(attackDamage);
    }
}
