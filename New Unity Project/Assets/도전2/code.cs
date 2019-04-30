using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class code : MonoBehaviour {
    public Vector3 target;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.X)) {
            gameObject.transform.localPosition = target;
        }
	}
    private void OnEnable()
    {
        
    }

}
