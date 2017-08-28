using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class FormARequestService
    {
        private readonly LogisticDbContext LogisticDbContext;

        public FormARequestService(LogisticDbContext logisticDbContext)
        {
            this.LogisticDbContext = logisticDbContext;
        }

        public async Task<List<FormARequestInvoicesViewModel>> GetInvoicesList()
        {
            var invoices = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<FormARequestInvoicesViewModel>($@"
SELECT
    pib.NomorAju AS NomorAju,
	pib.NomorPIB AS NomorPIB,
	pib.TanggalPIB AS [TanggalPIB],
	si.InvoiceNumber AS InvoiceNumber,
	si.InvoiceDate AS InvoiceDate,
    [sid].ShipmentInvoiceDetailId as ShipmentInvoiceDetailId,
    [sid].FrameNumber AS FrameNumber,
    [sid].EngineNumber AS EngineNumber,
	v.DTPLOD AS DTPLOD,
	ct.[Name] AS Model,
	cc.[Name] AS Jenis
FROM
	ShipmentInvoice si
	JOIN PersetujuanImportBarang pib ON pib.NomorAju = si.NomorAju
    JOIN ShipmentInvoiceDetail [sid] ON [sid].InvoiceNumber = si.InvoiceNumber
	JOIN Vehicle v ON v.Katashiki = [sid].Katashiki AND v.Suffix = [sid].Suffix
	JOIN CarType ct ON ct.Katashiki = [sid].Katashiki AND ct.Suffix = [sid].Suffix
	JOIN CarCategory cc ON cc.CarCategoryId = ct.CarCategoryId
WHERE
    [sid].FormARequestNumber IS NULL
")).ToList();

            return invoices;
        }

        // TIE: START
        //public async Task<DataTable> UpdatePIB(List<FormARequestInvoicesViewModel> updateInvoicesList)
        //{
        //    var formARequestNumber = "11111";
        //    var formARequestDate = DateTime.UtcNow;

        //    var excelModel = new List<FormARequestExcelModel>();

        //    foreach (var invoice in updateInvoicesList)
        //    {
        //        var shipmentInvoiceDetail = await this.LogisticDbContext.ShipmentInvoiceDetail
        //            .Where(Q => Q.ShipmentInvoiceDetailId == invoice.ShipmentInvoiceDetailId)
        //            .FirstOrDefaultAsync();
        //        shipmentInvoiceDetail.FormARequestNumber = formARequestNumber;
        //        shipmentInvoiceDetail.FormARequestDate = formARequestDate;

        //        var unit = new FormARequestExcelModel();

        //        unit.Importir = "PT. Toyota Astra Motor";
        //        unit.AlamatImportir = "Jl. Gaya Motor III";
        //        unit.NomorPIB = invoice.NomorPIB;
        //        unit.TanggalPIB = invoice.TanggalPIB.ToString("d/M/yyyy");
        //        unit.Jenis = invoice.Jenis;
        //        unit.Model = invoice.Model;
        //        unit.FrameNumber = invoice.FrameNumber;
        //        unit.EngineNumber = invoice.EngineNumber;
        //        unit.DTPLOD = invoice.DTPLOD.ToString("yyyy");

        //        excelModel.Add(unit);
        //    }

        //    await this.LogisticDbContext.SaveChangesAsync();

        //    var excel = ListToDataTable<FormARequestExcelModel>(excelModel);

        //    return excel;
        //}

        //public DataTable ListToDataTable<T>(List<T> data)
        //{
        //    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        //    DataTable dataTable = new DataTable();

        //    for (int i = 0; i < properties.Count; i++)
        //    {
        //        PropertyDescriptor property = properties[i];
        //        dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
        //    }

        //    object[] values = new object[properties.Count];
        //    foreach (T item in data)
        //    {
        //        for (int i = 0; i < values.Length; i++)
        //        {
        //            values[i] = properties[i].GetValue(item);
        //        }

        //        dataTable.Rows.Add(values);
        //    }
        //    return dataTable;
        //}
        // TIE: END
    }
}
