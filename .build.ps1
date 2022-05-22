# Weather Report. A Slack app to post David Lynch's daily Weather Report videos.
# Copyright (C) 2022 Dakota Clark

# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU Affero General Public License as published
# by the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.

# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU Affero General Public License for more details.

# You should have received a copy of the GNU Affero General Public License
# along with this program.  If not, see <https://www.gnu.org/licenses/>.

task . {
    exec { dotnet build --configuration Release }
}

task emulator {
    $jobname = 'firestore-weatherreport'
    $jobs = Get-Job -Name $jobname -ErrorAction Ignore | Where-Object { $_.State -eq 'Running' }
    if ($null -eq $jobs) {
        $null = Start-Job -Name $jobname -ScriptBlock {
            gcloud beta emulators firestore start --host-port=localhost:8338
        }
    }
}

task envvars {
    $env:FIRESTORE_EMULATOR_HOST = 'localhost:8338'
    $env:GCP_PROJECT_ID = 'weather-report-reporter'
}

task run emulator, envvars, {
    exec { dotnet run --project .\weatherreport.csproj }
}

task clean {
    $env:FIRESTORE_EMULATOR_HOST = $null
    $env:GCP_PROJECT_ID = $null
    Stop-Job -Name 'firestore-weatherreport'
    exec { dotnet clean }
}
