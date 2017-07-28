using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MovieScene : MonoBehaviour
{
    void Update()
    {
        if (InputAnyKey())
        {
            SceneManager.LoadScene("title");
        }
    }
    // 何らかの入力があればtrue、無ければfalse を返却する
    private bool InputAnyKey()
    {
        return Input.anyKey ||
        Mathf.Abs(Input.GetAxis("Horizontal")) > 0.3f ||
        Mathf.Abs(Input.GetAxis("Vertical")) > 0.3f ||
        Mathf.Abs(Input.GetAxis("Mouse X")) > 0 ||
        Mathf.Abs(Input.GetAxis("Mouse Y")) > 0 ||
        Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0;
    }
}
