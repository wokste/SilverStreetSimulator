using System.Diagnostics;
using SFML.Window;

namespace CitySimulator.Tools {
    class TileTool : Tool {
        private readonly AreaSelector _area;
        private readonly IToolEffect _effect;

        public TileTool(AreaSelector areaSelector, IToolEffect effect) {
            _area = areaSelector;
            _effect = effect;
        }

        protected override void OnMouseDown(Game game, IsometricView view, Vector2f screenPos)
        {
            _area.Start = view.ScreenPxToWens(screenPos);
        }
        
        protected override void OnMouseDrag(Game game, IsometricView view, Vector2f screenPos) {
            _area.End = view.ScreenPxToWens(screenPos);
        }

        protected override void OnMouseUp(Game game, IsometricView view, Vector2f screenPos)
        {
            _area.End = view.ScreenPxToWens(screenPos);

            var cost = CalculateCost();

            if (cost > game.Money) {
                Debug.WriteLine($"insufficient money. Need: {cost} Has {game.Money}");
                return;
            }

            game.Money -= cost;
            
            _area.Iterate(_effect.Apply, _effect.Filter);
        }

        public int CalculateCost() {
            var totalCost = 0;
            
            _area.Iterate((i,j) => {
                totalCost += _effect.Cost;
            }, _effect.Filter);

            return totalCost;
        }
    }
}
