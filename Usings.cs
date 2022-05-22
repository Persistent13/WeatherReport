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

global using System.ServiceModel.Syndication;
global using System.Xml;

global using Google.Api.Gax;
global using Google.Cloud.Firestore;

global using static System.DateTime;
global using static System.Environment;
