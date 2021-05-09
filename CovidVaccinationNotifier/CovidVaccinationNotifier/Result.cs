using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccinationNotifier
{
    public class Result<T>
    {
        public int resultCode { get; set; }
        public string message { get; set; }

        public T Object { get; set; }
    }
}
