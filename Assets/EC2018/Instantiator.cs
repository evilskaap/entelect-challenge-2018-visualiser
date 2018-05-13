﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018.Enums;

[RequireComponent(typeof(PrefabHolder))]
public class Instantiator : MonoBehaviour {

	private PrefabHolder prefabHolder;

	private GameObject buildingsParent;
	private GameObject missilesParent;
	private GameObject groundTilesParent;

	void Awake() {
		prefabHolder = GetComponent<PrefabHolder> ();
	}

	void Start() {
		buildingsParent = new GameObject (PrefabHolder.BUILDINGS_PARENT);
		missilesParent = new GameObject (PrefabHolder.MISSILES_PARENT);
		groundTilesParent = new GameObject (PrefabHolder.GROUNDTILES_PARENT);
	}

	public void ClearScene() {
		ClearGameObjectsWithTag ("Missile");
		ClearGameObjectsWithTag (GetTagForBuildingType(BuildingType.Attack));
		ClearGameObjectsWithTag (GetTagForBuildingType(BuildingType.Defense));
		ClearGameObjectsWithTag (GetTagForBuildingType(BuildingType.Energy));
	}

	private void ClearGameObjectsWithTag(string tag) {
		if (tag == null) {
			return;
		}

		GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag (tag);
		for (int i = 0; i < taggedGameObjects.Length; i++) {
			Destroy (taggedGameObjects[i]);
		}
	}

	public void InstantiateGroundTile(int x, int y) {
		GameObject o = Instantiate (prefabHolder.groundTilePrefab);
		o.transform.position = new Vector3 (x, o.transform.position.y, y);
		o.transform.SetParent (buildingsParent.transform);
	}

	public void InstantiateMissileAtLocation(List<Missile> missiles, int x, int y) {
		for (int m = 0; m < missiles.Count; m++) {
			GameObject missilePrefab = missiles[m].PlayerType == PlayerType.A ? prefabHolder.missilePrefab_A : prefabHolder.missilePrefab_B;
			GameObject o = Instantiate (missilePrefab);
			o.transform.position = new Vector3 (x, o.transform.position.y, y);
			o.transform.SetParent (missilesParent.transform);
		}
	}

	public void InstantiateBuildingsAtLocation(List<Building> buildings, int x, int y) {
		for (int b = 0; b < buildings.Count; b++) {
			GameObject o = GetPrefabForBuilding (buildings[b], buildings[b].PlayerType);
			if (o != null) {
				o = Instantiate (o);
				o.transform.position = new Vector3 (x, o.transform.position.y, y);
				o.transform.SetParent (groundTilesParent.transform);
				o.tag = GetTagForBuildingType(buildings [b].BuildingType);
			}
		}
	}

	private GameObject GetPrefabForBuilding(Building building, PlayerType playerType) {
		switch (building.BuildingType) {
			case BuildingType.Attack:
				return playerType == PlayerType.A ? prefabHolder.attackPrefab_A : prefabHolder.attackPrefab_B;
			case BuildingType.Defense:
				return playerType != PlayerType.A ? prefabHolder.defensePrefab_B : prefabHolder.defensePrefab_A;
			case BuildingType.Energy:
				return playerType != PlayerType.A ? prefabHolder.energyPrefab_B :  prefabHolder.energyPrefab_A;
			default:
				return null;
		}
	}

	private string GetTagForBuildingType(BuildingType buildingType) {
		switch (buildingType) {
			case BuildingType.Attack:
				return "Attack";
			case BuildingType.Defense:
				return "Defense";
			case BuildingType.Energy:
				return "Energy";
			default:
				return null;
		}
	}
}
