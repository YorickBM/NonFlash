using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Util.Combiner
{
    class SectionCombiner : ICombinable
    {
        private List<Image> images;
        private List<Font> fonts;

        public SectionCombiner()
        {
            images = new List<Image>();
            fonts = new List<Font>();
        }

        public SectionCombiner AddSection(ICombinable section)
        {
            images.AddRange(section.GetImages());
            fonts.AddRange(section.GetFonts());
            return this;
        }

        public SectionCombiner AddSections(ICombinable[] sections)
        {
            sections.ToList().ForEach(s => images.AddRange(s.GetImages()));
            sections.ToList().ForEach(s => fonts.AddRange(s.GetFonts()));
            return this;
        }

        public Image[] GetImages()
        {
            return images.ToArray();
        }
        public Font[] GetFonts()
        {
            return fonts.ToArray();
        }
    }

    interface ICombinable
    {
        Image[] GetImages();
        Font[] GetFonts();
    }
}
