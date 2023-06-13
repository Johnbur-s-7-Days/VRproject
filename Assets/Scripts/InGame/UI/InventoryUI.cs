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

    public GameObject imagePrefab; 
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
        if(CollisionPuzzle.transform.tag.Equals("Puzzle"))
        {
            ItemObject puzzle = CollisionPuzzle.transform.GetComponent<ItemObject>();
            if (puzzle == null)
                return;

            GameObject newImage = Instantiate(imagePrefab, _content);

            AudioSource.PlayClipAtPoint(DropItemClip, newImage.transform.position);

            Image imageComponent = newImage.GetComponent<Image>();
            if (imageComponent != null)
            {
                string Puzzlename = "Puzzle" + puzzle.itemCode;
                Sprite puzzleimg = Resources.Load<Sprite>("Image/Puzzle/" + Puzzlename);
                if (puzzleimg != null)
                {
                    imageComponent.sprite = puzzleimg;
                    imageComponent.name = Puzzlename;
                }
            }

            Destroy(puzzle.gameObject);
        }
       
    }


    public void GetItem()
    {
        //�ƹ��͵� ���� ������ ����.
        if (_content.childCount == 0) return;
        else 
        {
            RectTransform currentImg = GetCurrentChild(); // ���� ���̴� �̹���
            string currentImgName = currentImg.gameObject.name;
            
            //���� ��ġ ����
            Vector3 centerPos = currentImg.TransformPoint(currentImg.rect.center);
            centerPos.y += 0.3f;
            centerPos.x -= 0.4f;

            GameObject currentPuzzle = Resources.Load<GameObject>("Puzzles/" + currentImgName);
            if (currentPuzzle != null)
            {
                GameObject clonePuzzle = Instantiate(currentPuzzle, centerPos, Quaternion.identity);
                clonePuzzle.name = currentImgName;

                Rigidbody puzzleRb = clonePuzzle.GetComponent<Rigidbody>();
                if (puzzleRb != null) { puzzleRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }

                Destroy(currentImg.gameObject);
                Debug.Log("������ ����");
            }

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
