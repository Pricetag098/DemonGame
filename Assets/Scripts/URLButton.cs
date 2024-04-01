using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLButton : MonoBehaviour
{

    public void URLLink(string link)
    {
        Application.OpenURL(link);
    }
}
