#include "tvshowclass.h"

TVShowClass::TVShowClass()
{


}

TVShowClass::TVShowClass(QString fileFolder, QString fileName, QString fileExtention)
{
    _fileFolder = fileFolder;
    _fileName = _newFileName = fileName;
    _fileExtention = fileExtention;
    _auto = true;
    _fileTitle = "";
    _tvShowID = -1;
    _seasonNum = -1;
    _episodeNum = -1;
    _TVShowName = "";
    _getTitle = true;
}

void TVShowClass::Reset()
{
    _fileTitle = "";
    _newFileName = "";
    _auto = true;
    _tvShowID = -1;
    _seasonNum = -1;
    _episodeNum = -1;
    _TVShowName = "";
    _getTitle = true;
}
