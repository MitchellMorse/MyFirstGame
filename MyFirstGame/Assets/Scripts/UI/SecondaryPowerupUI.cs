using UnityEngine;
using System.Collections;
using Assets.Scripts.PlayerClasses;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class SecondaryPowerupUI : MonoBehaviour, IPointerClickHandler
    {

        public PlayerController Player;

        public void OnPointerClick(PointerEventData eventData)
        {
            Player.SecondaryPowerupClicked();
        }
    }
}