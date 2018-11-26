using System;

namespace BHJet_Admin.Models
{
    [Serializable]
    public class AutoCompleteModel
    {
        public string label { get; set; }
        public long? value { get; set; }
    }
}