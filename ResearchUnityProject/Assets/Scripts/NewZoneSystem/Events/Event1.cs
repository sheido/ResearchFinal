﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour {
	DetectableLocalManager DLM;

	bool tutorialFinished = false;
	bool pressed = false;
	bool pressed2 = false;
	bool artefactBloquer = false;

	//parametre d'animation
	public Animator anim;
	bool animation;
	GameObject Player;
	GameObject MainCamera;
	GameObject artefactSurFesses;
	GameObject artefactTuto;
	GameObject artefactSurUI;

	DeathSystem DS;

	// Use this for initialization
	void Start () {

		DS = GameObject.Find ("Player").GetComponent<DeathSystem> ();

		MainCamera = GameObject.Find ("Main Camera Main");
		Player = GameObject.Find ("Player");
		artefactTuto = GameObject.Find ("ArtefactTuto");
		artefactTuto.SetActive (false);
		artefactSurFesses = GameObject.Find ("ArtefactOnAss");
		artefactSurUI =  GameObject.Find ("artefactNewCanvasChaudFroid");
		DLM = GameObject.Find ("SmallItem1").GetComponent<DetectableLocalManager> ();
	}
	

	// Update is called once per frame
	void Update () {
	
		//demander au joueur d'appuyer sur submit (espace ou entrée)

		if (DLM.isPlayerHere && !tutorialFinished && Input.GetButtonDown ("Submit") && !pressed) {
			pressed = true;
			//launch tutorial
			StartCoroutine("animationQuiBloqueAMoitieLeCube");


			// pour récuperer le cube le joueur doit rappuyer sur submit (espace ou entrée)

			// pour récuperer le cube le joueur doit rappuyer sur submit (espace ou entrée)

			//launch anim ou le joueur ramasse le cube
		}

		if (artefactBloquer && Input.GetButtonDown ("Submit") && !pressed2) {
			tutorialFinished = true;

			artefactTuto.SetActive (false);
			artefactSurFesses.SetActive (true);
			artefactSurUI.SetActive (true);

			DS.tutoMort = false;

			pressed2 = true;
		}
	}

	IEnumerator animationQuiBloqueAMoitieLeCube () {
		yield return new WaitForSeconds (0.1f);
		//
		animation = true;
		artefactTuto.SetActive (true);


		anim.SetBool("AnimActiv",animation);
		MainCamera.SetActive(false);
		Player.SetActive(false);


		//fin animation
		yield return new WaitForSeconds (3.5f);

		artefactBloquer = true;

		MainCamera.SetActive(true);
		Player.SetActive(true);

		DS.tutoMort = true;

		artefactSurFesses.SetActive (false);
		artefactSurUI.SetActive (false);
	}
}
