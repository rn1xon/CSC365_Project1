using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC365_Project1.Models
{
    internal class SymptomTask2
    {
        public int VAERS_ID { get; set; }
        public decimal AGE_YRS { get; set; } 
        public string SEX { get; set; } = "";
        public string VAX_NAME { get; set; } = "";
        public string RPT_DATE { get; set; } = "";
        public string SYMPTOM { get; set; } = "";
        public string DIED { get; set; } = "";
        public string DATEDIED { get; set; } = "";
        public string SYMPTOM_TEXT { get; set; } = "";
    }
}
