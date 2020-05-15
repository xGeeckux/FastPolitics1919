using FastPolitics1919.Data.Common;
using FastPolitics1919.History.Governments;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FastPolitics1919.Common
{
    public class Landscape : Tile
    {
        public override BitmapImage Icon => Gfx.Images.IconLandscape;
        public override BitmapImage BackgroundImage => Gfx.Images.IconBackgroundLandscape;
    }
}
