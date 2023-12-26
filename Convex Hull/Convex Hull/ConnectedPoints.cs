using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convex_Hull
{
    internal class ConnectedPoints
    {
        public Point Origin, ToPoint1, ToPoint2;

        public ConnectedPoints(Point p)
        {
            this.Origin = p;
        }
    }
}
