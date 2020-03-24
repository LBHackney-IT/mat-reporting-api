using MaTReportingAPI.V1.Domain;
using System;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Domain
{
    public class MaTProcessDataTests
    {
        private MatProcessData processData;

        public MaTProcessDataTests()
        {
            processData = new MatProcessData();
        }

        [Fact]
        public void process_data_has_id()
        {
            Assert.Null(processData.Id);
        }

        [Fact]
        public void process_has_process_type()
        {
            Assert.Null(processData.ProcessType);
        }
        [Fact]
        public void process_data_has_date_created()
        {
            DateTime date = new DateTime(2019, 11, 21);
            processData.DateCreated = date;
            Assert.Equal(date, processData.DateCreated);
        }

        [Fact]
        public void process_data_has_date_last_modified()
        {
            DateTime date = new DateTime(2019, 11, 21);
            processData.DateLastModified = date;
            Assert.Equal(date, processData.DateLastModified);
        }
        [Fact]
        public void process_data_has_date_completed()
        {
            DateTime date = new DateTime(2019, 11, 21);
            processData.DateCompleted = date;
            Assert.Equal(date, processData.DateCompleted);
        }

        [Fact]
        public void process_data_has_data_schema_version()
        {
            Assert.Equal(0, processData.ProcessDataSchemaVersion);
        }

        [Fact]
        public void process_data_has_process_stage()
        {
            Assert.Null(processData.ProcessStage);
        }

        [Fact]
        public void process_data_has_pre_process_data()
        {
            Assert.Null(processData.PreProcessData);
        }

        [Fact]
        public void process_data_has_process_data_object()
        {
            Assert.Null(processData.ProcessData);
        }
    }
}
