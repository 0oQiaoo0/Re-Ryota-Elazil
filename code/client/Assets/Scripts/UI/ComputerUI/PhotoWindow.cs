using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PhotoWindow : WindowLogic,IWindowParams<string>
    {
        public Image photo;
        
        public void OnInitArg(string arg1)
        {
            if (photo == null)
                return;
            
            photo.sprite = Resources.Load<Sprite>(arg1);
        }
    }
}