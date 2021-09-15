using Solicit.Dapper.Example.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Solicit.Dapper.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //Example to using with SqlConnection instance. If using dependecy injection, using with IDbConnection;

            //Connection string
            using (SqlConnection connection = new(""))
            {
                var teste = new List<Company>();

                for (int i = 0; i < 10; i++)
                {
                    var companyToList = new Company()
                    {
                        Name = $"Solicit-{i}",
                        DateRegister = DateTime.Now,
                        Adress = "R. Neverland N 34",
                        City = "Sao Paulo",
                        Country = "Brasil"
                    };

                    teste.Add(companyToList);

                    //insert void
                    connection.Add(companyToList);

                    //insert with return inserted
                    var add = connection.AddOutValue(companyToList);

                    //insert with return inserted Asynchrnous, async class without .Result, put await
                    var addAsync = connection.AddOutValueAsync(companyToList).Result;
                }

                //Bulk operation void.
                connection.AddBulk(teste);

                //Bulk operations out inserted values.
                var bulkOut = connection.AddBulkOutValue(teste);

                //Bulk operations out inserted values, Asynchrnous, async class without .Result, put await
                var bulkOutAsync = connection.AddBulkOutValueAsync(teste).Result;

                //Get all rows with table
                var getAll = connection.GetAll<Company>();

                //Get all table rows, pass table name, ideal to used with temp table
                var getAllTableName = connection.GetAll<Company>("t_Company");

                //Get top X row to table
                var getAllTopRows = connection.GetAll<Company>(10);

                //Get all rows with table, async class without .Result, put await
                var getAllAsync = connection.GetAllAsync<Company>().Result;

                //Get top X row to table, async class without .Result, put await
                var getAllTopRowsAsync = connection.GetAllAsync<Company>(10).Result;

                //Get row with WHERE filter, all time used referenced value with ==, with bool property to. Ex. isbool == true. Even if property is already true, use == true
                var getWhere = connection.GetWhere<Company>(x => x.Country == "Brazil" || x.Country == "Canada");

                //Get row with WHERE filter, all time used referenced value with ==, with bool property to. Ex. isbool == true. Even if property is already true, use == true
                //Async class without .Result, put await
                var getWhereAsync = connection.GetWhereAsync<Company>(x => x.Country == "Brazil" || x.Country == "Canada").Result;

                //Get row with WHERE filter, all time used referenced value with ==, with bool property to. Ex. isbool == true. Even if property is already true, use == true
                //ideal to used with temp table
                var getWhereWithTableName = connection.GetWhere<Company>(x => x.Country == "Brazil" || x.Country == "Canada", "t_Company");

                //Get row with WHERE filter, all time used referenced value with ==, with bool property to. Ex. isbool == true. Even if property is already true, use == true
                //Async class without .Result, put await
                //ideal to used with temp table
                var getWhereAsyncWithTableName = connection.GetWhereAsync<Company>(x => x.Country == "Brazil" || x.Country == "Canada", "t_Company").Result;

                //get row with table key
                var getById = connection.GetById<Company>(4);

                //get row with table key
                //Async class without .Result, put await
                var getByIdAsync = connection.GetByIdAsync<Company>(3).Result;

                //check if rows exists with table key and return true or false
                var existById = connection.ExistsById<Company>(2);

                //check if rows exists with table key and return true or false
                //Async class without .Result, put await
                var existByIdAsync = connection.ExistsByIdAsync<Company>(1).Result;

                //check if rows exists with table filter WHERE and return true or false
                var existsWhere = connection.ExistsWhere<Company>(x => x.Country == "Brazil");

                //check if rows exists with table filter WHERE and return true or false
                //Async class without .Result, put await
                var existsWhereAsync = connection.ExistsWhereAsync<Company>(x => x.Country == "Brazil").Result;

                //Remove row with table key - DELETE COMMAND
                connection.Remove(getAll.FirstOrDefault());

                //Remove list with table key - DELETE COMMAND
                connection.Remove(getAllTopRows);

                //Remove row with table key - DELETE COMMAND
                //Async class without .Result, put await
                //Return deleted row
                var removedListRowAsync = connection.RemoveAsync(getAllTopRows).Result;

                //Remove row with table key - DELETE COMMAND
                //Async class without .Result, put await
                //Return deleted row
                var removedRowAsync = connection.RemoveAsync(getAll.FirstOrDefault()).Result;

                //Remove row with table key - DELETE COMMAND
                //Async class without .Result, put await
                //Return deleted row
                var removedById = connection.RemoveByIdAsync<Company>(1).Result;

                //UPDATE row with table key - UPDATE COMMAND
                connection.Update(getAll.FirstOrDefault());

                //UPDATE row with table key - UPDATE COMMAND
                //Async class without .Result, put await
                //Return updated row
                var updatedAsync = connection.UpdateAsync(getAll.FirstOrDefault()).Result;

                //UPDATE List with table key - UPDATE COMMAND
                //Async class without .Result, put await
                //Return updated row
                var updateListAsync = connection.UpdateAsync(getAll).Result;

                //Create table tempo for using
                using (var con = new SqlConnection(connection.ConnectionString))
                {
                    //only create table empty rows. Return temp table name
                    var tempTable = con.CreateTableTemp<Company>();

                    //only create table with rows.Return temp table name
                    var tempTableWithRow = con.CreateTableTemp(getAllTopRows);

                    var getAllToTemp = con.GetAll<Company>(tempTableWithRow);

                    //only create table with rows.Return temp table name
                    var tempTableWithRowName = con.CreateTableTemp("TempTableSolicit", getAll);

                    //only create table empty rows.Return temp table name
                    //Async class without .Result, put await
                    var tempTableWithRowNameAsync = con.CreateTableTempAsync<Company>("TempTableSolicitAsync").Result;

                    //only create table empty rows.Return temp table name
                    //Async class without .Result, put await
                    var tempTableAsync = con.CreateTableTempAsync<Company>().Result;
                }

                var merge = connection.Merge(getAllTopRows)
                    .UpdateWhere(x => x.Country == "Brazil")
                    .DeleteWhenNotMatched(x => x.City == "Sao Paulo")
                    .EvaluateQuery(true);

                //Join with Where
                var selectJoinWithWhere = connection
                     .Select()
                     .Join<Company, Employer>((x, y) => x.IdCompany == y.IdCompany)
                     .EvaluateQuery<Company>();

                //Left Join with Where
                var selectLeftJoinWithWhere = connection
                    .Select()
                    .LeftJoin<Company, Employer>((x, y) => x.IdCompany == y.IdCompany)
                    .EvaluateQueryWhere<Employer>(x => x.IdCompany == null);

                //Join get all data with top 10 Rows
                var selectJoinWith = connection
                     .Select()
                     .Join<Company, Employer>((x, y) => x.IdCompany == y.IdCompany)
                     .LeftJoin<Employer, Company>((x, y) => x.Adress == y.Adress)
                     .EvaluateQueryWhere<Employer>(x => x.City == "Sao Paulo");

                //Left Join get all data
                var selectLeftJoin = connection
                    .Select()
                    .LeftJoin<Company, Employer>((x, y) => x.IdCompany == y.IdCompany)
                    .EvaluateQuery<Company>();

            }
        }
    }
}
