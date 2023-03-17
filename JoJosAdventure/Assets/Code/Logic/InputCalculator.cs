using Assets.Code.Enums;

namespace Assets.Code.Logic
{
    public static class InputCalculator
    {
        public static MoveInputDirection MovementInputX(float inputX)
        {
            bool xChanged = inputX != 0;
            if (!xChanged) return MoveInputDirection.NoMovement;

            bool isLeft = inputX < 0;
            if (isLeft) return MoveInputDirection.WalkLeft;

            return MoveInputDirection.WalkRight;
        }

        public static MoveInputDirection MovementInputY(float inputY)
        {
            bool yChanged = inputY != 0;
            if (!yChanged) return MoveInputDirection.NoMovement;

            bool isDown = inputY < 0;
            if (isDown) return MoveInputDirection.WalkDown;

            return MoveInputDirection.WalkUp;
        }
    }
}