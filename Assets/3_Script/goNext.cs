using UnityEngine;
using System.Collections;

public class goNext : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public GameObject prefab;
	Vector3 pos = new Vector3 (0, 0, 0);
	static int cnt = 0;
	//public GameObject Player;
	// Update is called once per frame
	void OnTriggerExit(Collider col){
		GameObject map;
		string name = "building";
		if(col.gameObject.tag == "Player"){
			pos.x=-30*(cnt+1);
			map = Instantiate(prefab,pos,Quaternion.Euler(0, 0, 0))as GameObject; 
			map.name = name+cnt.ToString();
			cnt++;
			Debug.Log (cnt);
			Debug.Log(pos.x);
			Object.Destroy(GetComponent<goNext>());
		}
	}
}
