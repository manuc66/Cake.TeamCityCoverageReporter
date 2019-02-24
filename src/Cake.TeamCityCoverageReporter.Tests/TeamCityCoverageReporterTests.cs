using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Microsoft.EntityFrameworkCore.Specification.Tests.TestUtilities.Xunit;
using Moq;
using Shouldly;
using Xunit;

namespace Cake.TeamCityCoverageReporter.Tests
{
    public class TeamCityCoverageReporterTests
    {
        private static void WhenReceiveThenExpectTeamCityCoverageReport(string sampleFile, string sampleExpectedResultsPath)
        {
            // Given
            var sampleFilePath = new FilePath(sampleFile);
            var mockCakeContext = new Mock<ICakeContext>();
            var mockCakeLog = new Mock<ICakeLog>();
            mockCakeContext
                .SetupGet(x => x.Log)
                .Returns(mockCakeLog.Object);
            var loggedRows = new List<string>();
            mockCakeLog
                .Setup(x => x.Write(It.IsAny<Verbosity>(), It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<Verbosity, LogLevel, string, object[]>((verbosity, logLevel, format, objects)
                    => loggedRows.Add(string.Format(format, objects)));

            // When
            mockCakeContext.Object.TeamCityCoverageReporter(sampleFilePath);

            // Then
            var expectedRows = File.ReadAllLines(sampleExpectedResultsPath);
            var result = string.Join(Environment.NewLine, loggedRows);
            var expected = string.Join(Environment.NewLine, expectedRows);
            result.ShouldBe(expected);
        }

        [Fact]
        [UseCulture("fr-FR")]
        public void GIVEN_SampleFiles_WithFrench_Culture_WHEN_ReporterCalled_THEN_OutputIsAsExpected()
        {
            GIVEN_SampleFiles_WHEN_ReporterCalled_THEN_OutputIsAsExpected();
        }

        [Fact]
        [UseCulture("en-GB")]
        public void GIVEN_SampleFiles_WithEnglish_Culture_WHEN_ReporterCalled_THEN_OutputIsAsExpected()
        {
            GIVEN_SampleFiles_WHEN_ReporterCalled_THEN_OutputIsAsExpected();
        }

        [Fact]
        public void GIVEN_SampleFiles_WHEN_ReporterCalled_THEN_OutputIsAsExpected()
        {
            const string sampleFile = @".\Sample1.xml";
            const string sampleExpectedResultsPath = @".\Sample1-Expected.txt";
            WhenReceiveThenExpectTeamCityCoverageReport(sampleFile, sampleExpectedResultsPath);
        }

        [Fact]
        public void GIVEN_EmptySampleFiles_WHEN_ReporterCalled_THEN_OutputIsAsExpected()
        {
            // Given
            const string sampleFile = @".\Sample2-Empty.xml";
            const string sampleExpectedResultsPath = @".\Sample2-Empty-Expected.txt";
            WhenReceiveThenExpectTeamCityCoverageReport(sampleFile, sampleExpectedResultsPath);
        }
    }
}
