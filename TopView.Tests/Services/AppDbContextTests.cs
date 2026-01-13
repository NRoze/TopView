using System.Threading.Tasks;
using TopView.Model.Data;
using Xunit;
using System.IO;

namespace TopView.Tests.Services
{
 public class AppDbContextTests
 {
 [Fact]
 public async Task InitializeTables_CreatesDatabaseFile()
 {
 var path = Path.Combine(Path.GetTempPath(), "testdb.db");
 if (File.Exists(path)) File.Delete(path);

 var db = new AppDbContext(path);
 await db.InitializeTables();

 // After initialization, the underlying sqlite file should exist
 Assert.True(File.Exists(path));

 // Clean up
 try { File.Delete(path); } catch { }
 }
 }
}
