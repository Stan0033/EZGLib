using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZGLib.EZG
{
    public class EZSequence
    {
        public string Name { get; set; } = string.Empty;

        private int from = 0;
        public int From
        {
            get => from;
            set
            {
                if (value >= To) return;
                if (value < 0) return;
                from = value;
            }
        }

        private int to = 0;
        public bool Looping = false;

        public int To
        {
            get => to;
            set
            {
                if (value <= From) return;
                if (value < 0) return;
                to = value;
            }
        }

        public int Interval => To - From;

        public string _Name { get => Name.Replace(' ', '_').ToString(); }

        public EZSequence(string name, int from, int to)
        {
            Name = name;
            this.from = from;  // Use backing fields to avoid validation in constructor
            this.to = to;
        }
        public EZSequence(string name, int from, int to, bool looping)
        {
            Name = name;
            this.from = from;  // Use backing fields to avoid validation in constructor
            this.to = to;
            Looping = looping;
        }
        public bool Contains(int frame)
        {
            return frame >= From && frame <= To;
        }

        public override string ToString()
        {
            return $"[sequence] {_Name} {From} {To} {Looping}";
        }
    }
}
