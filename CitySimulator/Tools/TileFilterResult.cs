using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySimulator.Tools {
    public enum TileFilterResult {
        TERF_CanBuild,
        TERF_NoBuild,
        TERF_AlreadyExists
    }
}
