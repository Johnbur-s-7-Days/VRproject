// Script name: InventoryUI
// Script purpose: attaching a gameobject to a certain anchor and having the ability to enable and disable it.
// This script is a property of Realary, Inc

using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject Inventory;

    [SerializeField]
    private GameObject new3DObjectParent;

    public Image[] buttonImages;

    public GameObject buttonPrefab;

    [SerializeField]
    public Transform buttonParent; 


    // menuState
    [SerializeField]
    private InteractableUnityEventWrapper onAction;
    private bool isOn = true;

    private void Awake()
    {
        /*onAction.WhenSelect.AddListener(() =>
        {
            isOn = !isOn;
            var InventoryOption = onAction.GetComponentInChildren<TextMeshPro>();
            var InventoryState = isOn ? "ON" : "OFF";
            InventoryOption.text = $"Inventory {InventoryState}";
           
        });*/
    }

    public void InventoryVisibility(bool state)
    {
        Inventory.SetActive(state);
    }

    public void Add3DObject()
    {
        //���ο� ������Ʈ ���� 
        var newGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newGO.transform.parent = new3DObjectParent.transform;

        // ũ�� & ��ġ ����
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        newGO.AddComponent<Rigidbody>();


        //���� �����ϰ� ����
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.SetColor("_Color", Random.ColorHSV());
        newGO.GetComponent<MeshRenderer>().material = newMaterial;
       

        var colliderSurface = newGO.AddComponent<ColliderSurface>();
        colliderSurface.InjectCollider(newGO.GetComponent<Collider>());

    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Puzzle"))
        {
            string objectTag = collision.collider.gameObject.tag;
            Debug.Log("Puzzle");
            GameObject newButton = Instantiate(buttonPrefab, buttonParent); // ���ο� ��ư ����
            newButton.transform.position = new Vector3(0, 0, 0); // ��ư ��ġ ����
            newButton.name = "NewButton"; // ��ư �̸� ����

            Debug.Log("��ư ����");
            Destroy(collision.gameObject); // 3D ������Ʈ ����

            /*for (int i = 0; i < buttonImages.Length; i++)
            {
                if (buttonImages[i].gameObject.CompareTag(objectTag))
                {
                    //buttonImages[i].sprite = newButtonImage; // ���ο� �̹����� ����
                    Destroy(collision.gameObject); // 3D ������Ʈ ����
                }
            }*/
        }
    }

}