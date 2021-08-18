using System;
using System.Collections.Generic;
using System.Linq;
using CSVParser.Core.TrackFiles.Fixtures;
using FluentAssertions;
using NUnit.Framework;

namespace CSVParser.Core.TrackFiles.TrackBunches
{
    [TestFixture]
    public class TrackBunchBuilderTests
    {
        [Test]
        public void Validate_ok_arguments_should_raise_Validated_but_no_ValidationIssue()
        {
            // arrange
            var arg = _fixture.CreateTrackEventDtoList(count: 3);
            var sut = _fixture.CreateSut();
            // act
            sut.Build(arg);
            // assert
            _fixture.Subscriber.AssertRaiseEvent("Validated", expected: 3);
            _fixture.Subscriber.AssertNotRaiseEvent("ValidationIssue");
        }

        #region Test Helpers

        private TrackBunchBuilderTestsFixtures _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new TrackBunchBuilderTestsFixtures();
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion
    }

    public class Subscriber
    {
        private readonly Dictionary<string, List<EventArgs>> _cache = new();

        public void AddEvent(string eventName, EventArgs eventArgs)
        {
            if (_cache.TryGetValue(eventName, out List<EventArgs> events))
            {
                events.Add(eventArgs);
            }
            else
            {
                _cache[eventName] = new List<EventArgs> {eventArgs};
            }
        }

        public void AssertRaiseEvent(string eventName, int expected)
        {
            if (_cache.TryGetValue(eventName, out List<EventArgs> eventArgs))
            {
                eventArgs.Should().HaveCount(expected);
            }
            else
            {
                throw new Exception($"Event '{eventName}' was not raised");
            }
        }
        public void AssertNotRaiseEvent(string eventName)
        {
            if (_cache.TryGetValue(eventName, out List<EventArgs> eventArgs))
            {
                eventArgs.Should().BeNullOrEmpty();
            }
        }
    }

    public class TrackBunchBuilderTestsFixtures
    {
        public static TrackStatusCache StatusCache { get; } = new()
        {
            { 1, TrackStatus.PackageRegistered },
            { 2, TrackStatus.PackageReceivedAtWarehouse },
            { 3, TrackStatus.Delivered },
            { 4, TrackStatus.ReturnedToSender },
        };

        public readonly Subscriber Subscriber = new();

        public TrackBunchBuilder CreateSut()
        {
            var sut = new TrackBunchBuilder(StatusCache);
            sut.Validated += SutOnValidated;
            sut.ValidationIssue += SutOnValidationIssue;
            return sut;
        }

        private void SutOnValidated(object? sender, ValidatorEventArgs e)
        {
            Subscriber.AddEvent("Validated", e);
        }

        private void SutOnValidationIssue(object? sender, ValidatorEventArgs e)
        {
            Subscriber.AddEvent("ValidationIssue", e);
        }

        public IEnumerable<TrackEventDto> CreateTrackEventDtoList(int count)
        {
            var rowId = 0;
            var now = DateTime.Now;
            const string tn = "TN1";
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