using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;


namespace TV_show_Renamer
{
    public class TVRage
    {
        //string folder = null;
        //int format = -1;
        //int season = -1;
        //int episode = -1;
        //string tvdbTitle = null;
        //string fileName = "";
        //int TVShowID = -1;

        public string findTitle(string tvdbTitle, int season,int episode)
        {
            string finalTitle = "%%%%";
            if (tvdbTitle == null)
                return finalTitle;

            Show MainInfo = this.FindShow(tvdbTitle);
            if (MainInfo.Seasons.Count >= season - 1) {
                if (MainInfo.Seasons[season - 1].Episodes.Count >= episode - 1)
                {
                    finalTitle = MainInfo.Seasons[season - 1].Episodes[episode - 1].Title.ToString();
                    finalTitle = finalTitle.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");          
                }            
            }
            return finalTitle;
        }
        
        public string infoFinder(string fileName, int format, int season, int episode)
        {
            string tvdbTitle = null;
            string test = fileName;
            int you = -1;
            int i = season;
            int j = episode;

            string newi = season.ToString();
            string newj = j.ToString();
            //check if i is less than 10
            if (i < 10)
                newi = "0" + i.ToString();
            //check if j is less than 10
            if (j < 10)
                newj = "0" + j.ToString();
            //make string to compare changed name too
            switch (format)
            {
                case 1:
                    you = test.IndexOf(i.ToString() + "x" + newj);
                    break;
                case 2:
                    you = test.IndexOf(newi + newj);
                    break;
                case 3:
                    you = test.IndexOf("S" + newi + "E" + newj);
                    //you = test.IndexOf("S" + newi + "e" + newj);
                    break;
                case 4:
                    you = test.IndexOf(i.ToString() + newj);
                    break;
            }
            //stop loop when name is change                    
            if (you != -1)
            {
                season = i;
                episode = j;
                if (you == 0)
                    tvdbTitle = test.Remove(you, test.Length - (you));
                else
                    tvdbTitle = test.Remove(you - 1, test.Length - (you - 1));
                //tvdbTitle = test.Remove(you - 1, test.Length - (you - 1));
            }
            return tvdbTitle;
        }
        
        private List<Show> Cache = new List<Show>();

        private Show FindShow(string showName)
        {
            return FindShow(showName, true);
        }

        //http://www.tvrage.com/feeds/episode_list.php?show=Lost
        private Show FindShow(string showName, bool checkCache)
        {
            if (checkCache)
            {
                foreach (Show shows in Cache)
                {
                    if (shows.Name.ToLowerInvariant() == showName.ToLowerInvariant())
                    {
                        return shows;
                    }
                }
            }

            Show show = new Show(showName);
            try
            {
                XElement xml = XDocument.Load("http://www.tvrage.com/feeds/episode_list.php?show=" + showName).Element("Show");
                show.Name = xml.Element("name").Value;
                show.TotalSeasons = xml.Element("totalseasons").Value;

                foreach (XElement seasons in xml.Element("Episodelist").Elements())
                {
                    Season season = new Season();
                    if (seasons.Attribute("no") != null)
                    {
                        season.SeasonNumber = int.Parse(seasons.Attribute("no").Value);
                    }
                    foreach (XElement episodes in seasons.Elements("episode"))
                    {
                        season.Episodes.Add(new Episode(episodes));
                    }
                    show.Seasons.Add(season);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Cache.Add(show);
            return show;
        }
    }

    public class Show : IEnumerable
    {
        public string Name;
        public string TotalSeasons;
        public List<Season> Seasons = new List<Season>();

        public Show() { }

        public Show(string showName)
        {
            this.Name = showName;
        }

        public Season FindSeason(int number)
        {
            foreach (Season season in Seasons)
            {
                if (season.SeasonNumber == number)
                {
                    return season;
                }
            }
            return null;
        }

        public Episode FindEpisode(int season, int episode)
        {
            Season findSeason = FindSeason(season);
            if (findSeason != null)
            {
                return findSeason.FindEpisode(episode);
            }
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return Seasons.GetEnumerator();
        }
    }
    public class Season : IEnumerable
    {
        public int SeasonNumber;
        public List<Episode> Episodes = new List<Episode>();

        public Season() { }

        public Season(int seasonNumber)
        {
            SeasonNumber = seasonNumber;
        }

        public Episode FindEpisode(int number)
        {
            foreach (Episode episode in Episodes)
            {
                if (int.Parse(episode.SeasonNumber) == number)
                {
                    return episode;
                }
            }
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return Episodes.GetEnumerator();
        }
    }
    public class Episode
    {
        public string EpisodeNumber;
        public string SeasonNumber;
        public string ProdNumber;
        public string AirDate;
        public string Link;
        public string Title;

        public Episode() { }

        public Episode(int episodeNumber, string title)
        {
            SeasonNumber = episodeNumber.ToString();
            Title = title;
        }

        public Episode(XElement xe)
        {
            EpisodeNumber = xe.ToString("epnum");
            SeasonNumber = xe.ToString("seasonnum");
            ProdNumber = xe.ToString("prodnum");
            AirDate = xe.ToString("airdate");
            Link = xe.ToString("link");
            Title = xe.ToString("title");
        }
    }
    public static class Extensions
    {
        public static string ToString(this XElement xe, string name)
        {
            XElement xeItem = xe.Element(name);
            if (xeItem != null)
            {
                return xeItem.Value;
            }
            else
            {
                return "";
            }
        }

        public static int ToInt(this XElement xe, string name)
        {
            XElement xeItem = xe.Element(name);
            if (xeItem != null)
            {
                int number;
                bool result = int.TryParse(xeItem.Value, out number);
                if (result)
                {
                    return number;
                }
            }
            return -1;
        }
    }
}
