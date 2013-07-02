using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Show_Renamer_Server
{
	static class TVRenamer
	{
		public static string renameFile(string fileName) 
		{
			string temp = "";

			return fileName;
		}

		//all regexes are case insensitive

		static string standard_repeat = 
		// Show.Name.S01E02.S01E03.Source.Quality.Etc-Group
		// Show Name - S01E02 - S01E03 - S01E04 - Ep Name
		@"^(?P<series_name>.+?)[. _-]+s(?P<season_num>\d+)[. _-]*e(?P<ep_num>\d+)([. _-]+s(?P=season_num)[. _-]*e(?P<extra_ep_num>\d+))+[. _-]*((?P<extra_info>.+?)((?<![. _-])(?<!WEB)-(?P<release_group>[^- ]+))?)?$";

		static string fov_repeat =
		// Show.Name.1x02.1x03.Source.Quality.Etc-Group
		// Show Name - 1x02 - 1x03 - 1x04 - Ep Name
		@"^(?P<series_name>.+?)[. _-]+(?P<season_num>\d+)x(?P<ep_num>\d+)([. _-]+(?P=season_num)x(?P<extra_ep_num>\d+))+[. _-]*((?P<extra_info>.+?)((?<![. _-])(?<!WEB)-(?P<release_group>[^- ]+))?)?$";

		static string standard =
		// Show.Name.S01E02.Source.Quality.Etc-Group
		// Show Name - S01E02 - My Ep Name
		// Show.Name.S01.E03.My.Ep.Name
		// Show.Name.S01E02E03.Source.Quality.Etc-Group
		// Show Name - S01E02-03 - My Ep Name
		// Show.Name.S01.E02.E03
		@"^((?P<series_name>.+?)[. _-]+)?s(?P<season_num>\d+)[. _-]*e(?P<ep_num>\d+)(([. _-]*e|-)(?P<extra_ep_num>(?!(1080|720)[pi])\d+))*[. _-]*((?P<extra_info>.+?)((?<![. _-])(?<!WEB)-(?P<release_group>[^- ]+))?)?$";

		static string fov =
		// Show_Name.1x02.Source_Quality_Etc-Group
		// Show Name - 1x02 - My Ep Name
		// Show_Name.1x02x03x04.Source_Quality_Etc-Group
		// Show Name - 1x02-03-04 - My Ep Name
		@"^((?P<series_name>.+?)[\[. _-]+)?(?P<season_num>\d+)x(?P<ep_num>\d+)(([. _-]*x|-)(?P<extra_ep_num>(?!(1080|720)[pi])(?!(?<=x)264)\d+))*[\]. _-]*((?P<extra_info>.+?)((?<![. _-])(?<!WEB)-(?P<release_group>[^- ]+))?)?$";

		static string scene_date_format =
		// Show.Name.2010.11.23.Source.Quality.Etc-Group
		// Show Name - 2010-11-23 - Ep Name
		@"^((?P<series_name>.+?)[. _-]+)?(?P<air_year>\d{4})[. _-]+(?P<air_month>\d{2})[. _-]+(?P<air_day>\d{2})[. _-]*((?P<extra_info>.+?)((?<![. _-])(?<!WEB)-(?P<release_group>[^- ]+))?)?$";

		static string stupid =
		// tpz-abc102
		@"(?P<release_group>.+?)-\w+?[\. ]?(?!264)(?P<season_num>\d{1,2})(?P<ep_num>\d{2})$";

		static string verbose =
		// Show Name Season 1 Episode 2 Ep Name
		
		@"^(?P<series_name>.+?)[. _-]+season[. _-]+(?P<season_num>\d+)[. _-]+episode[. _-]+(?P<ep_num>\d+)[. _-]+(?P<extra_info>.+)$";


		static string season_only =
		// Show.Name.S01.Source.Quality.Etc-Group
		@"^((?P<series_name>.+?)[. _-]+)?s(eason[. _-])?(?P<season_num>\d+)[. _-]*[. _-]*((?P<extra_info>.+?)((?<![. _-])(?<!WEB)-(?P<release_group>[^- ]+))?)?$";

		static string no_season_multi_ep =
		// Show.Name.E02-03
		// Show.Name.E02.2010
		
		@"^((?P<series_name>.+?)[. _-]+)?(e(p(isode)?)?|part|pt)[. _-]?(?P<ep_num>(\d+|[ivx]+))((([. _-]+(and|&|to)[. _-]+)|-)(?P<extra_ep_num>(?!(1080|720)[pi])(\d+|[ivx]+))[. _-])([. _-]*(?P<extra_info>.+?)((?<![. _-])(?<!WEB)-(?P<release_group>[^- ]+))?)?$";

		static string no_season_general =
		// Show.Name.E23.Test
		// Show.Name.Part.3.Source.Quality.Etc-Group
		// Show.Name.Part.1.and.Part.2.Blah-Group
		
		@"^((?P<series_name>.+?)[. _-]+)?(e(p(isode)?)?|part|pt)[. _-]?(?P<ep_num>(\d+|([ivx]+(?=[. _-]))))([. _-]+((and|&|to)[. _-]+)?((e(p(isode)?)?|part|pt)[. _-]?)(?P<extra_ep_num>(?!(1080|720)[pi])(\d+|([ivx]+(?=[. _-]))))[. _-])*([. _-]*(?P<extra_info>.+?)((?<![. _-])(?<!WEB)-(?P<release_group>[^- ]+))?)?$";
		
		static string bare =
		// Show.Name.102.Source.Quality.Etc-Group
		
		@"^(?P<series_name>.+?)[. _-]+(?P<season_num>\d{1,2})(?P<ep_num>\d{2})([. _-]+(?P<extra_info>(?!\d{3}[. _-]+)[^-]+)(-(?P<release_group>.+))?)?$";
		
		static string no_season =
		// Show Name - 01 - Ep Name
		// 01 - Ep Name
		// 01 - Ep Name
		
		@"^((?P<series_name>.+?)(?:[. _-]{2,}|[. _]))?(?P<ep_num>\d{1,2})(?:-(?P<extra_ep_num>\d{1,2}))*[. _-]+((?P<extra_info>.+?)((?<![. _-])(?<!WEB)-(?P<release_group>[^- ]+))?)?$";

	}
}
