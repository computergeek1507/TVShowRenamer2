using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Show_Renamer_Server
{
	public class TVShowID
	{
		string _TVShowName;
		int _TVID;
		public TVShowID(string tVShowName, int tVID)
		{
			_TVShowName = tVShowName;
			_TVID = tVID;
		}
		public string TVShowName
		{
			get { return _TVShowName; }
			set { _TVShowName = value; }
		}
		public int TVID
		{
			get { return _TVID; }
			set { _TVID = value; }
		}

	};
	public class LoadFileInfo
	{
		string _fileName;
		string _fileFolder;
		string _fileExt;
		public LoadFileInfo(string filename, string filefolder,string fileext)
		{
			_fileName = filename;
			_fileFolder = filefolder;
			_fileExt = fileext;
		}
		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}
		public string FileFolder
		{
			get { return _fileFolder; }
			set { _fileFolder = value; }
		}
		public string FileExtension
		{
			get { return _fileExt; }
			set { _fileExt = value; }
		}
		public string FullFileDirectly 
		{
			get { return _fileFolder + "//" + _fileName; }
		}

	};

	public class CategoryInfo 
	{
		string _categoryTitle;
		string _commandWords;
		string _searchFolder;
		int _folderOptions;
		//string _folderOptions;
		public CategoryInfo(string categoryTitle, string commandWords, string searchFolder, int folderOptions)
		{
			_categoryTitle = categoryTitle;
			_commandWords = commandWords;
			_folderOptions = folderOptions;
			_searchFolder = searchFolder;
		}

		public CategoryInfo()
		{}

		public string CategoryTitle
		{
			get { return _categoryTitle; }
			set { _categoryTitle = value; }
		}
		public string CommandWords
		{
			get { return _commandWords; }
			set { _commandWords = value; }
		}
		public string SearchFolder
		{
			get { return _searchFolder; }
			set { _searchFolder = value; }
		}
		public int FolderOptions
		{
			get { return _folderOptions; }
			set { _folderOptions = value; }
		}
	};

	public class TVClass
	{
		//string _fileFolder;//origonal folder
		string _fileName;//origonal file name
		//string _fileExtention;//origonal file Extention
		string _fileTitle="";//Title files		
		string _newFileName;//new file name
		string _ShowName = null;
		//bool _auto = true;//autoconvert
		int _tvShowID = -1;//TVDB id number
		int _seasonNum = -1;//Season Number 
		int _episodeNum = -1;//Episode Number

		public TVClass(string fileName/*, string fileFolder, string fileExtention*/)
		{
			//_fileFolder = fileFolder;
			_fileName = _newFileName = fileName;
			//_fileExtention = fileExtention;
			//_auto = true;
		}

		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}

		public string NewFileName
		{
			get { return _newFileName; }
			set { _newFileName = value; }
		}
		/*
		public string FileFolder
		{
			get { return _fileFolder; }
			set { _fileFolder = value; }
		}

		public string FileExtention
		{
			get { return _fileExtention; }
			set { _fileExtention = value; }
		}
		
		public string FullFileName
		{
			get { return _fileFolder +"\\"+ _fileName; }
		}

		public string NewFullFileName
		{
			get { return _fileFolder + "\\" + _newFileName; }
		}

		public bool AutoEdit 
		{
			get { return _auto; }
			set { _auto = value; }
		}
		*/
		public string FileTitle
		{
			get { return _fileTitle; }
			set { _fileTitle = value; }
		}

		public string ShowName
		{
			get { return _ShowName; }
			set { _ShowName = value; }
		}

		public int TVShowID
		{
			get { return _tvShowID; }
			set { _tvShowID = value; }
		}

		public int SeasonNum
		{
			get { return _seasonNum; }
			set { _seasonNum = value; }
		}

		public int EpisodeNum
		{
			get { return _episodeNum; }
			set { _episodeNum = value; }
		}
	}//end of class

	public class TVShowSettings
	{
		string _SearchName;
		string _ShowFolder ;
		string _ShowFolderHD;
		string _TVDBShowName ;
		string _TMDbShowName ;

		int _TVDBSeriesID = -1;//TVDB id number
		int _TMDbSeriesID = -1;

		//bool _UseTVDBNumbering = false;
		bool _SeriesEnded = false;

		bool _getHD = false;

		bool _skip = false;

		SeriesData _seriesEpisodes = new SeriesData();

		//public TVShowSettings(string searchName, string showFolder, string tVDBShowName, int tVDBSeriesID, string tVRageShowName, int tVRageSeriesID)
		//{
		//	_SearchName = searchName;
		//	_ShowFolder = showFolder;
		//	_TVDBShowName = tVDBShowName;
		//	_TVRageShowName = tVRageShowName;
		//	_TVRageSeriesID = tVRageSeriesID;
		//	_TVDBSeriesID = tVDBSeriesID;
		//}
		public TVShowSettings(string searchName,string showFolder)
		{
			_SearchName = searchName;
			_ShowFolder = showFolder;
		}

		public TVShowSettings(string searchName, string showFolder, string showFolderHD)
		{
			_SearchName = searchName;
			_ShowFolder = showFolder;
			_ShowFolderHD = showFolderHD;
		}
		public TVShowSettings(string searchName, string showFolderHD,bool HDEnable)
		{
			_SearchName = searchName;
			_ShowFolderHD = showFolderHD;
			_getHD = HDEnable;
		}

		public TVShowSettings()
		{}

		public string SearchName
		{
			get { return _SearchName; }
			set { _SearchName = value; }
		}
		public string ShowFolder
		{
			get { return _ShowFolder; }
			set { _ShowFolder = value; }
		}
		public string ShowFolderHD
		{
			get { return _ShowFolderHD; }
			set { _ShowFolderHD = value; }
		}

		public string TVDBShowName
		{
			get { return _TVDBShowName; }
			set { _TVDBShowName = value; }
		}
		public string TMDbShowName
		{
			get { return _TMDbShowName; }
			set { _TMDbShowName = value; }
		}
		public int TVDBSeriesID
		{
			get { return _TVDBSeriesID; }
			set { _TVDBSeriesID = value; }
		}
		public int TMDbSeriesID
		{
			get { return _TMDbSeriesID; }
			set { _TMDbSeriesID = value; }
		}
		//public bool UseTVDBNumbering
		//{
		//	get { return _UseTVDBNumbering; }
		//	set { _UseTVDBNumbering = value; }
		//}
		public bool SeriesEnded
		{
			get { return _SeriesEnded; }
			set { _SeriesEnded = value; }
		}
		public bool GetHD
		{
			get { return _getHD; }
			set { _getHD = value; }
		}
		public bool SkipShow
		{
			get { return _skip; }
			set { _skip = value; }
		}
		public SeriesData SeriesEpisodes
		{
			get { return _seriesEpisodes; }
			set { _seriesEpisodes = value; }
		}


	}//end of class

	public class SearchInfo
	{
		string _title;
		string _newTitle = "";
		int _selected;

		public SearchInfo(string title, int selected)
		{
			_title = title;
			_selected = selected;
		}

		public SearchInfo(string title, string newTitle, int selected)
		{
			_title = title;
			_selected = selected;
			_newTitle = newTitle;
		}

		public SearchInfo()
		{
			_title = "";
			_selected = -1;
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public string NewTitle
		{
			get { return _newTitle; }
			set { _newTitle = value; }
		}

		public int SelectedValue
		{
			get { return _selected; }
			set { _selected = value; }
		}

	}

	public class OnlineShowInfo
	{
		string _ShowName;
		//string _newTitle = "";
		int _ShowID;
		string _StartYear;
		bool _ShowEnded = true;

		public OnlineShowInfo(string showName, int showID,string startYear,bool showEnded)
		{
			_ShowName = showName;
			_ShowID = showID;
			_StartYear = startYear;
			_ShowEnded = showEnded;
		}

		public OnlineShowInfo(string showName, int showID)
		{
			_ShowName = showName;
			_ShowID = showID;
			_StartYear = "";
		}

		public OnlineShowInfo(string showName, int showID, string startYear)
		{
			_ShowName = showName;
			_ShowID = showID;
			_StartYear = startYear;
		}

		public OnlineShowInfo()
		{
			_ShowName = "";
			_ShowID = -1;
			_StartYear = "";
			_ShowEnded = true;
		}

		public string ShowName
		{
			get { return _ShowName; }
			set { _ShowName = value; }
		}

		public int ShowID
		{
			get { return _ShowID; }
			set { _ShowID = value; }
		}

		public string StartYear
		{
			get { return _StartYear; }
			set { _StartYear = value; }
		}

		public bool ShowEnded
		{
			get { return _ShowEnded; }
			set { _ShowEnded = value; }
		}
	}

	public class ThreadAdd { public string AddType; public object ObjectToAdd;};

	public class piccache
	{
		public int banner;
		public int poster;
	}

	public class tvdbinfo 
	{
		public bool air_by_date;
		public piccache cache;
		public string language;
		public string next_ep_airdate;
		public bool paused;
		public string quality;
		public string show_name;
		public string status;
		public int tvrage_id;
		public string tvrage_name;

	
	}

	public class SeriesData
	{
		public SeriesData() { }
		//public override String ToString()
		//{
		//	return SeasonNumber.ToString();
		//}
		
		List<SeasonData> _seasonList = new List<SeasonData>();

		public bool ContainsSeason(int season)
		{
			return _seasonList.Any(item => item.SeasonNumber == season);
		}
		public bool ContainsEpisode(int season, int episode)
		{
			SeasonData seasonTemp = _seasonList.FirstOrDefault(item => item.SeasonNumber == season);

			if (seasonTemp == null)
				return false;

			return seasonTemp.ContainsEpisode(episode);
		}

		public List<SeasonData> SeasonList
		{
			get { return _seasonList; }
			set { _seasonList = value; }
		}

		public SeasonData GetSeason(int season)
		{
			return _seasonList.FirstOrDefault(item => item.SeasonNumber == season);
		}

		public int SeasonCount()
		{
			return _seasonList.Count();
		}

		//public override String ToString()
		//{
		//	return SeasonNumber.ToString();
		//}
	}

	public class SeasonData
	{
		public SeasonData() { }

		public SeasonData(int seasonNum) 
		{
			SeasonNumber = seasonNum;
		}

		public override String ToString()
		{
			return SeasonNumber.ToString() ;
		}
		//public string Name { get; set; }
		public int SeasonNumber { get; set; }
		public string SeasonName
		{
			get { return "Season_" + SeasonNumber.ToString(); }
		}
		List<EpisodeData> _epidsodeList = new List<EpisodeData>();
		public int EpisodeCount() { return _epidsodeList.Count; }

		public bool ContainsEpisode(int episode)
		{
			return _epidsodeList.Any(item => item.EpisodeNumber == episode);
		}

		public EpisodeData GetEpisode(int episode)
		{
			return _epidsodeList.FirstOrDefault(item => item.EpisodeNumber == episode);
		}

		public List<EpisodeData> EpisodeList
		{
			get { return _epidsodeList; }
			set { _epidsodeList = value; }
		}
	}

	public class EpisodeData
	{
		public EpisodeData()
		{
		}

		public EpisodeData(string episodeTile, int seasonNum, int episodeNum)
		{
			EpisodeTitle = episodeTile;
			SeasonNumber = seasonNum;
			EpisodeNumber = episodeNum;
			Status = EpisodeStatus.Skip;
			FileName = string.Empty;
		}

		public string EpisodeTitle { get; set; }
		public int SeasonNumber { get; set; }
		public int EpisodeNumber { get; set; }
		public EpisodeStatus Status { get; set; }
		
		public string FileName { get; set; }

		//public string FileFolder { get; set; }

		public override String ToString()
		{
			return SeasonNumber + "x" + EpisodeNumber + (EpisodeTitle != null ? " " + EpisodeTitle : "");
		}
	}

	public enum EpisodeStatus
	{
		Skip,
		Wanted,
		Snached,
		Downloaded
	}

	public class EPGuigeReturnObject
	{
		string _tvshowName;
		string _episodeNumber;
		string _episodeNumber2;
		string _episodeDate;
		string _episodeTitle;

		public EPGuigeReturnObject(string tvshowName, string episodeNumber, string episodeNumber2, string episodeDate, string episodeTitle)
		{
			_tvshowName = tvshowName;
			_episodeNumber = episodeNumber;
			_episodeNumber2 = episodeNumber2;
			_episodeDate = episodeDate;
			_episodeTitle = episodeTitle;

		}
		public string TVShowName
		{
			get { return _tvshowName; }
			set { _tvshowName = value; }
		}

		public string EpisodeNumber
		{
			get { return _episodeNumber; }
			set { _episodeNumber = value; }
		}

		public string EpisodeNumber2
		{
			get { return _episodeNumber2; }
			set { _episodeNumber2 = value; }
		}

		public string EpisodeDate
		{
			get { return _episodeDate; }
			set { _episodeDate = value; }
		}

		public string EpisodeTitle
		{
			get { return _episodeTitle; }
			set { _episodeTitle = value; }
		}
	}//end of class

}//end of namespace
