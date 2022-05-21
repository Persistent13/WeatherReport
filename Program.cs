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

Console.WriteLine($"[UTC {UtcNow}] Filtering done, printing results");
Console.WriteLine(
    "[UTC {3}] There is {0} weather report today, {2}.{1}",
    string.IsNullOrEmpty(report) ? "not presently a" : "a",
    string.IsNullOrEmpty(report) ? string.Empty : $" {report}",
    UtcNow.ToString("M/d/yy"),
    UtcNow
);
Console.WriteLine($"[UTC {UtcNow}] Stopping...");
