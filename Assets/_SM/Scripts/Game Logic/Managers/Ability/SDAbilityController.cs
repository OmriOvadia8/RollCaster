//using SD_GameLoad;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using Random = UnityEngine.Random;

//namespace SD_Ability
//{
//    public class SDAbilityController : SDLogicMonoBehaviour
//    {
//        [SerializeField] private SDAbilityAnimationController animationController;

//        private readonly Dictionary<Range, int> rollToAnimationLevel = new Dictionary<Range, int>
//        {
//            { new Range(1, 3), 1 },
//            { new Range(4, 5), 2 },
//            { new Range(6, 6), 3 }
//        };

//        public void UseAbility(string abilityName)
//        {
//            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName);
//            if (ability != null)
//            {
//                int diceRoll = RollDice();
//                int animationLevel = DetermineAnimationLevel(diceRoll);
//                animationController.PlayAnimation(abilityName, animationLevel);
//                int damage = CalculateDamage(ability, animationLevel);
//                DealDamage(damage);
//            }
//        }

//        private int RollDice()
//        {
//            return Random.Range(1, 7); // This will return a number between 1 and 6 inclusive.
//        }

//        private int DetermineAnimationLevel(int diceRoll)
//        {
//            return rollToAnimationLevel.First(pair => pair.Key.Contains(diceRoll)).Value;
//        }

//        private int CalculateDamage(SDAbility ability, int animationLevel)
//        {
//            return ability.BaseDamage * animationLevel;
//        }

//        private void DealDamage(int damage)
//        {
//            Debug.Log("Dealt " + damage + " damage.");
//        }
//    }
//}
