using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameTag : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    public GameObject player;
    public new string name;
    public bool isOn;

    void Start()
    {
        player = InGameController.instance.playerCtrl.gameObject;
        SetNameText(name);
        SetNameUI(false);
    }

    void Update()
    {
        if (!isOn) return;

        LookAtPlayer();
    }

    public void SetNameUI(bool _isOn)
    {
        isOn = _isOn;
        nameText.gameObject.SetActive(isOn);
    }

    public void SetNameText(string _name)
    {
        name = _name;
        nameText.text = name;
    }

    private void LookAtPlayer()
    {
        Vector3 lookVec = this.transform.position - player.transform.position;
        lookVec = new Vector3(lookVec.x, 0f, lookVec.z);
        nameText.transform.forward = lookVec.normalized;
    }
}
