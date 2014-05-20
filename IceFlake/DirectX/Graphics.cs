#if SLIMDX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;

namespace IceFlake.DirectX
{
    // TODO: Implement
    public class Graphics
    {
        public Graphics()
        {
            gPreparedFrame = false;
        }

        private bool gPreparedFrame
        {
            get;
            set;
        }

        public void Render(params IResource[] resources)
        {
            if (!gPreparedFrame)
            {
                // Add rendering
                gPreparedFrame = true;
            }
        }

    }
}
#endif
