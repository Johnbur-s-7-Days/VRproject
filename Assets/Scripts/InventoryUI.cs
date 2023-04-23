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
        //새로운 오브젝트 생성 
        var newGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newGO.transform.parent = new3DObjectParent.transform;

        // 크기 & 위치 설정
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        newGO.AddComponent<Rigidbody>();


        //색을 랜덤하게 설정
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
            GameObject newButton = Instantiate(buttonPrefab, buttonParent); // 새로운 버튼 생성
            newButton.transform.position = new Vector3(0, 0, 0); // 버튼 위치 설정
            newButton.name = "NewButton"; // 버튼 이름 설정

            Debug.Log("버튼 생성");
            Destroy(collision.gameObject); // 3D 오브젝트 삭제

            /*for (int i = 0; i < buttonImages.Length; i++)
            {
                if (buttonImages[i].gameObject.CompareTag(objectTag))
                {
                    //buttonImages[i].sprite = newButtonImage; // 새로운 이미지로 변경
                    Destroy(collision.gameObject); // 3D 오브젝트 삭제
                }
            }*/
        }
    }

}