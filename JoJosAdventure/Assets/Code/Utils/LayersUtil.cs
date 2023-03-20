using UnityEngine;

namespace JoJosAdventure.Utils
{
    public class LayersUtil
    {
        public const string PlayerLayerString = "Jojo";

        private static int? _playerLayer;

        public static int PlayerLayer
        {
            get
            {
                if (!_playerLayer.HasValue)
                    _playerLayer = LayerMask.NameToLayer(PlayerLayerString);
                return _playerLayer.Value;
            }
        }

        /// <summary>
        /// Checks if a gameObject layer is equal to a LayerMask layer
        /// </summary>
        public static bool EqualsLayerMask(int layer, LayerMask m)
        {
            return ((1 << layer) & m) != 0;
        }

        public static bool IsColliderPlayer(Collider2D collider)
        {
            return collider.gameObject.layer == PlayerLayer;
        }

        public static bool IsTouchingPlayer(Collider2D collider)
        {
            return collider.IsTouchingLayers(PlayerLayer);
        }
    }
}