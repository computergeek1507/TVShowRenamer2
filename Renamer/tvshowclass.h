#ifndef TVSHOWCLASS_H
#define TVSHOWCLASS_H

#include <QString>
#include <QDir>

class TVShowClass
{
public:
    TVShowClass();
    TVShowClass(QString fileFolder, QString fileName, QString fileExtention);
    void Reset();

    const QString& FileFolder    () const { return _fileFolder;    }
    const QString& FileName      () const { return _fileName;      }
    const QString& FileExtention () const { return _fileExtention; }
    const QString& FileTitle     () const { return _fileTitle;     }
    const QString& NewFileName   () const { return _newFileName;   }
    const QString& TVShowName    () const { return _TVShowName;    }
    const bool& Auto             () const { return _auto;          }
    const bool& GetTitle         () const { return _getTitle;      }
    const int& TvShowID          () const { return _tvShowID;      }
    const int& SeasonNum         () const { return _seasonNum;     }
    const int& EpisodeNum        () const { return _episodeNum;    }

    const QString& FullFileName    () const {
		return _fileFolder + /* QString(QDir::separator())*/"\\" + _fileName;  
	}
    const QString& NewFullFileName () const { 
		return _fileFolder +/* QString(QDir::separator())*/"\\" + _newFileName;
	}

    void setFileFolder    ( const QString& FileFolder    ){ _fileFolder    = FileFolder;    }
    void setFileName      ( const QString& FileName      ){ _fileName      = FileName;      }
    void setFileExtention ( const QString& FileExtention ){ _fileExtention = FileExtention; }
    void setFileTitle     ( const QString& FileTitle     ){ _fileTitle     = FileTitle;     }
    void setNewFileName   ( const QString& NewFileName   ){ _newFileName   = NewFileName;   }
    void setTVShowName    ( const QString& TVShowName    ){ _TVShowName    = TVShowName;    }
    void setAuto          ( const bool& Auto             ){ _auto          = Auto;          }
    void setGetTitle      ( const bool& GetTitle         ){ _getTitle      = GetTitle;      }
    void setTvShowID      ( const int&  TvShowID         ){ _tvShowID      = TvShowID;      }
    void setSeasonNum     ( const int&  SeasonNum        ){ _seasonNum     = SeasonNum;     }
    void setEpisodeNum    ( const int&  EpisodeNum       ){ _episodeNum    = EpisodeNum;    }


private:
    QString _fileFolder;//origonal folder
    QString _fileName;//origonal file name
    QString _fileExtention;//origonal file Extention
    QString _fileTitle;//="";//Title files
    QString _newFileName;//new file name
    bool    _auto;// = true;//autoconvert
    int     _tvShowID;// = -1;//TVDB id number
    int     _seasonNum;// = -1;//Season Number
    int     _episodeNum;// = -1;//Episode Number
    QString _TVShowName;// = ""; //Show Name
    bool    _getTitle;// = true;//autoGetTitle
};

#endif // TVSHOWCLASS_H
