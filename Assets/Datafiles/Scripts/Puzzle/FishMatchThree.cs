using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Match3;
using UnityEngine;

public class FishMatchThree : MonoBehaviour
{
   public LevelMoves _LevelMoves;
   public GameGrid _GameGrid;
   private void OnEnable()
   {
      Debug.LogError("Restarting Puzzle");
      _LevelMoves.UpdateMoves();
      _GameGrid.enabled = true;
   }

   private void Start()
   {
   }

   private void OnDisable()
   {
      _GameGrid.enabled = false;
   }
}
