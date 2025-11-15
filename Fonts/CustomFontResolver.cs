using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Fonts;

namespace AntiqueShopAvalonia.Fonts
{
    public class CustomFontResolver : IFontResolver
    {
        public static readonly CustomFontResolver Instance = new CustomFontResolver();

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Arial", StringComparison.OrdinalIgnoreCase))
            {
                if (isBold)
                    return new FontResolverInfo("Arial#Bold");

                return new FontResolverInfo("Arial#Regular");
            }

            return null!;
        }

        public byte[] GetFont(string faceName)
        {
            switch (faceName)
            {
                case "Arial#Regular":
                    var file = File.ReadAllBytes("C:/Users/Ильдар/Desktop/Учеба/4-1/учебка/projects/AntiqueShopAvalonia/Fonts/arialmt.ttf");
                    return file;

            }

            throw new Exception("Unknown font: " + faceName);
        }
    }
}
