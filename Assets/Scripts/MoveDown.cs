﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{



    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -.1f, 0);
    }


}
