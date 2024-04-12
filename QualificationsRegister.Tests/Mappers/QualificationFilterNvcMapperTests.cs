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
                { "GradingTypes", "Numeric, Pass/Fail" },
                { "MinTotalQualificationTime", "120" },
                { "MaxTotalQualificationTime", "300" }
            };

            var result = nvc.GetQualificationFilterQuery();

            result.Should().NotBeNull();
            result?.AssessmentMethods?.Length.Should().Be(2);
            result?.AssessmentMethods?.Should().BeEquivalentTo(["Numeric", "Pass/Fail"]);
            result?.MinTotalQualificationTime?.Should().Be(120);
            result?.MaxTotalQualificationTime?.Should().Be(300);
        }

        [Test]
        public void GetQualificationFilterQuery_WithEmptyCollection_ReturnsNull()
        {
            var nvc = new NameValueCollection();

            Func<QualificationFilter?> testDelegate = () => nvc.GetQualificationFilterQuery();
            testDelegate.Should().BeNull();
        }

        [Test]
        public void GetQualificationFilterQuery_WithInvalidIntParameter_ThrowsBadRequestException()
        {
            var nvc = new NameValueCollection
            {
                { "MinTotalQualificationTime", "abc" } // Invalid value for an integer parameter
            };

            Func<QualificationFilter?> testDelegate = () => nvc.GetQualificationFilterQuery();
            testDelegate.Should().Throw<BadRequestException>();
        }
    }
}
