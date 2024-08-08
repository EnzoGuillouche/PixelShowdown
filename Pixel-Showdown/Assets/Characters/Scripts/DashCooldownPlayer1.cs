using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCooldownPlayer1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(GutsActions1.dashCooldown*3, transform.localScale.y);
    }
}
