using System;
using System.Collections.Generic;
using System.Linq;
using CSVParser.Core.TrackFiles.TrackBunches.TrackEvents;
using FluentAssertions;
using NUnit.Framework;

namespace CSVParser.Core.TrackFiles
{
    [TestFixture]
    public class TrackFileBuilderTests
    {
        [Test]
        public void Build_empty_args_should_return_empty_result()
        {
            // arrange
            var args = Enumerable.Empty<TrackEventDto>();
            var sut = _fixture.CreateSut();
            using var monitoredSut = sut.Monitor();
            // act
            var actual = sut.Build(from: args);
            // assert
            actual.Should().BeNullOrEmpty();
            monitoredSut.Should().NotRaise("Validated");
            monitoredSut.Should().NotRaise("ValidationIssue");
        }

        [Test]
        public void Build_one_bunch_should_raise_validated_and_not_raise_validation_issue_events()
        {
            // arrange
            var args = _fixture.CreateValidTrackEventGroup(tn: "TN01", count: 3);
            var sut = _fixture.CreateSut();
            using var monitoredSut = sut.Monitor();
            // act
            var actual = sut.Build(args);
            // assert
            monitoredSut.Should().Raise("Validated");
            monitoredSut.Should().NotRaise("ValidationIssue");
        }

        #region Test Helpers

        private TrackFileBuilderTestsFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new TrackFileBuilderTestsFixture();
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion
    }

    public class TrackFileBuilderTestsFixture
    {
        public static TrackStatusCache StatusCache { get; } = new()
        {
            { 1, TrackStatus.PackageRegistered },
            { 2, TrackStatus.PackageReceivedAtWarehouse },
            { 3, TrackStatus.Delivered },
            { 4, TrackStatus.ReturnedToSender },
        };

        public ITrackBuilder<TrackFile> CreateSut()
        {
            return new TrackFileBuilder(StatusCache);
        }

        public IEnumerable<TrackEventDto> CreateValidTrackEventGroup(string tn, int count)
        {
            var rowId = 0;
            var now = DateTime.Now;
            var statusId = 1;
            return Enumerable
                .Range(0, count)
                .Select(x => new TrackEventDto
                {
                    RowNum = rowId++,
                    TrackNum = tn,
                    EventDate = now.AddMinutes(rowId),
                    EventStatusId = statusId++,
                    EventState = "CA",
                    EventCity = "LA",
                    Comment = "any"
                })
            ;
        }
    }
}