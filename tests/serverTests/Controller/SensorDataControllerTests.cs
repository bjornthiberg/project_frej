using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using project_frej.Controllers;
using project_frej.Data;
using project_frej.Models;
using project_frej.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace project_frej.Tests.Controller;
public class SensorDataControllerTests
{
    private readonly Mock<ISensorDataRepository> _mockRepository;
    private readonly Mock<ILogger<SensorDataController>> _mockLogger;
    private readonly Mock<IWebSocketHandler> _mockWebSocketHandler;
    private readonly SensorDataController _controller;

    public SensorDataControllerTests()
    {
        _mockRepository = new Mock<ISensorDataRepository>();
        _mockLogger = new Mock<ILogger<SensorDataController>>();
        _mockWebSocketHandler = new Mock<IWebSocketHandler>();

        _controller = new SensorDataController(
            _mockRepository.Object,
            _mockLogger.Object,
            _mockWebSocketHandler.Object);
    }

    [Fact]
    public void GetRootReturnsRootMessage()
    {
        // Arrange

        // Act
        var result = _controller.GetRoot();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("API for Project Frej", okResult.Value);
    }

    [Fact]
    public async Task PostSensorDataReturnsOkAndDataWhenValidData()
    {
        // Arrange
        var sensorReadingReq = new SensorReadingReq
        {
            Pressure = 1000,
            Temperature = 25,
            Humidity = 50.5F,
            Timestamp = new DateTime(1337, 1, 1)
        };

        var sensorReading = new SensorReading
        {
            Pressure = sensorReadingReq.Pressure,
            Temperature = sensorReadingReq.Temperature,
            Humidity = sensorReadingReq.Humidity,
            Timestamp = sensorReadingReq.Timestamp
        };

        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<SensorReading>()))
               .ReturnsAsync(sensorReading);

        // Act
        var result = await _controller.PostSensordata(sensorReadingReq);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sensorReading, okResult.Value);
    }

    [Fact]
    public async Task PostSensorDataReturnStatuscode500WhenException()
    {
        // Arrange
        var sensorReadingReq = new SensorReadingReq
        {
            Pressure = 1000,
            Temperature = 25,
            Humidity = 50.5F,
            Timestamp = DateTime.UtcNow
        };

        var sensorReading = new SensorReading
        {
            Pressure = sensorReadingReq.Pressure,
            Temperature = sensorReadingReq.Temperature,
            Humidity = sensorReadingReq.Humidity,
            Timestamp = sensorReadingReq.Timestamp
        };

        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<SensorReading>()))
               .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.PostSensordata(sensorReadingReq);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);
    }

    [Fact]
    public async Task PostBulkSensorDataReturnsOkForValidData()
    {
        // Arrange
        var sensorReadingReqs = new List<SensorReadingReq>
            {
                new() { Pressure = 1000, Temperature = 25, Humidity = 50.5F, Timestamp = DateTime.UtcNow },
                new() { Pressure = 1010, Temperature = 26, Humidity = 55.5F, Timestamp = DateTime.UtcNow }
            };

        var sensorReadings = sensorReadingReqs.Select(sr => new SensorReading
        {
            Pressure = sr.Pressure,
            Temperature = sr.Temperature,
            Humidity = sr.Humidity,
            Timestamp = sr.Timestamp
        }).ToList();

        _mockRepository.Setup(repo => repo.AddBulkAsync(It.IsAny<IEnumerable<SensorReading>>()))
                       .ReturnsAsync(sensorReadings);

        // Act
        var result = await _controller.PostBulkSensordata(sensorReadingReqs);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sensorReadings, okResult.Value);
    }

    [Fact]
    public async Task PostBulkSensorDataReturnsStatusCode500WhenExceptionIsThrown()
    {
        // Arrange
        var sensorReadingReqs = new List<SensorReadingReq>
            {
                new() { Pressure = 1000, Temperature = 25, Humidity = 50.5F, Timestamp = DateTime.UtcNow },
                new() { Pressure = 1010, Temperature = 26, Humidity = 55.5F, Timestamp = DateTime.UtcNow }
            };

        _mockRepository.Setup(repo => repo.AddBulkAsync(It.IsAny<IEnumerable<SensorReading>>()))
                       .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.PostBulkSensordata(sensorReadingReqs);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);
    }

    [Fact]
    public async Task GetSensorDataReturnsOkForValidId()
    {
        // Arrange
        int id = 1;
        var sensorReading = new SensorReading
        {
            Id = id,
            Pressure = 1000,
            Temperature = 25,
            Humidity = 50.5F,
            Timestamp = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.GetByIdAsync(id))
                       .ReturnsAsync(sensorReading);

        // Act
        var result = await _controller.GetSensorData(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sensorReading, okResult.Value);
    }

    [Fact]
    public async Task GetSensorDataReturnsNotFoundForInvalidId()
    {
        // Arrange
        int id = 1;

        _mockRepository.Setup(repo => repo.GetByIdAsync(id))
                       .ReturnsAsync((SensorReading?)null);

        // Act
        var result = await _controller.GetSensorData(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetSensorDataReturnsStatusCode500WhenExceptionIsThrown()
    {
        // Arrange
        int id = 1;

        _mockRepository.Setup(repo => repo.GetByIdAsync(id))
                       .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetSensorData(id);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);
    }

    [Fact]
    public async Task GetSensorDataHourlyReturnsOkForValidDateAndHour()
    {
        // Arrange
        var date = new DateTime(1337, 1, 1);
        int hour = 12;
        var sensorReadingHourly = new SensorReadingHourly
        {
            Id = 1,
            Hour = DateTime.Now,
            AvgPressure = 1000,
            MinPressure = 990,
            MaxPressure = 1010,
            AvgTemperature = 25,
            MinTemperature = 20,
            MaxTemperature = 30,
            AvgHumidity = 50,
            MinHumidity = 45,
            MaxHumidity = 55
        };

        _mockRepository.Setup(repo => repo.GetAggregateHourlyAsync(It.IsAny<DateTime>(), It.IsAny<int>()))
            .ReturnsAsync(sensorReadingHourly);


        // Act
        var result = await _controller.GetSensorDataHourly(date, hour);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sensorReadingHourly, okResult.Value);
    }

    [Fact]
    public async Task GetSensorDataHourlyReturnsNotFoundForInvalidDateAndHour()
    {
        // Arrange
        var date = new DateTime(1337, 1, 1);
        int hour = 12;

        _mockRepository.Setup(repo => repo.GetAggregateHourlyAsync(It.IsAny<DateTime>(), It.IsAny<int>()))
                       .ThrowsAsync(new ArgumentNullException());

        // Act
        var result = await _controller.GetSensorDataHourly(date, hour);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetSensorDataHourlyReturnsStatusCode500WhenExceptionIsThrown()
    {
        // Arrange
        var date = new DateTime(1337, 1, 1);
        int hour = 12;

        _mockRepository.Setup(repo => repo.GetAggregateHourlyAsync(It.IsAny<DateTime>(), It.IsAny<int>()))
                       .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetSensorDataHourly(date, hour);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);
    }

    [Fact]
    public async Task GetSensorDataDailyReturnsOkForValidDate()
    {
        // Arrange
        var date = new DateTime(1337, 1, 1);
        var sensorReadingDaily = new SensorReadingDaily
        {
            Id = 1,
            Date = date,
            AvgPressure = 1000,
            MinPressure = 990,
            MaxPressure = 1010,
            AvgTemperature = 25,
            MinTemperature = 20,
            MaxTemperature = 30,
            AvgHumidity = 50,
            MinHumidity = 45,
            MaxHumidity = 55
        };

        _mockRepository.Setup(repo => repo.GetAggregateDailyAsync(It.IsAny<DateTime>()))
                       .ReturnsAsync(sensorReadingDaily);

        // Act
        var result = await _controller.GetSensorDataDaily(date);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sensorReadingDaily, okResult.Value);
    }

    [Fact]
    public async Task GetSensorDataDailyReturnsNotFoundForInvalidDate()
    {
        // Arrange
        var date = new DateTime(1337, 1, 1);

        _mockRepository.Setup(repo => repo.GetAggregateDailyAsync(It.IsAny<DateTime>()))
                       .ThrowsAsync(new ArgumentNullException());

        // Act
        var result = await _controller.GetSensorDataDaily(date);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetSensorDataDailyReturnsStatusCode500WhenExceptionIsThrown()
    {
        // Arrange
        var date = new DateTime(1337, 1, 1);

        _mockRepository.Setup(repo => repo.GetAggregateDailyAsync(It.IsAny<DateTime>()))
                       .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetSensorDataDaily(date);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);
    }


    [Fact]
    public async Task GetSensorDataPagedReturnsOkForValidPage()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var pagedResult = new PagedResult<SensorReading>
        {
            TotalRecords = 100,
            TotalPages = 10,
            CurrentPage = pageNumber,
            PageSize = pageSize,
            Data = [new SensorReading()]
        };
        _mockRepository.Setup(repo => repo.GetPagedAsync(pageNumber, pageSize)).ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetSensorDataPaged(pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        dynamic response = okResult?.Value!;
        Assert.NotNull(response);
        Assert.Equal(100, (int)response!.TotalRecords);
        Assert.Equal(10, (int)response.TotalPages);
        Assert.Equal(pageNumber, (int)response.CurrentPage);
        Assert.Equal(pageSize, (int)response.PageSize);
        Assert.IsType<List<SensorReading>>(response.Data);
        Assert.NotNull(response.Data);
    }

    [Fact]
    public async Task GetSensorDataPagedReturnsNotFoundForInvalidPage()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 100;

        _mockRepository.Setup(repo => repo.GetPagedAsync(pageNumber, pageSize))
                       .ThrowsAsync(new ArgumentNullException());

        // Act
        var result = await _controller.GetSensorDataPaged(pageNumber, pageSize);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetSensorDataPagedReturnsStatusCode500WhenExceptionIsThrown()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 100;

        _mockRepository.Setup(repo => repo.GetPagedAsync(pageNumber, pageSize))
                       .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetSensorDataPaged(pageNumber, pageSize);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);
    }

    [Fact]
    public async Task GetAllSensorDataReturnsOkForValidRequest()
    {
        // Arrange
        var sensorReadings = new List<SensorReading>
            {
                new() { Id = 1, Pressure = 1000, Temperature = 25, Humidity = 50.5F, Timestamp = DateTime.UtcNow },
                new() { Id = 2, Pressure = 1010, Temperature = 26, Humidity = 55.5F, Timestamp = DateTime.UtcNow }
            };

        _mockRepository.Setup(repo => repo.GetAllAsync())
                       .ReturnsAsync(sensorReadings);

        // Act
        var result = await _controller.GetAllSensorData();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sensorReadings, okResult.Value);
    }

    [Fact]
    public async Task GetAllSensorDataReturnsNotFoundForNoData()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllAsync())
                       .ThrowsAsync(new ArgumentNullException());

        // Act
        var result = await _controller.GetAllSensorData();

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAllSensorDataReturnsStatusCode500WhenExceptionIsThrown()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllAsync())
                       .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetAllSensorData();

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemResult.StatusCode);
    }

}

