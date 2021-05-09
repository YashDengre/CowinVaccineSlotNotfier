using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CovidVaccinationNotifier
{
    public partial class CovidVaccineNotiferService : ServiceBase
    {
        IVaccineNotifierService _notifierService;
        public CovidVaccineNotiferService()
        {
            InitializeComponent();
            _notifierService = new VaccineNotifierService();
        }

        protected override void OnStart(string[] args)
        {
            _notifierService.Start();
        }

        protected override void OnStop()
        {
            _notifierService.Stop();
        }
    }
}
