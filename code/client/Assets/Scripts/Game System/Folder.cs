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
        GetComponent<Image>().sprite = Resources.Load<Sprite>("��һ����Դ/����/�ϰ�С��Ϸsprite/WenJianJia_2");
    }

    public void FloderClose()
    {
        isEmpty = false;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("��һ����Դ/����/�ϰ�С��Ϸsprite/WenJianJia_1");
    }

    public void FolderReset()
    {
        isEmpty = true;
        GetComponent<Image>().sprite = Resources.Load<Sprite>("��һ����Դ/����/�ϰ�С��Ϸsprite/WenJianJia_2");
    }
}
