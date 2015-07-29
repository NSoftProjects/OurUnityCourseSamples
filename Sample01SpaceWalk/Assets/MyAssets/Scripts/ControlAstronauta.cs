using UnityEngine;
using System.Collections;




public class ControlAstronauta : MonoBehaviour {
	float maxCarga;
	[HideInInspector] public float calor = 0f;
	[HideInInspector] public float altura;
	[HideInInspector] public bool enAtmosfera = false;
	[HideInInspector] public bool ganarPartida = false;
	
	
	
	public float fuerzaDesplazamiento = 1f;
	public float fuerzaGiro = 0.06f;
	public float carga = 100f;
	public float oxigeno = 100f;
	public float consumoOxigeno = 2f;
	public float descarga = 0.04f;
	public float fuerzaEstabilizado = 0.6f;
	public Transform planeta;
	
	// Use this for initialization
	void Start () {
		maxCarga = carga;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//faltan los deltatime para los incrementos de temperatura!
		
		if (enAtmosfera )
			//calor += 0.01f;
			calor += 0.2f;
		else 
			calor -= 0.1f;
		//calor -= 0.005f;
		
		calor = Mathf.Clamp (calor, 0, 98);
		
		if (oxigeno > 0) {
			oxigeno -= Time.deltaTime * consumoOxigeno;
		}
		
		
		Vector3 direccion = new Vector3 ();
		direccion = planeta.position -transform.position;
		RaycastHit hit;
		Ray rayo = new Ray();
		
		rayo.origin = transform.position;
		rayo.direction = direccion;

		planeta.GetComponent<Collider>().Raycast (rayo, out hit, direccion.magnitude);
		
		altura = hit.distance;

		

	}
	
	void FixedUpdate(){
		if (carga > 0) {
			//acelerar
			if (Input.GetKey (KeyCode.W)) {
				GetComponent<Rigidbody>().AddForce (transform.forward * fuerzaDesplazamiento);
				carga -= descarga;
			}
			//frenar
			if (Input.GetKey (KeyCode.S)) {
				GetComponent<Rigidbody>().AddForce (-transform.forward * fuerzaDesplazamiento);
				carga -= descarga;
			}
			
			//guiñada derecha
			if (Input.GetKey (KeyCode.L)) {
				GetComponent<Rigidbody>().AddTorque (transform.up * fuerzaGiro);
				carga -= descarga;
			}
			//guiñada izquierda
			if (Input.GetKey (KeyCode.J)) {
				GetComponent<Rigidbody>().AddTorque (-transform.up * fuerzaGiro);
				carga -= descarga;
			}
			//alabeo izquierda 
			if (Input.GetKey (KeyCode.A)) {
				GetComponent<Rigidbody>().AddTorque (transform.forward * fuerzaGiro);
				carga -= descarga;
			}
			//alabeo derecha
			if (Input.GetKey (KeyCode.D)) {
				GetComponent<Rigidbody>().AddTorque (-transform.forward * fuerzaGiro);
				carga -= descarga;
			}
			
			// cabeceo abajo
			if (Input.GetKey (KeyCode.I)) {
				GetComponent<Rigidbody>().AddTorque (transform.right * fuerzaGiro);
				carga -= descarga;
			}
			//cabeceo arriba
			if (Input.GetKey (KeyCode.K)) {
				GetComponent<Rigidbody>().AddTorque (-transform.right * fuerzaGiro);
				carga -= descarga;
			}	
			
			//boton del panico
			if (Input.GetKey (KeyCode.Space)) {
				GetComponent<Rigidbody>().velocity = Vector3.Lerp (GetComponent<Rigidbody>().velocity, Vector3.zero, Time.fixedDeltaTime * fuerzaEstabilizado);			
				GetComponent<Rigidbody>().angularVelocity = Vector3.Lerp (GetComponent<Rigidbody>().angularVelocity, Vector3.zero, Time.fixedDeltaTime * fuerzaEstabilizado);
				carga -= descarga;
			}
		} else
			carga = Mathf.Clamp (carga, 0f, maxCarga);
	}



	public void GiroAleatorio(bool withStop){
		Vector3 vectorAleatorio = new Vector3 (Random.value*3f, Random.value*3f, Random.value*3f);
		
		if (withStop)
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().AddTorque (vectorAleatorio);
	}
	
	void OnTriggerStay(Collider other){
		//if (other.name == "Planeta02Atmosfera") {
		//	calor += 0.01f	;
	}


	void OnTriggerEnter(Collider other){
		//Debug.Log (other.name);
		if (other.name == "Planet02Atmosfera") {
			enAtmosfera = true;
			
		}


		if (other.gameObject.tag == "Acceso"){
			ganarPartida = true;
			Debug.Log ("finpartida");
		}
		
	}
	
	void OnTriggerExit(Collider other){
		if (other.name == "Planet02Atmosfera")
			enAtmosfera = false;
	}
	
	


}




