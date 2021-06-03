using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager current;

    // store UDP input
    public int popularLabel = 0;
    public string testLabel = "check";
    public int inputState = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        // legacy: String.Equals(latestPacket, "0000003000000", StringComparison.OrdinalIgnoreCase)
        // to check label directly from padded packet
        
        if (Input.GetKeyDown("w") || (popularLabel== 2) || (testLabel == "2"))
        {
            inputState = 1;
        }
        else if (Input.GetKeyDown("s") || (popularLabel == 3) || (testLabel == "3"))
        {
            inputState = 2;
        }
        /* else if (Input.GetKeyDown("d") || (popularLabel == 4) || (testLabel == "4"))
        {
            inputState = 3;
        }
        else if (Input.GetKeyDown("a") || (popularLabel == 5) || (testLabel == "5"))
        {
            inputState = 4;
        }
        else if (Input.GetKeyDown("e") || (popularLabel == 6) || (testLabel == "6"))
        {
            inputState = 5;
        }
        else if (Input.GetKeyDown("q") || (popularLabel == 7) || (testLabel == "7"))
        {
            inputState = 6;
        } */
    }

}
