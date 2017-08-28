using Dapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
// TIE: START
// using System.Transactions;
// TIE: END
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    [ServiceFilter(typeof(UpdateFailedAttribute))]
    public class UploadDownloadService
    {
        private readonly LogisticDbContext dbContext;
        private readonly IExcelExportHelperService ExcelExportHelperService;
        private readonly WebEnvironmentService WebEnvService;

        public UploadDownloadService(LogisticDbContext dbContext,IExcelExportHelperService ExcelExportHelperService,WebEnvironmentService webEnvService)
        {
            this.dbContext = dbContext;
            this.ExcelExportHelperService = ExcelExportHelperService;
            this.WebEnvService = webEnvService;
        }

        // TIE: START
        //public byte[] GetLogBlob(int id)
        //{
        //    return dbContext.LogUploadDownloadFile.FirstOrDefault(Q => Q.LogUploadDownloadId == id).Blob;
        //}

        //public bool CheckTable(string master)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT 1 FROM sys.tables AS T
        //                            INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id
        //                            WHERE T.Name = @master";
        //        var result =  con.Query<bool>(query, new { master = master });
        //        return result.FirstOrDefault();
        //    }
        //}
        //public async Task<List<dynamic>> GetDataByFilter(string master,FilterDateModel Data)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string condition = "";
        //        for(int i = 0; i < Data.field.Count; i++)
        //        {
        //            if (Data.DateFrom[i] == null && Data.DateTo[i] != null)
        //            {
        //                condition += string.Format("{0} < '{1}' AND ", Data.field[i], (DateTime?)Data.DateTo[i]?.ToLocalTime());
        //            }
        //            else if (Data.DateTo[i] == null && Data.DateFrom[i] != null)
        //            {
        //                condition += string.Format("{0} > '{1}' AND ", Data.field[i], (DateTime?)Data.DateFrom[i]?.ToLocalTime());
        //            }
        //            else if (Data.DateFrom[i] != null && Data.DateTo[i] != null)
        //            {
        //                condition += string.Format("{0} BETWEEN '{1}' AND '{2}' AND ", Data.field[i], (DateTime?)Data.DateFrom[i]?.ToLocalTime(), (DateTime?)Data.DateTo[i]?.ToLocalTime());
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(condition))
        //        {
        //            condition = condition.Insert(0, "WHERE ");
        //            condition = condition.Remove(condition.Length - 4, 4);
        //        }
        //        string query = @"DECLARE @COLUMNS VARCHAR(1000)
        //                        DECLARE @table_name SYSNAME
        //                        DECLARE @Condition VARCHAR(MAX)

        //                        SET @table_name = @master
        //                        SET @Condition = @kondisi
        //                        SELECT @Columns = SubString (( SELECT ', ' + QUOTENAME(Column_name )
        //                        from INFORMATION_SCHEMA.columns
        //                        WHERE Table_name = @table_name
        //                        AND COLUMN_NAME NOT IN('CreatedAt','CreatedBy','UpdatedAt','UpdatedBy')
        //                        AND COLUMN_NAME NOT IN(
        //                            select b.name
        //                            from
        //                            sysobjects a inner join syscolumns b on a.id = b.id
        //                            where
        //                             columnproperty(a.id, b.name, 'isIdentity') = 1
        //                             AND a.name = @table_name
        //                            ) 
        //                        FOR XML PATH ( '' ) ), 3, 1000) 
        //                        EXEC('SELECT '+ @Columns +' FROM '+ @table_name+' '+@Condition)
        //                        ";
        //        var result = await con.QueryAsync<dynamic>(query, new { master = master, kondisi = condition });
        //        return result.ToList();
        //    }
        //}

        //public async Task<List<string>> GetPrimaryKeyByTable(string master)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string query = @"
        //                        DECLARE @table_name SYSNAME = @master
        //                        SELECT Col.Column_Name from 
        //                            INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
        //                            INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
        //                        WHERE 
        //                            Col.Constraint_Name = Tab.Constraint_Name
        //                            AND Col.Table_Name = Tab.Table_Name
        //                            AND Constraint_Type = 'PRIMARY KEY'
        //                            AND Col.Table_Name = @table_name";
        //        var result = await con.QueryAsync<string>(query, new { master = master });
        //        return result.ToList();
        //    }
        //}
        ///// <summary>
        ///// Get Condition for Query
        ///// </summary>
        ///// <returns>WHERE Katashiki IN (a,b) AND Suffix IN (a,b)</returns>
        //public string GetCondition(string columnsPrimary, List<string> Data)
        //{
        //    string condition = "";
        //    condition += string.Format("{0} IN (", columnsPrimary);
        //    if(Data[0].GetType() == typeof(int))
        //    {
        //        for (int i = 0; i < Data.Count; i++)
        //        {
        //            condition += string.Format("{0},", Data[i]);
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < Data.Count; i++)
        //        {
        //            condition += string.Format("'{0}',", Data[i]);
        //        }
        //    }
        //    condition = condition.Remove(condition.Length - 1, 1);
        //    return condition += ")"; ;
        //}
        ///// <summary>
        ///// Get Columns Name for Query
        ///// </summary>
        ///// <returns>(Katashiki,Suffix)</returns>
        //public string  GetColumns(List<string> columnsPrimary)
        //{
        //    string columns = "(";
        //    foreach(var col in columnsPrimary)
        //    {
        //        columns += string.Format("{0},",col) ;
        //    }
        //    columns = columns.Remove(columns.Length - 1, 1);
        //    return columns += ")";
        //}

        ///// <summary>
        ///// Get Data by Master by PK
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<dynamic>> GetDataByFilter(string master, List<string> columnsPrimary,UploadDownloadPrimay Data)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string condition = "";
        //        if (columnsPrimary.Count == 3 )
        //        {
        //            condition += GetCondition(columnsPrimary[0], Data.FirstProp);
        //            condition += " AND ";
        //            condition += GetCondition(columnsPrimary[1], Data.SecondProp);
        //            condition += " AND ";
        //            condition += GetCondition(columnsPrimary[2], Data.ThirdProp);
        //        }
        //        else if (columnsPrimary.Count ==2 )
        //        {
        //            condition += GetCondition(columnsPrimary[0], Data.FirstProp);
        //            condition += " AND ";
        //            condition += GetCondition(columnsPrimary[1], Data.SecondProp);
        //        }else if (columnsPrimary.Count == 1)
        //        {
        //            condition += GetCondition(columnsPrimary[0], Data.FirstProp);
        //        }
        //        string query = @"DECLARE @COLUMNS VARCHAR(1000)
        //                        DECLARE @table_name SYSNAME
        //                        DECLARE @Condition VARCHAR(MAX)

        //                        SET @table_name = @master
        //                        SET @Condition = @kondisi
        //                        SELECT @Columns = SubString (( SELECT ', ' + QUOTENAME(Column_name )
        //                        from INFORMATION_SCHEMA.columns
        //                        WHERE Table_name = @table_name
        //                        AND COLUMN_NAME NOT IN('CreatedAt','CreatedBy','UpdatedAt','UpdatedBy')
        //                        AND COLUMN_NAME NOT IN(
        //                            select b.name
        //                            from
        //                            sysobjects a inner join syscolumns b on a.id = b.id
        //                            where
        //                             columnproperty(a.id, b.name, 'isIdentity') = 1
        //                             AND a.name = @table_name
        //                            ) 
        //                        FOR XML PATH ( '' ) ), 3, 1000) 
        //                        EXEC('SELECT '+ @Columns +' FROM '+ @table_name+' WHERE '+@Condition)
        //                        ";
        //        var result =  await con.QueryAsync<dynamic>(query, new { master = master, kondisi = condition });

        //        return result.ToList();
        //    }
        //}


        //public async Task<List<dynamic>> GetData(String master)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string query = @"DECLARE @COLUMNS VARCHAR(1000)
        //                        DECLARE @table_name SYSNAME
        //                        SET @table_name = @master
        //                        SELECT @Columns = SubString (( SELECT ', ' + QUOTENAME(Column_name )
        //                        from INFORMATION_SCHEMA.columns
        //                        WHERE Table_name = @table_name
        //                        AND COLUMN_NAME NOT IN('CreatedAt','CreatedBy','UpdatedAt','UpdatedBy')
        //                        AND COLUMN_NAME NOT IN(
        //                            select b.name
        //                            from
        //                            sysobjects a inner join syscolumns b on a.id = b.id
        //                            where
        //                             columnproperty(a.id, b.name, 'isIdentity') = 1
        //                             AND a.name = @table_name
        //                            ) 
        //                        FOR XML PATH ( '' ) ), 3, 1000) 
        //                        EXEC('SELECT '+ @Columns +' FROM '+ @table_name)";

        //        var result = await con.QueryAsync<dynamic>(query, new {master = master});
        //        return result.ToList();
        //    }
        //}

        //public DataTable GetDtByDynamic(object dt)
        //{
        //    DataTable Data =  ExcelExportHelperService.DynamicToDataTable(dt);
        //    return Data;
        //}
        //[ServiceFilter(typeof(UpdateFailedAttribute))]
        //public void DownloadAndInsertLog(object json,string master,string title,int logId)
        //{
        //    DataTable dt = GetDtByDynamic(json);
        //    var jobId = UpdateFailedAttribute.JobId;
        //    logId = CreateLogUploadDownload(master, false, string.Format("Master{0}_{1:ddMMyyyyHHmmss}.xlsx", title.Replace(" ", string.Empty),DateTime.Now.ToLocalTime()), jobId);
        //    byte[] Data = ExcelExportHelperService.ExportExcel(dt, "Master " + title, false);
        //    UpdateLogDownload(logId, Data);
        //}

        //public void UpdateLogDownload(int logId,byte[] blob)
        //{
        //    dbContext.Database.CreateExecutionStrategy().Execute(()=> {
        //        var trans = dbContext.Database.BeginTransaction();
        //        {
        //            LogUploadDownload log = dbContext.LogUploadDownload.Find(logId);
        //            log.EndTime = DateTime.UtcNow;
        //            log.Status = "Sukses";
        //            log.UpdatedAt = DateTime.UtcNow;
        //            log.UpdatedBy = WebEnvService.UserHumanName ;

        //            LogUploadDownloadFile logFile = new LogUploadDownloadFile()
        //            {
        //                LogUploadDownloadId = logId,
        //                Blob = blob
        //            };
        //            dbContext.LogUploadDownloadFile.Add(logFile);
        //            dbContext.SaveChanges();
        //            trans.Commit();
        //        }
        //    });

        //}

        //public int CreateLogUploadDownload(string menu,bool isUploadProsess, string fileName,int jobId)
        //{
        //    int row = 0, logId = 0;
        //    dbContext.Database.CreateExecutionStrategy().Execute(()=>
        //    {
        //        var tran = dbContext.Database.BeginTransaction();
        //        {
        //            LogUploadDownload log = new LogUploadDownload()
        //            {
        //                Module = "Master Data",
        //                Menu = menu,
        //                IsUploadProcess = isUploadProsess,
        //                StartTime = DateTime.UtcNow,
        //                EndTime = null,
        //                FileName = !isUploadProsess ? fileName : null,
        //                Status = "Sedang " + (isUploadProsess ? "Upload" : "Download"),
        //                JobId = jobId,
        //                CreatedBy = WebEnvService.UserHumanName,
        //                CreatedAt = DateTime.UtcNow,
        //                UpdatedBy = WebEnvService.UserHumanName,
        //                UpdatedAt = DateTime.UtcNow
        //            };
        //            dbContext.LogUploadDownload.Add(log);
        //            row = dbContext.SaveChanges();
        //            tran.Commit();
        //            if(row==1)
        //                logId = log.LogUploadDownloadId;
        //        }
        //    });
        //    return logId;
        //}

        //public string GetColumnsQuery(DataColumnCollection dataColumns)
        //{
        //    string columns = "";
        //    foreach (DataColumn column in dataColumns)
        //    {
        //        columns += string.Format("{0},", column.ColumnName);
        //    }
        //    return columns.Remove(columns.Length - 1, 1);
        //}
        //public string GetValuesQuery(DataRow dr) {
        //    string values = @"";
        //    foreach (var item in dr.ItemArray)
        //    {
        //        if (item.GetType() == typeof(string))
        //        {
        //            if (string.IsNullOrEmpty(item.ToString()))
        //            {
        //                values += "NULL,";
        //            }
        //            else
        //                values += string.Format("'{0}',",item.ToString().Replace("'", "''"));
        //        }
        //        else if (item.GetType() == typeof(DateTime))
        //        {
        //            values += string.Format("'{0}',", DateTime.Parse(item.ToString()).ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
        //        }
        //        else if (item.GetType() == typeof(bool))
        //        {
        //            values += string.Format("{0},", (bool)item? 1 : 0);
        //        }
        //        else
        //            values += string.Format("{0},", string.IsNullOrEmpty(item.ToString())?"null":item);
        //    }
        //    values = values.Remove(values.Length - 1, 1);
        //    return values;
        //}

        //public string GetFullQueryInsert(DataRowCollection dataRow,string dataColumn,string master)
        //{
        //    String values = @"";
        //    foreach (DataRow dr in dataRow)
        //    {
        //        values += "INSERT INTO "+master+"("+dataColumn+") VALUES(";
        //        values += GetValuesQuery(dr);
        //        values += ");";
        //    }
        //    return values;
        //}
        //[ServiceFilter(typeof(UpdateFailedAttribute))]
        //public void SaveUpload(string master,string title,int logId,object jsonObject)
        //{
        //    var jobId = UpdateFailedAttribute.JobId;
        //    DataTable Data = GetDtByDynamic(jsonObject);
        //    logId = CreateLogUploadDownload(master, true, string.Format("Master{0}_{1:ddMMyyyyhhmmss}.xlsx", title.Replace(" ", string.Empty), DateTime.Now.ToLocalTime()), jobId);
        //    dbContext.Database.CreateExecutionStrategy().Execute(()=>
        //    {
        //        using (var trans = new TransactionScope())
        //        using (var con = dbContext.Database.GetDbConnection())
        //        {
        //            foreach (DataRow dr in Data.Rows)
        //            {
        //                con.Execute($@"INSERT INTO {master}({GetColumnsQuery(Data.Columns)}) VALUES({GetValuesQuery(dr)});");
        //            }
        //            //con.Execute(GetValuesQuery(Data.Rows, GetColumnsQuery(Data.Columns), master), new { });

        //            string query = @"
        //                    UPDATE LogUploadDownload
        //                    SET Status = 'Sukses',EndTime = GETUTCDATE(),UpdatedAt = GETUTCDATE()
        //                    ,UpdatedBy = 'SYSTEM'
        //                    WHERE LogUploadDownloadId = @logId
        //                    ";
        //            con.Execute(query, new { logId = logId });
        //            trans.Complete();
        //        }
        //    });


        //}
        //public List<InformationSchemaModel> GetSchema(string master)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT COLUMN_NAME,IS_NULLABLE,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH
        //                        FROM INFORMATION_SCHEMA.COLUMNS a
        //                        WHERE TABLE_NAME = @master
        //                        AND COLUMN_NAME NOT IN('CreatedAt','CreatedBy','UpdatedAt','UpdatedBy')
        //                        AND COLUMN_NAME NOT IN(
        //                            select
        //                            b.name as IdentityColumn
        //                            from
        //                            sysobjects a inner join syscolumns b on a.id = b.id
        //                            where
        //                             columnproperty(a.id, b.name, 'isIdentity') = 1
        //                             AND a.name = @master
        //                            ) ";
        //        return con.Query<InformationSchemaModel>(query, new { master = master }).ToList();
        //    }
        //}

        //public async Task<List<string>> GetColumnDateAsync(string master)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT COLUMN_NAME
        //                    FROM INFORMATION_SCHEMA.COLUMNS a
        //                    WHERE TABLE_NAME = @master
        //                    AND COLUMN_NAME NOT IN('CreatedAt','CreatedBy','UpdatedAt','UpdatedBy')
        //                    AND DATA_TYPE = 'datetime2'";
        //        var result = await con.QueryAsync<string>(query, new { master = master });
        //        return result.ToList();
        //    }
        //}


        //public async Task<List<InformationSchemaModel>> GetSchemaAsync(string master)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT COLUMN_NAME,IS_NULLABLE,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH
        //                        FROM INFORMATION_SCHEMA.COLUMNS a
        //                        WHERE TABLE_NAME = @master
        //                        AND COLUMN_NAME NOT IN('CreatedAt','CreatedBy','UpdatedAt','UpdatedBy')
        //                        AND COLUMN_NAME NOT IN(
        //                            select
        //                            b.name as IdentityColumn
        //                            from
        //                            sysobjects a inner join syscolumns b on a.id = b.id
        //                            where
        //                             columnproperty(a.id, b.name, 'isIdentity') = 1
        //                             AND a.name = @master
        //                            ) ";
        //        var result = await con.QueryAsync<InformationSchemaModel>(query, new { master = master });
        //        return result.ToList();
        //    }
        //}
        // TIE: END
    }
}
