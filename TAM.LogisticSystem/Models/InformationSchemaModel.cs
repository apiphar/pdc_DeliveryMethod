using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class InformationSchemaModel
    {
        public string COLUMN_NAME { get; set; }
        public string IS_NULLABLE { get; set; }
        public string DATA_TYPE { get; set; }
        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
    }

    public class ForeignKeySchema
    {
        public string FKColumn { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
    }
}
