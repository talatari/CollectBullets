using Sirenix.OdinInspector;
using Source.Scripts.Players;
using Source.Scripts.Upgrades;
using UnityEngine;

namespace Source.Scripts.Mediators
{
    public class Mediator : MonoBehaviour
    {
        [Required, SceneObjectsOnly] public Player Player;
        [SerializeField] private UpgradeButton _upgradeButtonLeft;
        [SerializeField] private UpgradeButton _upgradeButtonMiddle;
        [SerializeField] private UpgradeButton _upgradeButtonRight;
    }
}