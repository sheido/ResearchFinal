﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

	//detection des zones.

	DetectableClass[] det;
	public GameObject[] T;
	GameObject Player;

	ArrayList isDetectedAround;

	public float distancePremierCercleDetection = 80.0f; // distancePremierCercleDetection
	public float distanceChaudFroid = 40.0f;
	public float distancePressE = 5.0f;

	public bool hotcold;

	float[] distance;

	float DistanceLaPlusProche;

	//systeme chaud froid.

	Animator anim;
	QT_SurfaceNoise2 QTSurfShad;

	//variable temporaire
	bool didIsendOnceToParticularObject = false;

	//gestion des sons

	//distance longue

	public AudioClip LongueDistanceDetection;

	//chaud froidt
	bool playSoundOnceHC;
	public AudioClip hotColdActivationSound;

	//appuyer sur e

	bool playSoundOncePressE;
	public AudioClip PressESound;

	void Start () {

		//detection des zones

		Player = GameObject.Find ("Player");
		T = GameObject.FindGameObjectsWithTag ("Detectable");

		det = new DetectableClass[T.Length];
		distance = new float[T.Length];

		isDetectedAround = new ArrayList();

		for (int i = 0; i < T.Length; i++) {

			//CircleRadar [i].SetActive (false);

			det [i] = new DetectableClass (
				T [i].transform.position, 
				T [i].name,
				false, false, false);

			//print (T [i].transform.GetChild(0).name);

			T [i].transform.GetChild(0).gameObject.SetActive (false);

			//det [i].Cercle.SetActive (false);
			distance [i] = Vector3.Distance (det [i].pos, Player.transform.position);
			//print (distance [i] + det[i].Name);
		}


		//check si l'objet le plus proche est a moins de distancePremierCercleDetection
		DistanceLaPlusProche = (Mathf.Min (distance));
		//print (Array.IndexOf(distance, Mathf.Min(distance)));

		StartCoroutine ("CheckEveryHalfSec");

		//systeme chaud froid

		anim =  GameObject.Find ("ArtefactAnimation").GetComponent<Animator>();
		QTSurfShad = GameObject.Find ("artefactNewCanvasChaudFroid").GetComponent<QT_SurfaceNoise2>();

	}

	IEnumerator CheckEveryHalfSec () {
		yield return new WaitForSeconds (0.5f);

		for (int i = 0; i < T.Length; i++) {
			distance [i] = Vector3.Distance (det [i].pos, Player.transform.position);
			//print (distance [i] + det[i].Name);

			//check si c'est a moins de distancePremierCercleDetection
			if (distance [i] < distancePremierCercleDetection) {
				if (!isDetectedAround.Contains (det [i])) {
					isDetectedAround.Add (det [i]);
					print ("added smthg to array");
					print (det [i].Name);

					//det [i].isDetectedFarCircle = true;

					//Envoyer au détectable spécifique le fait qu'il a été vu de loin 

					GameObject.Find (det[i].Name).GetComponent<DetectableLocalManager>().ImDetectedFar();
					T[i].transform.GetChild(0).gameObject.SetActive (true);

					//play sound
					AudioSource AS;
					AS = GameObject.Find ("Sound1").GetComponent<AudioSource> ();
					AS.PlayOneShot(LongueDistanceDetection);

					//ajouter a la carte l'info comme quoi un nouvel objet est apparu
					// faire briller l'icone de la map

				}
			}



			if (distance[i] <= distancePressE) {
				GameObject.Find (det[i].Name).GetComponent<DetectableLocalManager>().YouCanPressE();
			}

			if (distance[i] > distancePressE) {
				GameObject.Find (det[i].Name).GetComponent<DetectableLocalManager>().ICantPressEAnyMore();
			}

			//check si c'est a moins de distancePremierCercleDetection


		}

		//calcul chaud froid par rapport a l'objet le plus proche
		DistanceLaPlusProche = (Mathf.Min (distance));
//		print (Array.IndexOf(distance, Mathf.Min(distance)));
		//print (DistanceLaPlusProche);

		//systeme de chaud froid

		if (DistanceLaPlusProche < distanceChaudFroid) {
			ActiverChaudFroid ();
		} else {
			DesactiverChaudFroid ();
		}

		//systeme lacher le cube

		if (DistanceLaPlusProche < distancePressE) {
			//global action comme le son
			ActionDisponibleLacherCube ();
			//envoyer au local



		} else {
			DesactiverActionDisponibleLacherCube ();
		}


		StartCoroutine ("CheckEveryHalfSec");
	}
	//il faut que je check par rapport a la plus proche le chaud froid
	//il faut que je check globalement si une apparait a moins de 50m

	//Systeme chaud froid

	void ActiverChaudFroid()
	{
		if (!playSoundOnceHC) {
			AudioSource AS;
			AS = GameObject.Find ("Sound1").GetComponent<AudioSource> ();
			AS.PlayOneShot(hotColdActivationSound);
			playSoundOnceHC = true;
		}


		QTSurfShad.scaleModifier = (distanceChaudFroid - DistanceLaPlusProche)/4;
		QTSurfShad.speedModifier = (distanceChaudFroid - DistanceLaPlusProche)/4;
		QTSurfShad.noiseStrength = (distanceChaudFroid - DistanceLaPlusProche) / distanceChaudFroid/2;

		anim.speed = (distanceChaudFroid - DistanceLaPlusProche) / (distanceChaudFroid/4);
	}

	public void DesactiverChaudFroid()
	{
		QTSurfShad.scaleModifier = 0;
		QTSurfShad.speedModifier = 0;
		QTSurfShad.noiseStrength = 0;
		anim.speed = 1.0f;

		playSoundOnceHC = false;
	}

	//systeme déposer le cube lorsqu'on est dans une zone propice.

	void ActionDisponibleLacherCube()
	{
		//son

		if (!playSoundOncePressE) {
			AudioSource AS;
			AS = GameObject.Find ("Sound1").GetComponent<AudioSource> ();
			AS.PlayOneShot(PressESound);
			playSoundOncePressE = true;
		}
		//bouton "Press E" apparaitre a l'écran

	}

	void DesactiverActionDisponibleLacherCube()
	{
		playSoundOncePressE = false;
		// Bouton press E disparaitre de l'écran
	}

}