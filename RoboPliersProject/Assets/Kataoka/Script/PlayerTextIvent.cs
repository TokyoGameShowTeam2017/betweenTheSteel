using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTextIvent : MonoBehaviour
{

    [SerializeField, Tooltip("テキストスピード")]
    public float m_TextScroolSpeed;
    [SerializeField, Tooltip("声")]
    public AudioClip m_Voice;
    [SerializeField, Tooltip("流すテキスト"), TextArea(1, 20)]
    public string m_Text;
    //プレイヤーテキスト
    private TutorealText mTutorealText;
    // Use this for initialization
    void Start()
    {
        mTutorealText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mTutorealText.SetText(m_Text);
            mTutorealText.SetTextScroolSpeed(m_TextScroolSpeed);
            if (m_Voice != null)
                SoundManager.Instance.PlaySe(m_Voice.name);
            Destroy(gameObject);
        }
    }

}
