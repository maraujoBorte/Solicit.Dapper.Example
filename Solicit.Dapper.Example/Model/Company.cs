using Solicit.Dapper.Mapper;
using System;

namespace Solicit.Dapper.Example.Model
{
    //attribute to identify table name, if class have same name to table, isn't necessary
    [TableName("t_Company")]
    public class Company
    {
        //attribute to identify column name, if property have same name to column, isn't necessary
        [ColumnName("Id")]
        [Key] // Identify this column is key to update/delete and not used to insert
        public int IdCompany { get; set; }

        public string Name { get; set; }
        
        public string Adress { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
        
        [ReadOnly] // Identify this column is used only to get data, insert/update not used
        public DateTime DateRegister { get; set; }
    }
}
