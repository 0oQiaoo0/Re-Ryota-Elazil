using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Folder : MonoBehaviour
{
    public bool isEmpty;

    private void Awake()
    {
        isEmpty = true;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("第一日资源/美术/老板小游戏sprite/WenJianJia_2");
    }

    public void FloderClose()
    {
        isEmpty = false;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("第一日资源/美术/老板小游戏sprite/WenJianJia_1");
    }

    public void FolderReset()
    {
        isEmpty = true;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("第一日资源/美术/老板小游戏sprite/WenJianJia_2");
    }
}
