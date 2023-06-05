using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public int itemCode;

    // Start is called before the first frame update
    void Start()
    {
        /*
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
            grabInteractable = this.gameObject.AddComponent<XRGrabInteractable>();

        grabInteractable.hoverEntered.AddListener(Interaction_Enter);
        grabInteractable.hoverExited.AddListener(Interaction_Exit);
        grabInteractable.selectEntered.AddListener(Select_Enter);
        grabInteractable.trackPosition = grabInteractable.trackRotation = false;
        */
    }

    public void Interaction_Enter(HoverEnterEventArgs args)
    {

    }

    public void Interaction_Exit(HoverExitEventArgs args)
    {

    }

    public void Select_Enter(SelectEnterEventArgs args)
    {
        // MapCtrl.instance.SetAudio(0);
        // PlayerCtrl.instance.hasPuzzles[itemCode] = true;
        // Destroy(this.gameObject);
    }

    
}
