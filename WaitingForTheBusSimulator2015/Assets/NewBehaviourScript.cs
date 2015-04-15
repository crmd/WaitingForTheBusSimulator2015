using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	public RenderTexture rt;
	public Camera cameraInst;
	public GameObject phone;
//	Material mat = new Material;
	void Start() {
		rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
		rt.Create();
		cameraInst = /*transform.parent.*/GetComponent<Camera> ();
		cameraInst.targetTexture = rt;
//		mat.SetTexture ("rt", rt);
		phone.renderer.material.mainTexture = rt;
	}
}
