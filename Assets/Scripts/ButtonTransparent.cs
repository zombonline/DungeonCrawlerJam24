using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTransparent : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = .6f;
    }
}
