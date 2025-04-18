using FluentAssertions;
using Moq;
using NUnit.Framework.Legacy;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfqualCommon.RegisterAPI.Tests.Mappers
{
    public class QualificationFilterNvcMapperTests
    {
        [Test]
        public void GetQualificationFilterQuery_WithValidParameters_ReturnsQualificationFilter()
        {
            var nvc = new NameValueCollection
            {
                { "AssessmentMethods", "Exam, Assignment" },
                { "GradingTypes", "Graded, Pass/Fail" },
                { "MinTotalQualificationTime", "120" },
                { "MaxTotalQualificationTime", "300" }
            };

            var result = nvc.GetQualificationFilterQuery();

            result.Should().NotBeNull();
            result?.AssessmentMethods?.Length.Should().Be(2);
            result?.GradingTypes?.Should().BeEquivalentTo(["Graded", "Pass/Fail"]);
            result?.MinTotalQualificationTime?.Should().Be(120);
            result?.MaxTotalQualificationTime?.Should().Be(300);
        }

        [Test]
        public void GetQualificationFilterQuery_WithEmptyCollection_ReturnsNull()
        {
            var nvc = new NameValueCollection();

            var filter =  nvc.GetQualificationFilterQuery();
            filter.Should().BeNull();
        }

        [Test]
        public void GetQualificationFilterQuery_WithInvalidIntParameter_ThrowsBadRequestException()
        {
            var nvc = new NameValueCollection
            {
                { "MinTotalQualificationTime", "abc" } // Invalid value for an integer parameter
            };

            //Assert.That(nvc.GetQualificationFilterQuery().thr)

            Func<QualificationFilter?> testDelegate = () => nvc.GetQualificationFilterQuery();
            testDelegate.Should().Throw<BadRequestException>();
        }

        [Test]
        [TestCase("intentionToSeekFundingInEngland=true", true)]
        [TestCase("intentionToSeekFundingInEngland=1", true)]
        [TestCase("intentionToSeekFundingInEngland=false", false)]
        [TestCase("intentionToSeekFundingInEngland=0", false)]
        [TestCase("intentionToSeekFundingInEngland=asdf", null)]
        [TestCase("intentionToSeekFundingInEngland=null", null)]
        [TestCase("intentionToSeekFundingInEngland=2", null)]
        [TestCase("differentParameterForOtherFlag=true", null)]
        [TestCase("differentParameterForOtherFlag=false", null)]
        public void GetQualificationFilterQuery_WithIntentionToSeekFundingInEngland_ReturnsExpected(string requestedQuery, bool? expected)
        {
            //Arrange
            var rpq = requestedQuery.Split('=', 2, StringSplitOptions.TrimEntries);
            var nvc = new NameValueCollection { { rpq.First(), rpq.Last() } };

            //Act
            var result = nvc.GetQualificationFilterQuery();

            //Assert

            result.Should().NotBeNull();
            result!.IntentionToSeekFundingInEngland.Should().Be(expected);
        }
    }
}
