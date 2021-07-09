using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cardFlipper : MonoBehaviour
{
    public GameObject cardBack;

    public void FlipCard(bool enable)
    {
        cardBack.SetActive(enable);
    }
}
