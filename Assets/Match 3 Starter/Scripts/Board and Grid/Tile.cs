/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
	private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
	public static Tile previousSelected = null;

	private SpriteRenderer render;
	private bool isSelected = false;

	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
	public List<GameObject> adjacentTiles = new List<GameObject>();
	
	void Awake() {
		render = GetComponent<SpriteRenderer>();
    }

	private void Select() {
		isSelected = true;
		render.color = selectedColor;
		previousSelected = gameObject.GetComponent<Tile>();
		SFXManager.instance.PlaySFX(Clip.Select);
	}

	private void Deselect() {
		isSelected = false;
		render.color = Color.white;
		previousSelected = null;
	}

	void OnMouseDown() {
		// Not Selectable conditions
		Debug.LogError("OnMouseDown");
		if (render.sprite == null || BoardManager.instance.IsShifting) {
			Debug.LogError("57");
			return;
		}

		if (isSelected) { // Is it already selected?
			Debug.LogError("62");
			Deselect();
		} else {
			Debug.LogError("65");
			if (previousSelected == null) { // Is it the first tile selected?
				Select();
				Debug.LogError("68");
			} else {
				Debug.LogError("70");
				/*if (GetAllAdjacentTiles().Contains(previousSelected.gameObject)) { // Is it an adjacent tile?*/
				if (GetAllAdjacentTiles().Contains(previousSelected.gameObject)) { // Is it an adjacent tile?
					Debug.LogError("71");
					try
					{
						if (previousSelected != null)
						{
							SwapSprite(previousSelected.render);
						}
						previousSelected.ClearAllMatches();
						previousSelected.Deselect();
						ClearAllMatches();
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						throw;
					}
				} else {
					Debug.LogError("77");
					previousSelected.GetComponent<Tile>().Deselect();
					Select();
				}
			}
		}
	}

	public void SwapSprite(SpriteRenderer render2) {
		if (render.sprite == render2.sprite) {
			return;
		}

		Sprite tempSprite = render2.sprite;
		render2.sprite = render.sprite;
		render.sprite = tempSprite;
		SFXManager.instance.PlaySFX(Clip.Swap);
		GUIManager.instance.MoveCounter--; // Add this line here
	}

	private GameObject GetAdjacent(Vector2 castDir) {
			Debug.LogError($"Getting Adjacent block in dir {castDir}");
			float rayDistance = 2f;
			Vector2 rayStart = (Vector2)transform.position + (castDir.normalized * rayDistance * 0.5f);
			RaycastHit2D hit = Physics2D.Raycast(rayStart, castDir, rayDistance);

		/*
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir,rayDistance);
		*/
		if (hit.collider != null) {
			Debug.LogError("Getting Adjacent block");
			return hit.collider.gameObject;
		}
		return null;
	}

	private List<GameObject> GetAllAdjacentTiles() {
		adjacentTiles = new List<GameObject>();
		for (int i = 0; i < adjacentDirections.Length; i++) {
			GameObject adjacentTile = GetAdjacent(adjacentDirections[i]);
			/*adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));*/
			if (adjacentTile != null) {
				adjacentTiles.Add(adjacentTile);
			}
		}
		return adjacentTiles;
	}

	private List<GameObject> FindMatch(Vector2 castDir) {
		List<GameObject> matchingTiles = new List<GameObject>();
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
		while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite) {
			matchingTiles.Add(hit.collider.gameObject);
			hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
		}
		return matchingTiles;
	}

	private void ClearMatch(Vector2[] paths) {
		List<GameObject> matchingTiles = new List<GameObject>();
		for (int i = 0; i < paths.Length; i++) { matchingTiles.AddRange(FindMatch(paths[i])); }
		if (matchingTiles.Count >= 2) {
			for (int i = 0; i < matchingTiles.Count; i++) {
				matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
			}
			matchFound = true;
		}
	}

	private bool matchFound = false;
	public void ClearAllMatches() {
		if (render.sprite == null)
			return;

		ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
		ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
		if (matchFound) {
			render.sprite = null;
			matchFound = false;
			StopCoroutine(BoardManager.instance.FindNullTiles()); //Add this line
			StartCoroutine(BoardManager.instance.FindNullTiles()); //Add this line
			SFXManager.instance.PlaySFX(Clip.Clear);
		}
	}

}