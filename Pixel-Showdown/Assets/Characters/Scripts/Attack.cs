using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private int attackDamage = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<GutsActions>().Hit(attackDamage);
    }
}
