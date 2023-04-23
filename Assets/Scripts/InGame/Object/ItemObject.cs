using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent (typeof (Renderer))]
public class ItemObject : MonoBehaviour
{
    private const short OUTLINE_COLORNUM = 3;
    private static Material[] outlines;

    private new Renderer renderer;

    private XRGrabInteractable grabInteractable;
    private List<Material> materials = new List<Material>();
    private List<Material> defaultMaterials = new List<Material>();
    public LineType lineType;
    public LineCode lineCode;
    public bool isActive, isCanGet;
    public int itemCode;

    // Start is called before the first frame update
    void Start()
    {
        if (outlines == null)
            outlines = Resources.LoadAll<Material>("3D_Model/Outline/Materials");

        renderer = this.GetComponent<Renderer>();
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
            grabInteractable = this.gameObject.AddComponent<XRGrabInteractable>();

        defaultMaterials.AddRange(renderer.sharedMaterials);
        for (int i = 0; i < defaultMaterials.Count; i++)
        {
            if (defaultMaterials[i].shader.name == Shader.Find("Outlines/BackFaceOutlines").name)
                defaultMaterials.Remove(GetOutlineMaterial(lineType, LineCode.IDLE));
        }

        SetActive(isActive);

        grabInteractable.hoverEntered.AddListener(Interaction_Enter);
        grabInteractable.hoverExited.AddListener(Interaction_Exit);
        grabInteractable.selectEntered.AddListener(Select_Enter);
        grabInteractable.trackPosition = grabInteractable.trackRotation = false;
    }

    public void Interaction_Enter(HoverEnterEventArgs args)
    {
        if (!isActive)
            return; 

        lineCode = GetLineCode();
        SetMaterials(GetOutlineMaterial(lineType, lineCode));
    }

    public void Interaction_Exit(HoverExitEventArgs args)
    {
        if (!isActive)
            return;

        lineCode = LineCode.IDLE;
        SetMaterials(GetOutlineMaterial(lineType, lineCode));
    }

    public void Select_Enter(SelectEnterEventArgs args)
    {
        if (!isActive)
            return;

        if (!isCanGet)
        {
            MapCtrl.instance.SetAudio(1);
            return;
        }

        MapCtrl.instance.SetAudio(0);
        PlayerCtrl.instance.hasPuzzles[itemCode] = true;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Idle Outline(��� ������)�� ON/OFF �ϴ� �Լ�
    /// </summary>
    /// <param name="_isActive">Outline�� ON �� ���ΰ�?</param>
    public void SetActive(bool _isActive)
    {
        isActive = _isActive;
        lineCode = LineCode.IDLE;

        if (isActive)
        {
            SetMaterials(GetOutlineMaterial(lineType, lineCode));
        }
    }

    /// <summary>
    /// Default Materials�� (_add) Material�� �߰��Ͽ� Renderer�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="_add"></param>
    private void SetMaterials(Material _add)
    {
        materials.Clear();
        materials.AddRange(defaultMaterials);
        if (_add != null)
            materials.Add(_add);
        renderer.materials = materials.ToArray();
    }

    /// <summary>
    /// ���� ���¿� ���� LineCode �� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    private LineCode GetLineCode()
    {
        if (isCanGet) return LineCode.CLICKED_CAN;
        else return LineCode.CLICKED_CANNOT;
    }

    /// <summary>
    /// ���� ������ ������ ���¿� �´� Material�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="_lineType">������ ����</param>
    /// <param name="_lineCode">������ ����</param>
    /// <returns></returns>
    static private Material GetOutlineMaterial(LineType _lineType, LineCode _lineCode)
    {
        return outlines[OUTLINE_COLORNUM * ((short)_lineType) + ((short)_lineCode)];
    }
}
