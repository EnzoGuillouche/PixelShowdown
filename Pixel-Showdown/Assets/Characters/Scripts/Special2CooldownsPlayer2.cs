using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special2CooldownsPlayer2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(GutsActions2.spe2Cooldown, transform.localScale.y);
    }
}
