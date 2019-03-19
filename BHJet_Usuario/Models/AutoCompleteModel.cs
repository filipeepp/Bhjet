using System;

namespace BHJet_Usuario.Models
{
    [Serializable]
    public class AutoCompleteModel
    {
        public string label { get; set; }
        public long? value { get; set; }
    }
}