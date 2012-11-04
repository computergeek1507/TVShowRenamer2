#ifndef TVSHOWMODEL_H
#define TVSHOWMODEL_H

#include <QAbstractTableModel>
#include <QList>
#include <QString>
#include <QVector>
#include "tvshowclass.h"

#define NEWFILENAME_COLUMN 0
#define FILENAME_COLUMN 1
#define FILEFOLDER_COLUMN 2
#define FILETITLE_COLUMN 3
#define TVSHOWNAME_COLUMN 4
#define TVSHOWID_COLUMN 5
#define SEASONNUM_COLUMN 6
#define EPISODENUM_COLUMN 7

#define NEWFILENAME_HEADING "Pending File Name"
#define FILENAME_HEADING "Current File Name"
#define FILEFOLDER_HEADING "File Folder Location"
#define FILETITLE_HEADING "Episode Title"
#define TVSHOWNAME_HEADING "TV Show Name"
#define TVSHOWID_HEADING "Online TV Show ID"
#define SEASONNUM_HEADING "Season Number"
#define EPISODENUM_HEADING "Episode Number"

#define COLUMN_COUNT 8


class TVShowModel: public QAbstractTableModel
{
    Q_OBJECT

public:
    TVShowModel(QObject *parent=0);
    TVShowModel(QVector<TVShowClass> TVShowItemList, QObject *parent=0);

    int rowCount(const QModelIndex &parent) const;
    int columnCount(const QModelIndex &parent) const;
    QVariant data(const QModelIndex &index, int role) const;
    QVariant headerData(int section, Qt::Orientation orientation, int role) const;
    QVector<TVShowClass> getList();
    void addTVShowItem(TVShowClass TVShow);
    void removeAll();
    void removeTopRow();

private:
    QVector<TVShowClass> _TVShowItemList;
};

#endif // TVSHOWMODEL_H
