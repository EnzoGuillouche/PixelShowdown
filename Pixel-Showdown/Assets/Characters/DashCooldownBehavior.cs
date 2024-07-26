using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCooldownBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(GutsActions.dashCooldown, transform.localScale.y);
    }
}
