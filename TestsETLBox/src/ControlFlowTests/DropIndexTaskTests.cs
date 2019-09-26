using ALE.ETLBox;
using ALE.ETLBox.ConnectionManager;
using ALE.ETLBox.ControlFlow;
using ALE.ETLBox.Helper;
using ALE.ETLBox.Logging;
using ALE.ETLBoxTests.Fixtures;
using System;
using System.Collections.Generic;
using Xunit;

namespace ALE.ETLBoxTests.ControlFlowTests
{
    [Collection("ControlFlow")]
    public class DropIndexTaskTests
    {
        public static IEnumerable<object[]> Connections => Config.AllSqlConnections("ControlFlow");

        public DropIndexTaskTests(ControlFlowDatabaseFixture dbFixture)
        { }

        [Theory, MemberData(nameof(Connections))]
        public void DropIndex(IConnectionManager connection)
        {
            //Arrange
            CreateTableTask.Create(connection, "DropIndexTable", new List<TableColumn>()
            {
                new TableColumn("Test1", "INT")
            });
            CreateIndexTask.CreateOrRecreate(connection, "IndexToDrop","DropIndexTable",
                new List<string>() { "Test1" });
            Assert.True(IfIndexExistsTask.IsExisting(connection, "IndexToDrop", "DropIndexTable"));

            //Act
            DropIndexTask.Drop(connection, "IndexToDrop", "DropIndexTable");

            //Assert
            Assert.False(IfIndexExistsTask.IsExisting(connection, "IndexToDrop", "DropIndexTable"));
        }
    }
}
