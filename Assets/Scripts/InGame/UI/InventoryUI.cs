using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oculus.Interaction;



public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame 
    
    [SerializeField]
    private RectTransform _viewport;

    [SerializeField]
    private RectTransform _content;

    [SerializeField]
    private AnimationCurve _easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public int CurrentChildIndex => _currentChildIndex;

    public RectTransform ContentArea => _content;

    private int _currentChildIndex = 0;
    private float _scrollVal = 0;

    public GameObject imagePrefab; // 생성할 이미지의 프리팹
    public float jumpForce = 1f;

    public AudioClip DropItemClip; 

    void Start()
    {
        this.AssertField(_viewport, nameof(_viewport));
        this.AssertField(_content, nameof(_content));
    }

    private RectTransform GetCurrentChild()
    {
        return _content.GetChild(_currentChildIndex) as RectTransform;
    }

    public void ScrollRight()
    {
        if (_content.childCount <= 1)
        {
            return;
        }
        else if (_currentChildIndex > 0)
        {
            RectTransform currentChild = GetCurrentChild();
            _content.GetChild(0).SetAsLastSibling();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
            ScrollToChild(currentChild, 1);
        }
        else
        {
            _currentChildIndex++;
        }
        _scrollVal = Time.time;
    }

    private void ScrollToChild(RectTransform child, float amount01)
    {
        if (child == null)
        {
            return;
        }

        amount01 = Mathf.Clamp01(amount01);

        Vector3 viewportCenter = _viewport.TransformPoint(_viewport.rect.center);
        Vector3 imageCenter = child.TransformPoint(child.rect.center);
        Vector3 offset = imageCenter - viewportCenter;

        if (offset.sqrMagnitude > float.Epsilon)
        {
            Vector3 targetPosition = _content.position - offset;
            float lerp = Mathf.Clamp01(_easeCurve.Evaluate(amount01));
            _content.position = Vector3.Lerp(_content.position, targetPosition, lerp);
        }
    }

    //DropItem
    private void OnTriggerEnter(Collider CollisionPuzzle)
    {
        if(CollisionPuzzle.gameObject.CompareTag("Puzzle"))
        {
            for (int index = 0; index < 5; index++)
            {
                string Puzzlename = "Puzzle" + index;
                if (CollisionPuzzle.name == Puzzlename)
                {
                    GameObject newImage = Instantiate(imagePrefab, _content);

                    AudioSource.PlayClipAtPoint(DropItemClip, newImage.transform.position);

                    // 생성된 이미지 오브젝트의 Image 컴포넌트에 이미지를 설정
                    Image imageComponent = newImage.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        Sprite puzzleimg = Resources.Load<Sprite>("Image/Puzzle/" + Puzzlename);
                        if (puzzleimg != null)
                        {
                            imageComponent.sprite = puzzleimg;
                            imageComponent.name = Puzzlename;
                        }
                    }
                }
            }
        }
        Destroy(CollisionPuzzle.gameObject);
    }


    public void GetItem()
    {
        //아무것도 담긴게 없으면 리턴.
        if (_content.childCount == 0) return;
        else 
        {
            RectTransform currentImg = GetCurrentChild(); // 현재 보이는 이미지
            string currentImgName = currentImg.gameObject.name;
            

            //생성 위치 설정
            Vector3 centerPos = currentImg.TransformPoint(currentImg.rect.center);
            centerPos.y += 0.5f;
            centerPos.x += 0.5f;

            GameObject currentPuzzle = Resources.Load<GameObject>("Puzzles/" + currentImgName);
            GameObject clonePuzzle = Instantiate(currentPuzzle, centerPos, Quaternion.identity);
            clonePuzzle.name = currentImgName;

            Rigidbody puzzleRb = clonePuzzle.GetComponent<Rigidbody>();
            if (puzzleRb != null) { puzzleRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }

            Destroy(currentImg.gameObject);
            Debug.Log("아이템 삭제");

        }
    }



    protected virtual void Update()
    {
        _currentChildIndex = Mathf.Clamp(
            _currentChildIndex, 0, _content.childCount - 1);

        bool hasImages = _content.childCount > 0;
        if (hasImages)
        {
            RectTransform currentImage = GetCurrentChild();
            ScrollToChild(currentImage, Time.time - _scrollVal);
        }
    }
}
