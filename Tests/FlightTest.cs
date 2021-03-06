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

    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      //Arrange, Act
      DateTime departureTime = new DateTime(2017, 3, 14, 9, 30, 0);
      string flightStatus = "On Time";
      Flight firstFlight = new Flight("AX5390", departureTime, flightStatus);
      Flight secondFlight = new Flight("AX5390", departureTime, flightStatus);

      //Assert
      Assert.Equal(firstFlight, secondFlight);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      DateTime departureTime = new DateTime(2017, 3, 14, 9, 30, 0);
      string flightStatus = "On Time";
      Flight testFlight = new Flight("AX5390", departureTime, flightStatus);
      testFlight.Save();

      //Act
      List<Flight> resultList = Flight.GetAll();
      List<Flight> testList = new List<Flight>{testFlight};

      //Assert
      Assert.Equal(testList, resultList);
    }

    [Fact]
    public void Test_Save_AssignedIdtoFlight()
    {
      //Arrange
      DateTime departureTime = new DateTime(2017, 3, 14, 9, 30, 0);
      string flightStatus = "On Time";
      Flight testFlight = new Flight("AX5390", departureTime, flightStatus);
      testFlight.Save();
      Flight savedFlight = Flight.GetAll()[0];

      //Act
      int savedId = savedFlight.GetId();
      int testId = testFlight.GetId();

      //Assert
      Assert.Equal(testId,savedId);
    }

    [Fact]
    public void Test_GetAll_ReturnsListofFlights()
    {
      //Arrange
      DateTime firstDepartureTime = new DateTime(2017, 3, 14, 9, 30, 0);
      DateTime secondDepartureTime = new DateTime(2007, 3, 14, 9, 30, 0);
      string flightStatus = "On Time";
      Flight firstFlight = new Flight("AX5390", firstDepartureTime, flightStatus);
      Flight secondFlight = new Flight("B45390", secondDepartureTime, flightStatus);

      //Act
      firstFlight.Save();
      secondFlight.Save();

      //Assert
      List<Flight> actualResult = Flight.GetAll();
      List<Flight> expectedResult = new List<Flight>{secondFlight, firstFlight};

      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void Test_Find_ReturnFlightFromDatabase()
    {
      //Arrange
      DateTime departureTime = new DateTime(2017, 3, 14, 9, 30, 0);
      string flightStatus = "On Time";
      Flight testFlight = new Flight("AX5390", departureTime, flightStatus);
      testFlight.Save();

      //Act
      Flight foundFlight = Flight.Find(testFlight.GetId());

      //Assert
      Assert.Equal(testFlight, foundFlight);
    }

    [Fact]
    public void Test_Update_UpdatesFlightStatusinDB()
    {
      //Arrange
      DateTime departureTime = new DateTime(2017, 3, 14, 9, 30, 0);
      string flightStatus = "On Time";
      Flight testFlight = new Flight("AX5390", departureTime, flightStatus);
      testFlight.Save();

      string newFlightStatus = "Delayed";

      //Act
      testFlight.UpdateStatus(newFlightStatus);


      //Assert
      string actualResult = testFlight.GetStatus();
      string expectedResult = newFlightStatus;

      Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void Test_DeleteThisFlight_OneFlightDeletedFromDatabase()
    {//Arrange
      DateTime departureTime = new DateTime(2017, 3, 14, 9, 30, 0);
      string flightStatus = "On Time";
      Flight firstFlight = new Flight("AX5390", departureTime, flightStatus);
      Flight secondFlight = new Flight("BD890", departureTime, flightStatus);
      firstFlight.Save();
      secondFlight.Save();

      //Act
      firstFlight.DeleteThisFlight();
      List<Flight> result = Flight.GetAll();
      List<Flight> verify = new List<Flight>{secondFlight};

      //Assert
      Assert.Equal(verify, result);
    }

    [Fact]
    public void Test_AddDepartureCity_AddsDepartureCityToFlight()
    {
      //Arrange
      DateTime departureTime = new DateTime(2017, 3, 14, 9, 30, 0);
      string flightStatus = "On Time";
      Flight testFlight = new Flight("AX5390", departureTime, flightStatus);
      testFlight.Save();

      City departureCity = new City("Seattle");
      departureCity.Save();

      //Act
      testFlight.AddDepartureCity(departureCity);

      List<City> result = testFlight.GetDepartureCity();
      List<City> testList = new List<City>{departureCity};

      //Assert
      Assert.Equal(testList, result);
    }

    public void Dispose()
    {
      Flight.DeleteAll();
    }
  }
}
