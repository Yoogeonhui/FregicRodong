using UnityEngine;
using System.Collections;

public class dooropen : MonoBehaviour {
    public int roomnumber,doornumber;
    public GameObject floorprefab;

    public float floorscale,buildingscale;
    Transform mydoor;
    GameObject sys,building;
    Vector3 beftransform;
    Vector3 originrot;
    float startmoving=-1.0f;
    bool isfirst=true,isexit = false,isreallyfirst=true;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag=="Player")
        {
            
            isexit = true;
            isfirst = true;
            startmoving = Time.time;
            Vector3 rotatedangle = this.transform.rotation.eulerAngles;
            float asdf = rotatedangle.y;
            copyroom(asdf, Roomnumber.move((int)asdf / 90,roomnumber), isreallyfirst);
            if (isreallyfirst) isreallyfirst = false;
        }
    }
    void copyroom(float asdf,bool roommake,bool floormake)
    {
        Vector3 relativedirection;
        switch ((int)asdf / 90)
        {
            case 0:
                //x+ㅁㄴㅇㄹ
                relativedirection = new Vector3(1, 0, 0);
                break;
            case 1:
                //z-
                relativedirection = new Vector3(0, 0, -1);
                break;
            case 2:
                //x-
                relativedirection = new Vector3(-1, 0, 0);
                break;
            default:
                relativedirection = new Vector3(0, 0, 1);
                //z+
                break;
        }
        
        floorscale = 10f;
        buildingscale = 20f;
        Vector3 qwer = GameObject.Find("Floor_" + roomnumber.ToString()).transform.position;
        Vector3 floorloc = qwer + relativedirection * (floorscale + buildingscale) / 2;
        int nowrooms;
        if (roommake)
        {
            Vector3 buildingloc = GameObject.Find("Building_" + roomnumber.ToString()).transform.position + relativedirection * (buildingscale + floorscale);
            nowrooms = (++Roomnumber.roomnumber);
            //생성 및 이름바꿈 ㅁㄴㅇㄹ
            
            GameObject created = (GameObject)Instantiate(building, buildingloc, Quaternion.Euler(Vector3.zero));
            created.name = "Building_" + nowrooms.ToString();
            GameObject.Find("Floor").name = "Floor_" + nowrooms;
            for (int i = 0; i < 4; i++)
            {
                
                GameObject resetplz = GameObject.Find("trigger_" + i);
                resetplz.name = "trigger_" + nowrooms + "_" + i;
                resetplz.GetComponent<dooropen>().roomnumber = nowrooms;
                if (i == ((int)asdf / 90 + 2) % 4)
                  resetplz.GetComponent<dooropen>().isreallyfirst = false;
                GameObject.Find("Door_" + i).name = "Door_" + nowrooms + "_" + i;
            }
        }
        if (floormake)
        {
            floorloc -= new Vector3(0, 0.15f, 0);
            Instantiate(floorprefab, floorloc, Quaternion.Euler(0, asdf, 0));
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            isexit = false;
            isfirst = true;
            startmoving = Time.time;
        }
    }
    float rotation(float from, float to, float time)
    {
        return from + (to - from) * ((time>=0f&&time<=1f)?Mathf.Sin(time / 2 * Mathf.PI):(time<0f)?0f:1f);
    }
	// Use this for initialization
	void Start () {
        building = (GameObject) Resources.Load("Building");
        originrot = transform.rotation.eulerAngles;
        sys = GameObject.Find("System");
        mydoor = GameObject.Find("Door_" + roomnumber.ToString() + "_" + doornumber.ToString()).transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - startmoving <= 1.0f)
        {
            if (isfirst)
            {
                beftransform = mydoor.localRotation.eulerAngles;
                isfirst = false;
            }
            float result;
            if (isexit)
                result = rotation(beftransform.y, originrot.y+90f, Time.time - startmoving);
            else
                result = rotation(beftransform.y, originrot.y+0f, Time.time - startmoving);
            mydoor.localRotation = Quaternion.Euler(new Vector3(0, result, 0));
        }
	}
}
