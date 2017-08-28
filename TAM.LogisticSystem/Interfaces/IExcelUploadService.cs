using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Interfaces
{
    public interface IExcelUploadService
    {
        DataTable toDataTable(ExcelPackage package);
    }
}
