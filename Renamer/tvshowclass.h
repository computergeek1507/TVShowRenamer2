#ifndef TVSHOWCLASS_H
#define TVSHOWCLASS_H

#include <QString>

class TVShowClass
{
public:
    TVShowClass();
    TVShowClass(QString fileFolder, QString fileName, QString fileExtention);

private:
    QString _fileFolder;//origonal folder
    QString _fileName;//origonal file name
    QString _fileExtention;//origonal file Extention
    QString _fileTitle;//="";//Title files
    QString _newFileName;//new file name
    bool _auto;// = true;//autoconvert
    int _tvShowID;// = -1;//TVDB id number
    int _seasonNum;// = -1;//Season Number
    int _episodeNum;// = -1;//Episode Number
    QString _TVShowName;// = ""; //Show Name
    bool _getTitle;// = true;//autoGetTitle
};

#endif // TVSHOWCLASS_H
