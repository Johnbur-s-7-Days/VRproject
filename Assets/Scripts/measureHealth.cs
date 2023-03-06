using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class measureHealth : MonoBehaviour
{
    // Start is called before the first frame update
    
    private float time = 15f; // ½Ã°£.
    public TMP_Text secnum;
    public GameObject measureUIObject;

    void Start()
    {
        time = 15f;
        this.secnum.text = string.Format("{0}",this.time);
        this.StartCoroutine(this.waitMeasure());
    }

    private IEnumerator waitMeasure()
    {
        float delta = this.time;
        while(true)
        {
            delta -= Time.deltaTime;
            Debug.Log(delta);
            this.secnum.text = "( ";
            this.secnum.text += string.Format ("{0}",(int)delta);
            this.secnum.text += " sec )";
            if (delta <= 0)
            {
                measureUIObject.SetActive(false);
                break;
            }
            
            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
}
