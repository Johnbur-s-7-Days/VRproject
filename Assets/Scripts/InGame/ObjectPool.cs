using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Transform objectTr;

    public GameObject npc; // 0

    Queue<NPC> NPC_queue = new Queue<NPC>();
    
    private T CreateNewObject<T>(int type, Transform tr, Vector3 pos) where T : MonoBehaviour
    {
        T newObj = null;

        switch (type)
        {
            case 0: newObj = Instantiate(npc, pos, Quaternion.identity, tr).GetComponent<T>(); break;
        }

        newObj.gameObject.SetActive(false);
        return newObj;
    }

    public T GetObject<T>(int type, Transform tr, Vector3 pos) where T : MonoBehaviour
    {
        int count = GetCount(type);
        if (count > 0)
        {
            T obj = null;

            switch (type)
            {
                case 0: obj = NPC_queue.Dequeue().GetComponent<T>(); break;
            }

            obj.transform.SetParent(tr);
            obj.transform.position = pos;
            obj.gameObject.SetActive(true);

            return obj;
        }
        else
        {
            var newObj = CreateNewObject<T>(type, tr, pos);
            newObj.transform.SetParent(tr);
            newObj.transform.position = pos;
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public void ReturnObject<T>(int type, T obj) where T : MonoBehaviour
    {
        if (obj == null)
        {
            Debug.LogError("Return Object is Failed.");
            return;
        }
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(this.transform);

        switch (type)
        {
            case 0: NPC_queue.Enqueue(obj.GetComponent<NPC>()); break;
        }
    }

    private int GetCount(int type)
    {
        int count = 0;

        switch (type)
        {
            case 0: count = NPC_queue.Count; break;
        }

        return count;
    }
}
