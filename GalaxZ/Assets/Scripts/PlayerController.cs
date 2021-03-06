using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour {

	CharacterController cc;
	Vector3 motion;

	public float sensitivity = 5;

	public Transform head;

	float headRotation = 0;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.up, Input.GetAxis ("Mouse X") * sensitivity);

		motion = new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		cc.Move (Collapse(transform) * (motion * 0.2f));

		headRotation += Input.GetAxis ("Mouse Y") * sensitivity;
		headRotation = Mathf.Clamp (headRotation, -60, 60);
		head.localRotation = Quaternion.AngleAxis (headRotation, Vector3.left);
	}

	T IsLookingAt<T>() where T : MonoBehaviour{
		Ray ray = new Ray (head.position, head.forward);
		RaycastHit info;
		if (Physics.Raycast (ray, out info, 3)) {
			Transform transformOfObject = info.collider.transform;
			T comp = transformOfObject.gameObject.GetComponent<T> ();
			while (comp == null) {
				transformOfObject = transformOfObject.parent;
				if (transformOfObject == null)
					break;
				comp = transformOfObject.gameObject.GetComponent<T> ();
			}
			return comp;
		}
		return null;
	}

	Quaternion Collapse (Transform t){
		return Quaternion.LookRotation (Vector3.Scale (t.forward, new Vector3 (1, 0, 1)), Vector3.up);
	}
}
