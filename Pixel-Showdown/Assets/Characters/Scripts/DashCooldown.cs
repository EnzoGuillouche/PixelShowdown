using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCooldown : MonoBehaviour
{
    private string dashName;
    // Start is called before the first frame update
    void Start()
    {
        dashName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(dashName == "Dash1" ? GutsActions.dashCooldown1*3 : GutsActions.dashCooldown2*3, transform.localScale.y);
    }
}
