// Weather Report. A Slack app to post David Lynch's daily Weather Report videos.
// Copyright (C) 2022 Dakota Clark

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.

// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

Console.WriteLine($"[UTC {UtcNow}] Starting...");
string ReportDate = UtcNow.ToString("M/d/yy");
Console.WriteLine($"[UTC {UtcNow}] Loading RSS feed");
using var reader = XmlReader.Create("https://www.youtube.com/feeds/videos.xml?channel_id=UCDLD_zxiuyh1IMasq9nbjrA");
var feed = SyndicationFeed.Load(reader);
Console.WriteLine($"[UTC {UtcNow}] RSS feed parsed");

Console.WriteLine($"[UTC {UtcNow}] Filtering RSS feed for today's post");
var report = feed.Items.Where(i =>
    i.LastUpdatedTime.UtcDateTime.Date == UtcNow.Date &&
    i.Title.Text.StartsWith("David Lynch's Weather Report")
// We only ever care to have the latest so [0] it is
).FirstOrDefault()?.Links[0].Uri.ToString();

if (!string.IsNullOrEmpty(report))
{
    Dictionary<string, string> reportRecord = new()
    {
        { "Url", report },
        { "ReportDate", ReportDate }
    };

    Console.WriteLine($"[UTC {UtcNow}] Filtering done, printing results");
    Console.WriteLine(
        "[UTC {3}] There is {0} weather report today, {2}.{1}",
        string.IsNullOrEmpty(report) ? "not presently a" : "a",
        string.IsNullOrEmpty(report) ? string.Empty : $" {report}",
        ReportDate,
        UtcNow
    );

    Console.WriteLine($"[UTC {UtcNow}] Connecting to FireStore");
    FirestoreDbBuilder dbBuilder = new();
    var projectId = GetEnvironmentVariable("GCP_PROJECT_ID");
    // To use the emulator, set the environment variable FIRESTORE_EMULATOR_HOST
    // Make sure to set the env var in context that this code can see it!
    dbBuilder.EmulatorDetection = EmulatorDetection.EmulatorOrProduction;
    dbBuilder.ProjectId = projectId;
    Console.WriteLine($"[UTC {UtcNow}] Connecting using Project ID: {projectId}");
    var db = await dbBuilder.BuildAsync();

    Console.WriteLine($"[UTC {UtcNow}] Conncting to reports collection");
    var collection = db.Collection("reports");
    Console.WriteLine($"[UTC {UtcNow}] Running query");
    var query = collection.WhereEqualTo("Url", report);
    var results = await query.GetSnapshotAsync();
    if (results.Count == 0)
    {
        Console.WriteLine($"[UTC {UtcNow}] Adding new report entry");
        // add results to DB
        await collection.AddAsync(reportRecord);
    }
    else
    {
        Console.WriteLine($"[UTC {UtcNow}] Report already added, nothing to do");
    }
}
else
{
    Console.WriteLine($"[UTC {UtcNow}] RSS feed empty!!!!");
}

Console.WriteLine($"[UTC {UtcNow}] Stopping...");
