using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Models
{
    public class MaintenaceShiftKerjaViewModel
    {
        public List<LocationWorkHour> MaintenanceShiftKerjaFullModel { get; set; }
        public List<Shift> ShiftModel { get; set; }
        public List<MaintenaceShiftKerja_LocationModel> LokasiModel { get; set; }
    }
}
