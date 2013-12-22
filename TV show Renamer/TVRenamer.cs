using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TV_Show_Renamer
{
    static class TVRenamer
    {
        public static TVClass renameFile(TVClass fileInfo)
        {
			List<string> regexList = new List<string>();
			regexList.Add(standard_repeat);
			regexList.Add(fov_repeat);
			regexList.Add(standard);
			regexList.Add(fov);
			regexList.Add(scene_date_format);
			regexList.Add(stupid);
			regexList.Add(verbose);
			regexList.Add(season_only);
			regexList.Add(no_season_multi_ep);
			regexList.Add(no_season_general);
			regexList.Add(bare);
			regexList.Add(no_season);

			foreach (string regex in regexList) 
			{
				var regexStandard = new Regex(regex, RegexOptions.IgnoreCase);
				string modFileName = fileInfo.FileName.Replace(fileInfo.FileExtention, "");
				Match episode = regexStandard.Match(modFileName);

				string Showname = episode.Groups["series_name"].Value;
				string Season = episode.Groups["season_num"].Value;
				string Episode = episode.Groups["ep_num"].Value;
				string Episode2 = episode.Groups["extra_ep_num"].Value;
				string extra = episode.Groups["extra_info"].Value;
				string group = episode.Groups["release_group"].Value;

				if (!string.IsNullOrEmpty(Showname) && !string.IsNullOrEmpty(Season) && !string.IsNullOrEmpty(Episode) && !string.IsNullOrEmpty(Episode2))
				{
					fileInfo.TVShowName = Showname;
					fileInfo.SeasonNum = Int32.Parse(Season);
					fileInfo.EpisodeNum = Int32.Parse(Episode);
					fileInfo.EpisodeNum2 = Int32.Parse(Episode2);
					fileInfo.FileTitle = extra;
					break;
				}
				else if (!string.IsNullOrEmpty(Showname) && !string.IsNullOrEmpty(Season) && !string.IsNullOrEmpty(Episode))
				{
					fileInfo.TVShowName = Showname;
					fileInfo.SeasonNum = Int32.Parse(Season);
					fileInfo.EpisodeNum = Int32.Parse(Episode);
					fileInfo.FileTitle = extra;
					break;
				}
				
			}
            return fileInfo;
        }

        //all regexes are case insensitive

        static string standard_repeat =
            // Show.Name.S01E02.S01E03.Source.Quality.Etc-Group
            // Show Name - S01E02 - S01E03 - S01E04 - Ep Name
        @"^(?<series_name>.+?)[. _-]+s(?<season_num>\d+)[. _-]*e(?<ep_num>\d+)([. _-]+s(?=season_num)[. _-]*e(?<extra_ep_num>\d+))+[. _-]*((?<extra_info>.+?)((?<![. _-])(?<!WEB)-(?<release_group>[^- ]+))?)?$";

        static string fov_repeat =
            // Show.Name.1x02.1x03.Source.Quality.Etc-Group
            // Show Name - 1x02 - 1x03 - 1x04 - Ep Name
        @"^(?<series_name>.+?)[. _-]+(?<season_num>\d+)x(?<ep_num>\d+)([. _-]+(?=season_num)x(?<extra_ep_num>\d+))+[. _-]*((?<extra_info>.+?)((?<![. _-])(?<!WEB)-(?<release_group>[^- ]+))?)?$";

        static string standard =
            // Show.Name.S01E02.Source.Quality.Etc-Group
            // Show Name - S01E02 - My Ep Name
            // Show.Name.S01.E03.My.Ep.Name
            // Show.Name.S01E02E03.Source.Quality.Etc-Group
            // Show Name - S01E02-03 - My Ep Name
            // Show.Name.S01.E02.E03
        @"^((?<series_name>.+?)[. _-]+)?s(?<season_num>\d+)[. _-]*e(?<ep_num>\d+)(([. _-]*e|-)(?<extra_ep_num>(?!(1080|720)[pi])\d+))*[. _-]*((?<extra_info>.+?)((?<![. _-])(?<!WEB)-(?<release_group>[^- ]+))?)?$";

        static string fov =
            // Show_Name.1x02.Source_Quality_Etc-Group
            // Show Name - 1x02 - My Ep Name
            // Show_Name.1x02x03x04.Source_Quality_Etc-Group
            // Show Name - 1x02-03-04 - My Ep Name
        @"^((?<series_name>.+?)[\[. _-]+)?(?<season_num>\d+)x(?<ep_num>\d+)(([. _-]*x|-)(?<extra_ep_num>(?!(1080|720)[pi])(?!(?<=x)264)\d+))*[\]. _-]*((?<extra_info>.+?)((?<![. _-])(?<!WEB)-(?<release_group>[^- ]+))?)?$";

        static string scene_date_format =
            // Show.Name.2010.11.23.Source.Quality.Etc-Group
            // Show Name - 2010-11-23 - Ep Name
        @"^((?<series_name>.+?)[. _-]+)?(?<air_year>\d{4})[. _-]+(?<air_month>\d{2})[. _-]+(?<air_day>\d{2})[. _-]*((?<extra_info>.+?)((?<![. _-])(?<!WEB)-(?<release_group>[^- ]+))?)?$";

        static string stupid =
            // tpz-abc102
        @"(?<release_group>.+?)-\w+?[\. ]?(?!264)(?<season_num>\d{1,2})(?<ep_num>\d{2})$";

        static string verbose =
            // Show Name Season 1 Episode 2 Ep Name

        @"^(?<series_name>.+?)[. _-]+season[. _-]+(?<season_num>\d+)[. _-]+episode[. _-]+(?<ep_num>\d+)[. _-]+(?<extra_info>.+)$";


        static string season_only =
            // Show.Name.S01.Source.Quality.Etc-Group
        @"^((?<series_name>.+?)[. _-]+)?s(eason[. _-])?(?<season_num>\d+)[. _-]*[. _-]*((?<extra_info>.+?)((?<![. _-])(?<!WEB)-(?<release_group>[^- ]+))?)?$";

        static string no_season_multi_ep =
            // Show.Name.E02-03
            // Show.Name.E02.2010

        @"^((?<series_name>.+?)[. _-]+)?(e(p(isode)?)?|part|pt)[. _-]?(?<ep_num>(\d+|[ivx]+))((([. _-]+(and|&|to)[. _-]+)|-)(?<extra_ep_num>(?!(1080|720)[pi])(\d+|[ivx]+))[. _-])([. _-]*(?<extra_info>.+?)((?<![. _-])(?<!WEB)-(?<release_group>[^- ]+))?)?$";

        static string no_season_general =
            // Show.Name.E23.Test
            // Show.Name.Part.3.Source.Quality.Etc-Group
            // Show.Name.Part.1.and.Part.2.Blah-Group

        @"^((?<series_name>.+?)[. _-]+)?(e(p(isode)?)?|part|pt)[. _-]?(?<ep_num>(\d+|([ivx]+(?=[. _-]))))([. _-]+((and|&|to)[. _-]+)?((e(p(isode)?)?|part|pt)[. _-]?)(?<extra_ep_num>(?!(1080|720)[pi])(\d+|([ivx]+(?=[. _-]))))[. _-])*([. _-]*(?<extra_info>.+?)((?<![. _-])(?<!WEB)-(?<release_group>[^- ]+))?)?$";

        static string bare =
            // Show.Name.102.Source.Quality.Etc-Group

        @"^(?<series_name>.+?)[. _-]+(?<season_num>\d{1,2})(?<ep_num>\d{2})([. _-]+(?<extra_info>(?!\d{3}[. _-]+)[^-]+)(-(?<release_group>.+))?)?$";

        static string no_season =
            // Show Name - 01 - Ep Name
            // 01 - Ep Name
            // 01 - Ep Name

        @"^((?<series_name>.+?)(?:[. _-]{2,}|[. _]))?(?<ep_num>\d{1,2})(?:-(?<extra_ep_num>\d{1,2}))*[. _-]+((?<extra_info>.+?)((?<![. _-])(?<!WEB)-(?<release_group>[^- ]+))?)?$";

    }
}
