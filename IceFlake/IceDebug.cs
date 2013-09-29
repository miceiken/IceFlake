using IceFlake.Client.Miscellaneous;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IceFlake.Client;

namespace IceFlake
{
    public partial class IceDebug : Form
    {
        public IceDebug()
        {
            InitializeComponent();
        }

        private const int IMAGE_SIZE = 256;
        private const float TILE_SIZE = 1600 / 3;
        private const float ORIGIN = -17066.666f;

        private void GetTileByLocation(float[] loc, out float x, out float y)
        {
            x = (loc[0] - ORIGIN) / TILE_SIZE;
            y = (loc[1] - ORIGIN) / TILE_SIZE;
        }

        private Bitmap GetMinimapImage(string world, int width, int height, int minX, int maxX, int minY, int maxY)
        {
            var background = new MinimapImage(world, 256, 256, 0, 64, 0, 64);
            background.Generate();

            using (var graphics = Graphics.FromImage(background.Result))
            {
                // DO THE DRAWING HERE

                graphics.SmoothingMode = SmoothingMode.HighQuality;
            }
            return background.Result;
        }

        private void pbMap_Click(object sender, EventArgs e)
        {

        }

        private void mapRenderTimer_Tick(object sender, EventArgs e)
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            var lp = Manager.LocalPlayer.Location;
            //var tX = (((32 * TILE_SIZE) - lp.Y) / TILE_SIZE);
            //var tY = (((32 * TILE_SIZE) - lp.X) / TILE_SIZE);
            //const float scale = 0.5f;
            //var cX = (((32 * TILE_SIZE) - lp.Y) % TILE_SIZE * (IMAGE_SIZE / TILE_SIZE) * scale);
            //var cY = (((32 * TILE_SIZE) - lp.X) % TILE_SIZE * (IMAGE_SIZE / TILE_SIZE) * scale);

            float x, y;
            GetTileByLocation(lp.ToFloatArray(), out x, out y);

            int minX = (int)(x - 1), maxX = (int)(x + 1), minY = (int)(y - 1), maxY = (int)(y + 1);

            //pbMap.Image = GetMinimapImage(WoWWorld.CurrentMap, pbMap.Width, pbMap.Height, minX, maxX, minY, maxY);
            pbMap.Image = GetMinimapImage(WoWWorld.CurrentMap, 256, 256, 0, 64, 0, 64);
        }

        //// Prints specified color arrow to screen facing heading (0 - right, PI/2 - down, PI - left, 3PI/2 - up)
        //private void PrintArrow(Color color, int x, int y, double heading /*radians*/, string topString, string botString)
        //{
        //    heading = ConvertHeading(heading);

        //    //Define arrow/rotation
        //    var arrow = new Point[5]
        //        {
        //            new Point(Convert.ToInt32(x + Math.Cos(heading) * 10), Convert.ToInt32(y + Math.Sin(heading) * 10)),
        //            new Point(Convert.ToInt32(x + Math.Cos(heading + Math.PI * 2 / 3) * 5), Convert.ToInt32(y + Math.Sin(heading + Math.PI * 2 / 3) * 5)),
        //            new Point(x, y),
        //            new Point(Convert.ToInt32(x + Math.Cos(heading + Math.PI * 2 / -3) * 5), Convert.ToInt32(y + Math.Sin(heading + Math.PI * 2 / -3) * 5)),
        //            new Point(Convert.ToInt32(x + Math.Cos(heading) * 10), Convert.ToInt32(y + Math.Sin(heading) * 10)),
        //        };

        //    //Print arrow
        //    _offScreenDc.DrawLines(new Pen(color), arrow);

        //    //Print Top String
        //    if (topString.Length > 0)
        //    {
        //        _offScreenDc.DrawString(topString, _fontText, new SolidBrush(color),
        //                                new PointF(x - topString.Length * (float)2.2, y - 15));
        //    }
        //    //Print Bottom String
        //    if (botString.Length > 0)
        //    {
        //        _offScreenDc.DrawString(botString, _fontText, new SolidBrush(color),
        //                                new PointF(x - botString.Length * 2, y + 6));
        //    }
        //}

        //// Print circle
        //private void PrintCircle(Color color, int x, int y, string name)
        //{
        //    const int radius = 3;
        //    SolidBrush redBrush = new SolidBrush(color);
        //    //Do whatever should be printed
        //    _offScreenDc.DrawEllipse(new Pen(color), x - radius, y - radius, 2 * radius, 2 * radius);
        //    _offScreenDc.FillEllipse(redBrush, x - radius, y - radius, 2 * radius, 2 * radius);

        //    _offScreenDc.DrawString(name, _fontText, new SolidBrush(color),
        //                                new PointF(x - name.Length * 2, y - 15));
        //}
    }
}
