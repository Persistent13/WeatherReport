Console.WriteLine($"[UTC {UtcNow}] Starting...");
Console.WriteLine($"[UTC {UtcNow}] Loading RSS feed");
using var reader = XmlReader.Create("https://www.youtube.com/feeds/videos.xml?channel_id=UCDLD_zxiuyh1IMasq9nbjrA");
var feed = SyndicationFeed.Load(reader);
Console.WriteLine($"[UTC {UtcNow}] RSS feed parsed");

Console.WriteLine($"[UTC {UtcNow}] Filtering RSS feed for today's post");
var report = feed.Items.Where(i =>
    i.LastUpdatedTime.UtcDateTime.Date == UtcNow.Date &&
    i.Title.Text.StartsWith("David Lynch's Weather Report") &&
    i.Links.Any(l =>
        !string.IsNullOrEmpty(l.Uri.ToString())
    )
// We only ever care to have the latest so [0] it is
).FirstOrDefault()?.Links[0].Uri.ToString();
Console.WriteLine($"[UTC {UtcNow}] Filtering done, printing results");

Console.WriteLine(
    "There is {0} weather report today, {2}.{1}",
    string.IsNullOrEmpty(report) ? "not presently a" : "a",
    string.IsNullOrEmpty(report) ? string.Empty : $" {report}",
    DateTime.UtcNow.ToString("M/d/yy")
);
Console.WriteLine($"[UTC {UtcNow}] Stopping...");
