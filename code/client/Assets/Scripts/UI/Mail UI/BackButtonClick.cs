using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonClick : ButtonClick
{
    public GameObject NowInterface;
    public GameObject BackInterface;
    public void ClickBack()
    {
        BackInterface.SetActive(true);
        NowInterface.SetActive(false);
    }
}
