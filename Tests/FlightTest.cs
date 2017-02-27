using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirlineApp
{
  public class FlightTest: IDisposable
  {
    public FlightTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange,Act
      int result = Flight.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    public void Dispose()
    {
      Flight.DeleteAll();
    }
  }
}
