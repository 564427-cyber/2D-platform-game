using System;
using UnityEngine;

public class HelperScript : MonoBehaviour
{    
    public void DoFlipObject( bool flip)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (flip == true)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
    public void DoSpeech()
    {
        if (Input.GetKey("h"))
        {
            print("Hello World!");
        }
    }
}