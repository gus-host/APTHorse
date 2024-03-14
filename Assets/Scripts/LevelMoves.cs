using System;
using UnityEngine;

namespace Match3
{
    public class LevelMoves : Level
    {

        public int numMoves;
        public int targetScore;

        public int _movesUsed = 0;

        public event Action OnPlayerWonPuzzle;
        public event Action OnPlayerLosePuzzle;
        
        private void Start()
        {
            type = LevelType.Moves;
            hud.SetLevelType(type);
            hud.SetScore(currentScore);
            hud.SetTarget(targetScore);
            hud.SetRemaining(numMoves);
        }

        public void UpdateMoves()
        {
            hud.SetRemaining(numMoves);
        }
        
        public override void OnMove()
        {
            _movesUsed++;

            hud.SetRemaining(numMoves - _movesUsed);

            if (numMoves - _movesUsed != 0) return;

            if (numMoves - _movesUsed == 0)
            {
                OnPlayerWonPuzzle?.Invoke();
            }
            else
            {
                OnPlayerLosePuzzle?.Invoke();
            }
            
            /*if (currentScore >= targetScore)
            {
                GameWin();
            }
            else
            {
                GameLose();
            }*/
        }
    }
}
