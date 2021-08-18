using System;
using System.Collections.Generic;
using System.Linq;
using CSVParser.Core.TrackFiles.Fixtures;
using CSVParser.Core.TrackFiles.TrackBunches.TrackEvents;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;

namespace CSVParser.Core.TrackFiles.TrackBunches
{
    [TestFixture]
    public class TrackBunchTests
    {
        [Test]
        public void Validate_ok_events_order_should_return_empty_result_v1()
        {
            // arrange
            var dt = DateTime.Now;
            var te1 = TrackEventFixture.Create(x => x.EventDate(dt).Status(Registered));
            var te2 = TrackEventFixture.Create(x => x.EventDate(dt).AddToEventDate().Status(ReceivedWh));
            var te3 = TrackEventFixture.Create(x => x.EventDate(dt).AddToEventDate().Status(Delivered));

            var sut = _fixture.CreateSut(te1, te2, te3);
            // act
            var actual = sut.Validate();
            // assert
            actual.Should().NotBeNull().And.BeEmpty();
        }

        [Test]
        public void Validate_ok_events_order_should_return_empty_result_v2()
        {
            // arrange
            var dt = DateTime.Now;
            var te1 = TrackEventFixture.Create(x => x.EventDate(dt).Status(Registered));
            var te2 = TrackEventFixture.Create(x => x.EventDate(dt).AddToEventDate().Status(ReceivedWh));
            var te4 = TrackEventFixture.Create(x => x.EventDate(dt).AddToEventDate().Status(Registered));

            var sut = _fixture.CreateSut(te1, te2, te4);
            // act
            var actual = sut.Validate();
            // assert
            actual.Should().NotBeNull().And.BeEmpty();
        }

        [Test]
        public void Validate_incompatible_events_should_return_error()
        {
            // arrange
            var dt = DateTime.Now;
            var te1 = TrackEventFixture.Create(x => x.EventDate(dt).Status(Registered));
            var te2 = TrackEventFixture.Create(x => x.EventDate(dt).AddToEventDate().Status(ReceivedWh));
            var te3 = TrackEventFixture.Create(x => x.EventDate(dt).AddToEventDate().Status(Delivered));
            var te4 = TrackEventFixture.Create(x => x.EventDate(dt).AddToEventDate().Status(Returned));

            var sut = _fixture.CreateSut(te1, te2, te3, te4);
            // act
            var actual = sut.Validate();
            // assert
            actual.Should().NotBeNullOrEmpty("expected one validation error");
            actual.Should().HaveCount(1);
            var expected = new ValidatorEventArgs
                {Success = false, Severity = SeverityLevel.Fatal, Issue = IssueKind.IncompatibleEvents};
            actual.Single().Should().BeEquivalentTo(expected, WithSuccessSeverityAndIssueKind);
        }
        
        [Test]
        public void Validate_wrong_event_order_should_return_error()
        {
            // arrange
            var dt = DateTime.Now;
            var te1 = TrackEventFixture.Create(x => x.EventDate(dt).Status(Registered));
            var te2 = TrackEventFixture.Create(x => x.EventDate(dt).AddToEventDate(10).Status(ReceivedWh));
            var te3 = TrackEventFixture.Create(x => x.EventDate(dt).AddToEventDate(1).Status(Delivered));

            var sut = _fixture.CreateSut(te1, te2, te3);
            // act
            var actual = sut.Validate();
            // assert
            actual.Should().NotBeNullOrEmpty("expected one validation error");
            actual.Should().HaveCount(1);
            var expected = new ValidatorEventArgs
                {Success = false, Severity = SeverityLevel.Fatal, Issue = IssueKind.WrongEventOrder};
            actual.Single().Should().BeEquivalentTo(expected, WithSuccessSeverityAndIssueKind);
        }

        private EquivalencyAssertionOptions<ValidatorEventArgs> WithSuccessSeverityAndIssueKind(
            EquivalencyAssertionOptions<ValidatorEventArgs> arg)
            => arg
                .Including(x => x.Success)
                .Including(x => x.Severity)
                .Including(x => x.Issue);

        private static readonly TrackStatus Registered = TrackStatus.PackageRegistered;
        private static readonly TrackStatus ReceivedWh = TrackStatus.PackageReceivedAtWarehouse;
        private static readonly TrackStatus Delivered  = TrackStatus.Delivered;
        private static readonly TrackStatus Returned   = TrackStatus.ReturnedToSender;

        #region Test Helpers

        private TrackBunchTestsFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new TrackBunchTestsFixture();
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion
    }

    public class TrackBunchTestsFixture
    {
        public TrackBunch CreateSut(params TrackEvent[] args)
        {
            return new(args);
        }
    }
}