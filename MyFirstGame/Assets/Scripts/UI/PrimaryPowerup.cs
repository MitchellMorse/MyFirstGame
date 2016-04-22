using UnityEngine;
using System.Collections;
using Assets.Scripts.PlayerClasses;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class PrimaryPowerup : MonoBehaviour, IPointerClickHandler
    {

        public PlayerController Player;

        public void OnPointerClick(PointerEventData eventData)
        {
            Player.PrimaryPowerupClicked();
        }
    }
}