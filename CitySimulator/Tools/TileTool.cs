using System.Diagnostics;
using System.Drawing;

namespace CitySimulator.Tools {
    class TileTool : Tool {
        private readonly AreaSelector _area;
        private readonly IToolEffect _effect;

        public TileTool(AreaSelector areaSelector, IToolEffect effect) {
            _area = areaSelector;
            _effect = effect;
        }

        protected override void OnMouseDown(Game game, Camera camera, Point screenPos)
        {
            var pos3D = camera.ScreenSpaceToWorldSpace(screenPos, null, true);
            _area.Start = pos3D.Xy.Floor();
        }
        
        protected override void OnMouseDrag(Game game, Camera camera, Point screenPos) {

            var pos3D = camera.ScreenSpaceToWorldSpace(screenPos, null, true);
            _area.End = pos3D.Xy.Floor();
        }

        protected override void OnMouseUp(Game game, Camera camera, Point screenPos) {
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
