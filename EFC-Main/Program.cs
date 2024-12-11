using Examples;

await ClassExamples.DeleteDatabase();
await ClassExamples.CreateDatabase();
await ClassExamples.InsertRecords();
await ClassExamples.ReadRecordsFromProductTable();
await ClassExamples.UpdateRecordInProductTable();
await ClassExamples.DeleteRecordFromProductTable();