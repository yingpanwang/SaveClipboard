//using Npgsql;

//IDbConnection dbConnection = new NpgsqlConnection("Host=127.0.0.1;Port=5432;Database=clipboarddatahistory;Username=postgres;Password=postgres");
//dbConnection.Open();

//IClipboardDataRepository clipboardDataRepository = new ClipboardDataRepository(dbConnection);

//using ClipboardListener l = new(
//   IntPtr.Zero,
//   [
//       new DebugClipboardDataVisitor(),
//       new ClipboardDataKeeper(clipboardDataRepository)
//   ]);

//l.Listen();

//Console.ReadLine();