using System.Collections.Generic;

namespace GlobusTest.Settings
{
    public class LgaSettings
    {
        public string Test { get; set; }
        public List<SateList> SateList { get; set; }
    }

    public class SateList
    {
        public State States { get; set; }
    }
    public class State
    {
        public string Name { get; set; }
        public double Id { get; set; }
        public List<Lga> Locals { get; set; }
    }

    public class Lga
    {
        public string Name { get; set; }
        public double Id { get; set; }
    }
}
