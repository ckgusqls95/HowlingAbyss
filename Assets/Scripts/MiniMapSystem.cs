using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MiniMapSystem : MonoBehaviour
{
    [SerializeField]
    private GraphicRaycaster GR;
    PointerEventData EventData;

    public Rect ActualMapsize;
    [SerializeField]
    GameObject Icon = null;

    public Dictionary<GameObject, GameObject> observers;

    [SerializeField]
    private Vector3 offset = new Vector3(-10, 100, 0);
    private void Awake()
    {
        EventData = new PointerEventData(null);

        GR = GetComponentInChildren<GraphicRaycaster>();
        observers = new Dictionary<GameObject, GameObject>();

        ActualMapsize.width = 175;
        ActualMapsize.height = 175;
        ActualMapsize.x = -25;
        ActualMapsize.y = -25;

    }

    private void FixedUpdate()
    {
        foreach (var obj in observers)
        {
            GameObject key = obj.Key;
            Vector2 position = IndicateObject(key.transform.position);
            obj.Value.transform.localPosition = new Vector3(position.x, position.y, 0);
        }
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            EventData.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            GR.Raycast(EventData, result);

            if (result.Count > 0)
            {
                moveCamera(Input.mousePosition, result[0].gameObject.transform.position);
            }
        }
    }

    GameObject IndicateObject(GameObject obj, Color color, GameObject SelectIcon = null)
    {
        // -51.1 198.9   0 250
        // w - xMax = 51.1 이만큼 더해줌
        // h - yMax = ? 만큼 더해줌  0 ~ 250
        //  78% ->  0 390 ->  296 - 190 106.4
        Rect MiniMapSize = GetComponent<RectTransform>().rect;

        Vector2 IconPosition = new Vector2(obj.transform.position.x, obj.transform.position.z);
        IconPosition.x += Mathf.Abs(ActualMapsize.xMin);
        IconPosition.y += Mathf.Abs(ActualMapsize.yMin);
        float x = (IconPosition.x / ActualMapsize.width) * MiniMapSize.width;
        float y = (IconPosition.y / ActualMapsize.height) * MiniMapSize.height;
        x -= Mathf.Abs(MiniMapSize.xMin);
        y -= Mathf.Abs(MiniMapSize.yMin);

        GameObject icon =
         SelectIcon == null ? Instantiate(Icon, Vector3.zero, Quaternion.identity, this.transform) :
                              Instantiate(SelectIcon, Vector3.zero, Quaternion.identity, this.transform);

        Image iconImage = icon.GetComponent<Image>();
        if(obj.TryGetComponent<Champion>(out var script))
        {

        }
        else
        {
            iconImage.color = color;
        }
        
        icon.transform.localPosition = new Vector3(x, y, 0);

        return icon;
    }

    Vector2 IndicateObject(Vector3 position)
    {
        Rect MiniMapSize = GetComponent<RectTransform>().rect;

        Vector2 IconPosition = new Vector2(position.x, position.z);
        IconPosition.x += Mathf.Abs(ActualMapsize.xMin);
        IconPosition.y += Mathf.Abs(ActualMapsize.yMin);
        float x = (IconPosition.x / ActualMapsize.width) * MiniMapSize.width;
        float y = (IconPosition.y / ActualMapsize.height) * MiniMapSize.height;

        x -= Mathf.Abs(MiniMapSize.xMin);
        y -= Mathf.Abs(MiniMapSize.yMin);

        return new Vector2(x, y);
    }

    public void Attach(GameObject observer, GameObject Icon = null)
    {
        observers.Add(observer,
            IndicateObject(
                observer,
                observer.transform.tag == "Red" ? new Color(255, 0, 0) : new Color(0, 0, 255),
                Icon != null ? Icon : null));
    }

    public void Dettach(GameObject observer)
    {
        if (observers.ContainsKey(observer))
        {
            if (observers[observer])
                Object.Destroy(observers[observer]);

            observers.Remove(observer);
        }
    }

    public void moveCamera(Vector3 mousepos, Vector3 centerpos)
    {
        Rect MiniMapSize = GetComponent<RectTransform>().rect;

        Vector2 pos = new Vector2((mousepos.x - centerpos.x), (mousepos.y - centerpos.y));

        pos.x += Mathf.Abs(MiniMapSize.xMin); // range -180~180 => 0~380
        pos.y += Mathf.Abs(MiniMapSize.yMin);

        pos.x = (pos.x / MiniMapSize.width) * ActualMapsize.width;
        pos.y = (pos.y / MiniMapSize.height) * ActualMapsize.height;

        pos.x -= Mathf.Abs(ActualMapsize.xMin);
        pos.y -= Mathf.Abs(ActualMapsize.yMin);

        Camera.main.transform.position = new Vector3(pos.x + offset.x, offset.y, pos.y + offset.z);
        return;
    }
}
