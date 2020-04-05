using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public float distance = 8; //横向角度
    public float rot = 0; //纵向角度
    public float roll;
    public float soomth = 2f;
    [SerializeField]
    private GameObject target;

    Vector3 targetPos;
    Vector3 cameraPos;


    void Awake()
	{
		
	}
	
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(target==null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        if (target == null)
        {
            return;
        }
        if (Camera.main == null)
        {
            return;
        }
        targetPos = target.transform.position;
        float d = distance * Mathf.Cos(roll);
        float height = distance * Mathf.Sin(roll);
        cameraPos.x = targetPos.x + d * Mathf.Cos(rot);
        cameraPos.z = targetPos.z + d * Mathf.Sin(rot);
        cameraPos.y = targetPos.y + height;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPos, soomth * Time.deltaTime);
        Camera.main.transform.LookAt(target.transform);
    }
}
