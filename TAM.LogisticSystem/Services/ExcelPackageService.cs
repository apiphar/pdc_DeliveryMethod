using Dapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    // TIE: START
    //public class ExcelPackageService : IExcelPackageExtension
    public class ExcelPackageService
    // TIE: END
    {
        private readonly LogisticDbContext dbContext;
        public ExcelPackageService(LogisticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // TIE: START
        ///// <summary>
        ///// Get All Content of ForeignKey Column
        ///// </summary>
        ///// <returns>
        ///// </returns>
        //List<HashSet<string>> GetAllFKList(List<ForeignKeySchema> FKReference)
        //{
        //    List<HashSet<string>> FKList = new List<HashSet<string>>();
        //    foreach (var row in FKReference)
        //    {
        //        String[] colList = GetColumnListByFK(row.Table, row.Column).Select(Q=>Q.ToLower()).ToArray();
        //        HashSet<string> colHashSet = new HashSet<string>(colList);
        //        FKList.Add(colHashSet);
        //    }
        //    return FKList;
        //}

        ///// <summary>
        ///// Get FK Column reference (ForeignKeySchema)
        ///// </summary>
        ///// <returns>
        ///// RegionCode,ParentRegionCode,RegionCodeAFI
        ///// </returns>
        //List<ForeignKeySchema> GetFKReference(string master)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT  
        //                            KCU1.COLUMN_NAME AS FKColumn
        //                            ,KCU2.TABLE_NAME AS [Table]
        //                            ,KCU2.COLUMN_NAME AS [Column] 
        //                        FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 

        //                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1 
        //                            ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
        //                            AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
        //                            AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME 

        //                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU2 
        //                            ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG  
        //                            AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA 
        //                            AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME 
        //                            AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION 
        //                        WHERE KCU1.TABLE_NAME = @master";
        //        return con.Query<ForeignKeySchema>(query, new { master = master }).ToList();
        //    }
        //}

        //List<String> GetColumnListByFK(string table, string column)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string query = @"DECLARE @COLUMNS VARCHAR(100)
        //                        DECLARE @table_name SYSNAME
        //                        SET @table_name = @master
        //                        SELECT @Columns = @col
        //                        EXEC('SELECT '+ @Columns +' FROM '+ @table_name)";
        //        return con.Query<String>(query, new { master = table, col = column }).ToList();
        //    }
        //}
        //public Type GetType(string text, string dataType)
        //{
        //    Type resultType = null;
        //    if (dataType.ToLower() == "varchar")
        //        resultType = typeof(string);
        //    else if (dataType.ToLower() == "int")
        //        resultType = typeof(int);
        //    else if (dataType.ToLower() == "datetime2")
        //        resultType = typeof(DateTime);
        //    else if (dataType.ToLower() == "decimal")
        //        resultType = typeof(decimal);
        //    else if (dataType.ToLower() == "bit")
        //        resultType = typeof(bool);
        //    else if (dataType.ToLower() == "char")
        //        resultType = typeof(char);
        //    else if (dataType.ToLower() == "uniqueidentifier")
        //    {
        //        Guid result;
        //        Guid.TryParse(text, out result);
        //        resultType = typeof(Guid);
        //    }
        //    else if (dataType.ToLower() == "varbinary")
        //        resultType = typeof(Byte[]);
        //    return resultType;
        //}

        //public bool IsRowUnderLimit(ExcelPackage package, int limit)
        //{
        //    ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
        //    return workSheet.Dimension.End.Row <= limit;
        //}

        //public HashSet<string> SchemaListToHashSet(List<InformationSchemaModel> SchemaList)
        //{
        //    HashSet<String> hashColumnName = new HashSet<string>();
        //    for (int j = 0; j < SchemaList.Count; j++)
        //    {
        //        hashColumnName.Add(SchemaList[j].COLUMN_NAME.ToLower());
        //    }
        //    return hashColumnName;
        //}

        //public bool IsTableTrue(ExcelPackage package, List<InformationSchemaModel> SchemaList)
        //{
        //    int i = 0;
        //    ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

        //    HashSet<String>  hashColumnName = SchemaListToHashSet(SchemaList);

        //    foreach (var firstRowCell in workSheet.Cells[4, 2, 4, workSheet.Dimension.End.Column])
        //    {
        //        i += hashColumnName.Contains(firstRowCell.Text.ToLower()) ? 1 : 0;
        //    }
        //    return i == SchemaList.Count;
        //}

        //object ValidateDataType(InformationSchemaModel schema, string value, ref bool isError)
        //{
        //    if (schema.DATA_TYPE == "int")
        //    {
        //        int result;
        //        if (!int.TryParse(value, out result))
        //        {
        //            if(schema.IS_NULLABLE == "YES" && string.IsNullOrEmpty(value))
        //            {
        //                return DBNull.Value;
        //            }
        //            isError = true;
        //            return schema.COLUMN_NAME + " harus integer";
        //        }
        //        else
        //            return result;
        //    }
        //    if (schema.DATA_TYPE == "decimal")
        //    {
        //        decimal result;
        //        if (!decimal.TryParse(value, out result))
        //        {
        //            if (schema.IS_NULLABLE == "YES" && string.IsNullOrEmpty(value))
        //            {
        //                return DBNull.Value;
        //            }
        //            isError = true;
        //            return schema.COLUMN_NAME + " harus decimal";
        //        }
        //        else
        //            return result;
        //    }
        //    if (schema.DATA_TYPE == "bit")
        //    {
        //        if (value != "0" && value != "1")
        //        {
        //            if (schema.IS_NULLABLE == "YES" && string.IsNullOrEmpty(value))
        //            {
        //                return DBNull.Value;
        //            }
        //            isError = true;
        //            return schema.COLUMN_NAME + " harus 0 atau 1";
        //        }
        //        else
        //        {
        //            return Convert.ToBoolean(Convert.ToInt32(value));
        //        }
        //    }

        //    if (schema.DATA_TYPE == "datetime2")
        //    {
        //        DateTime result;
        //        if (!DateTime.TryParse(value, out result))
        //        {
        //            if (schema.IS_NULLABLE == "YES" && string.IsNullOrEmpty(value))
        //            {
        //                return DBNull.Value;
        //            }
        //            isError = true;
        //            return schema.COLUMN_NAME + " harus datetime";
        //        }
        //        else
        //            return result;
        //    }

        //    if (schema.DATA_TYPE == "uniqueidentifier")
        //    {
        //        Guid result;
        //        if (!Guid.TryParse(value, out result))
        //        {
        //            if (schema.IS_NULLABLE == "YES" && string.IsNullOrEmpty(value))
        //            {
        //                return DBNull.Value;
        //            }
        //            isError = true;
        //            return schema.COLUMN_NAME + " harus guid/uniqueidentifier";
        //        }
        //        else
        //            return result;
        //    }

        //    if (schema.DATA_TYPE == "char")
        //    {
        //        char result;
        //        if (!char.TryParse(value, out result))
        //        {
        //            if (schema.IS_NULLABLE == "YES" && string.IsNullOrEmpty(value))
        //            {
        //                return DBNull.Value;
        //            }
        //            isError = true;
        //            return schema.COLUMN_NAME + " harus 1 character";
        //        }
        //        else
        //            return result;
        //    }
        //    if (schema.DATA_TYPE == "varchar")
        //    {
        //        if (value.Length > schema.CHARACTER_MAXIMUM_LENGTH)
        //        {
        //            if (schema.IS_NULLABLE == "YES" && string.IsNullOrEmpty(value))
        //            {
        //                return DBNull.Value;
        //            }
        //            isError = true;
        //            return schema.COLUMN_NAME + "maximum karakter adalah " + schema.CHARACTER_MAXIMUM_LENGTH;
        //        }
        //        else
        //            return value;
        //    }

        //    if (schema.DATA_TYPE == "varbinary")
        //    {
        //        return Encoding.ASCII.GetBytes(value);
        //    }
        //    return null;
        //}

        //HashSet<string> GetCurrentRow(List<List<string>> uniqueColList, int row)
        //{
        //    HashSet<string> uniqueRow = new HashSet<string>();
        //    if (uniqueColList.Count == 0 || uniqueColList == null)
        //    {
        //        return new HashSet<string>();
        //    }
        //    for (int j = 0; j < uniqueColList[0].Count(); j++)
        //    {
        //        if (row == j)
        //        {
        //            foreach (var item in uniqueColList)
        //            {
        //                uniqueRow.Add(item[row].ToLower().ToString());
        //            }
        //        }
        //    }
        //    return uniqueRow;
        //}

        //DataRow ValidateRow(DataRow dr, ExcelRange row, List<InformationSchemaModel> schemaList, List<UploadDownloadUniqueColumnSchema> uniqueSchema, List<List<string>> uniqueColList, List<List<string>> compositeColList,List<ForeignKeySchema> fkColumn,List<HashSet<string>> allFkList,int currentRow)
        //{
        //    int count = 0;
        //    string message = "";
        //    var unique = uniqueSchema.Select(Q => Q.ColumnName).ToList();
        //    bool isError = false;
        //    foreach (var cell in row)
        //    {
        //        var schema = schemaList[count];
        //        object currentValue = new object();
        //        if (schema.IS_NULLABLE == "NO")
        //        {
        //            if (string.IsNullOrEmpty(cell.Text))
        //            {
        //                message += "Silahkan mengisi " + schema.COLUMN_NAME;
        //                dr[schemaList.Count] = message;
        //                return dr;
        //            }
        //        }
        //        currentValue = ValidateDataType(schema, cell.Text, ref isError);
        //        if (isError)
        //        {
        //            dr[schemaList.Count] = currentValue;
        //            return dr;
        //        }
        //        else
        //            dr[count] = currentValue;
        //        count++;

        //    }
        //    count = 0;


        //    //cek foreign Key
        //    foreach (var cell in row)
        //    {
        //        var schema = schemaList[count];
        //        for (int i = 0; i < fkColumn.Count; i++) {
        //            if (schema.COLUMN_NAME == fkColumn[i].FKColumn )
        //            {
        //                if(schema.IS_NULLABLE=="YES" && string.IsNullOrEmpty(cell.Text))
        //                {
        //                    break;
        //                }
        //                if (!allFkList[i].Contains(cell.Text.ToLower()))
        //                {
        //                    dr[schemaList.Count] = fkColumn[i].FKColumn + " melanggar kode ForeignKey";
        //                    return dr;
        //                }
        //            }
        //        }
        //        count++;
        //    }
        //    var primaryKeyList = uniqueSchema.Where(Q => Q.ConstraintType == "PRIMARY KEY").Select(Q => Q.ColumnName).ToList();
        //    var isComposite = primaryKeyList.Count>1?true:false;
        //    //ifcompositekey
        //    if (isComposite)
        //    {
        //        var dataRow = ValidateCompositeKey(dr, row, schemaList, unique, uniqueColList,compositeColList, primaryKeyList);
        //        if (dataRow != null)
        //        {
        //            dr[schemaList.Count] = dataRow;
        //            return dr;
        //        }
        //    }
        //    else
        //    {
        //        count = 0;
        //        if (uniqueColList.Count != 0)
        //        {
        //            //cek isExists
        //            foreach (var cell in row)
        //            {
        //                var schema = schemaList[count];
        //                for (int i = 0; i < unique.Count; i++)
        //                {
        //                    if (unique[i] == schema.COLUMN_NAME)
        //                    {
        //                        HashSet<string> CurrentRow = GetCurrentRow(uniqueColList, i);
        //                        if (GetCurrentRow(compositeColList,i).Contains(cell.Text.ToLower()))
        //                        {
        //                            dr[schemaList.Count] = schema.COLUMN_NAME + " tidak boleh sama";
        //                            return dr;
        //                        }
        //                        if (CurrentRow.Contains(cell.Text.ToLower()))
        //                        {
        //                            dr[schemaList.Count] = schema.COLUMN_NAME + " sudah terdaftar!";
        //                            return dr;
        //                        }
        //                    }
        //                }
        //                count++;
        //            }
        //        }
        //    }
        //    return dr;
        //}
        //public List<List<string>> DynamicToListString(List<dynamic> Data)
        //{
        //    List<List<string>> uniqueTable = new List<List<string>>();
        //    foreach (var r in Data)
        //    {
        //        List<string> uniqueRow = new List<string>();
        //        foreach (var cell in r)
        //        {
        //            uniqueRow.Add(cell.Value.ToString());
        //        }
        //        uniqueTable.Add(uniqueRow);
        //    }
        //    return uniqueTable;
        //}
        ///// <summary>
        ///// Get All Column that Unique in table
        ///// e.g : Region
        ///// </summary>
        ///// <returns>
        ///// RegionCode,RegionCodeAFI
        ///// </returns>
        //public List<UploadDownloadUniqueColumnSchema> GetUniqueCol(string master)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string query = @"
        //                        select[ColumnName]= b.COLUMN_NAME,[ConstraintType]=a.CONSTRAINT_TYPE
        //                        from INFORMATION_SCHEMA.TABLE_CONSTRAINTS a
        //                        JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE b ON a.CONSTRAINT_NAME = b.CONSTRAINT_NAME
        //                        WHERE a.TABLE_NAME = @master AND CONSTRAINT_TYPE IN('PRIMARY KEY','UNIQUE')
        //                        ";
        //        return con.Query<UploadDownloadUniqueColumnSchema>(query, new { master = master }).ToList();
        //    }
        //}
        ///// <summary>
        ///// Count Primary Key in table
        ///// </summary>
        ///// <returns></returns>
        //public string ValidateCompositeKey(DataRow dr, ExcelRange row, List<InformationSchemaModel> schemaList,List<string> unique,  List<List<string>> uniqueColList, List<List<string>> compositeColList,List<string> primaryKeyList)
        //{
        //    int count = 0;
        //    List<string> temp = new List<string>();
        //    foreach (var cell in row)
        //    {
        //        var schema = schemaList[count];
        //        for (int i = 0; i < primaryKeyList.Count; i++)
        //        {
        //            if (primaryKeyList[i] == schema.COLUMN_NAME)
        //            {
        //                temp.Add(cell.Text.ToLower());
        //            }
        //        }
        //        count++;
        //    }
        //    if (temp.Count == 2)
        //    {
        //        if (compositeColList.Where(Q => Q[0] == temp[0].ToLower() && Q[1] == temp[1].ToLower()).FirstOrDefault() != null)
        //        {
        //            return string.Format("{0} dan {1} tidak boleh sama", unique[0], unique[1]);
        //        }
        //        if (uniqueColList.Where(Q=>Q[0]==temp[0] && Q[1]==temp[1]).FirstOrDefault() != null)
        //        {
        //             return string.Format("{0} dan {1} sudah terdaftar", unique[0], unique[1]);
        //        }
        //    }
        //    else if (temp.Count == 3)
        //    {
        //        if (compositeColList.Where(Q => Q[0] == temp[0].ToLower() && Q[1] == temp[1].ToLower() && Q[2] == temp[2].ToLower()).FirstOrDefault() != null)
        //        {
        //            return string.Format("{0} dan {1} sudah terdaftar", unique[0], unique[1]);
        //        }
        //        if (uniqueColList.Where(Q => Q[0] == temp[0] && Q[1] == temp[1] && Q[2] == temp[2]).FirstOrDefault() != null)
        //        {
        //            return string.Format("{0} dan {1} sudah terdaftar", unique[0], unique[1]);
        //        }
        //    }
        //    return null;
        //}
        ///// <summary>
        ///// Get All content of unique column 
        ///// e.g:
        ///// master: Region
        ///// unique : RegionCode,RegionCodeAFI
        ///// </summary>
        ///// <returns>
        ///// RegionCode1 , RegionCodeAFI1
        ///// RegionCode2, RegionCodeAFI2
        ///// RegionCode3 , RegionCodeAFI3
        ///// RegionCode4, RegionCodeAFI4
        ///// </returns>
        //public List<List<string>> GetUniqueColumnList(string master, List<string> uniqueColumns)
        //{
        //    var con = dbContext.Database.GetDbConnection();
        //    {
        //        string col = "";
        //        uniqueColumns.ForEach(
        //            Q =>
        //            {
        //                col += string.Format("{0},", Q);
        //            }
        //            );
        //        col = col.Remove(col.Length - 1, 1);
        //        string query = @"
        //                        DECLARE @COLUMNS VARCHAR(1000)
        //                        DECLARE @table_name SYSNAME
        //                        SET @table_name = @master
        //                        SELECT @Columns = @col
        //                        EXEC('SELECT '+ @Columns +' FROM '+ @table_name)
        //                        ";
        //        var result = con.Query<dynamic>(query, new { master = master, col = col }).ToList();
        //        return DynamicToListString(result).ConvertAll(Q=>Q.ConvertAll(i=>i.ToLower()));
        //    }
        //}
        //public DataTable toDataTable(ExcelPackage package, string master, List<InformationSchemaModel> schemaList)
        //{
        //    ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
        //    DataTable Dt = new DataTable();
        //    int i = 0;
        //    foreach (var firstRowCell in workSheet.Cells[4, 2, 4, workSheet.Dimension.End.Column])
        //    {
        //        Dt.Columns.Add(firstRowCell.Text, GetType(firstRowCell.Text, schemaList[i].DATA_TYPE));
        //        i++;
        //    }
        //    Dt.Columns.Add("MessageError");
        //    var unique = GetUniqueCol(master);
        //    var uniqueColList = GetUniqueColumnList(master, unique.Select(Q => Q.ColumnName).ToList());
        //    var fkColumn = GetFKReference(master);
        //    var allFkList = GetAllFKList(fkColumn);
        //    var CompositeSchema = unique.Where(Q => Q.ConstraintType == "PRIMARY KEY").ToList();
        //    if (CompositeSchema.Count() > 1)
        //    {
        //        unique = CompositeSchema;
        //    }
        //    var compositeList = GetExcelUniqueColList(package, schemaList, unique, master);
        //    i = 0;
        //    for (var rowNumber = 5; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        //    {
        //        var row = workSheet.Cells[rowNumber, 2, rowNumber, workSheet.Dimension.End.Column];
        //        var newRow = Dt.NewRow();
        //        var compositeListTemp =  compositeList.Where(Q => Q != compositeList[i]).ToList();
        //        newRow = ValidateRow(newRow, row, schemaList, unique, uniqueColList, compositeListTemp, fkColumn, allFkList,i);
        //        Dt.Rows.Add(newRow);
        //        i++;
        //    }
        //    return Dt;
        //}

        //public DataTable toDataTable(ExcelPackage package)
        //{
        //    ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
        //    DataTable Dt = new DataTable();
        //    foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        //    {
        //        Dt.Columns.Add(firstRowCell.Text.Replace(" ", ""));
        //    }
        //    for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        //    {
        //        var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
        //        var newRow = Dt.NewRow();
        //        foreach (var cell in row)
        //        {
        //            newRow[cell.Start.Column - 1] = cell.Text;
        //        }
        //        Dt.Rows.Add(newRow);
        //    }
        //    return Dt;
        //}

        //public List<List<string>> GetExcelUniqueColList(ExcelPackage package, List<InformationSchemaModel> Schema, List<UploadDownloadUniqueColumnSchema> uniqueSchema, string master)
        //{
        //    ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
        //    List<List<string>> columnList = new List<List<string>>();
        //    var unique = uniqueSchema.Select(i => i.ColumnName).ToList();
        //    for (var rowNumber = 5; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        //    {
        //        List<string> columns = new List<string>();
        //        var row = workSheet.Cells[rowNumber, 2, rowNumber, workSheet.Dimension.End.Column];
        //        int count = 0;
        //        foreach (var cell in row)
        //        {
        //            var schema = Schema[count];
        //            for (int i = 0; i < unique.Count; i++)
        //            {
        //                if (unique[i] == schema.COLUMN_NAME)
        //                {
        //                    columns.Add(cell.Text.ToLower());
        //                }
        //            }
        //            count++;
        //        }
        //        columnList.Add(columns);
        //    }
        //    return columnList;
        //}
        // TIE: END
    }

    public interface IExcelPackageExtension
    {
        bool IsRowUnderLimit(ExcelPackage package, int limit);
        bool IsTableTrue(ExcelPackage package, List<InformationSchemaModel> SchemaList);
        DataTable toDataTable(ExcelPackage package, string master, List<InformationSchemaModel> schemaList);

    }
}
