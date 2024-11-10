using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerActions>() != null) other.GetComponent<PlayerActions>().Hit(attackDamage);
    }
}
