#ifndef TVSHOWMODEL_H
#define TVSHOWMODEL_H

#include <QAbstractTableModel>
#include <QList>
#include <QString>
#include <QVector>
#include "tvshowclass.h"

#define DATE_COLUMN 0
#define MODULE_COLUMN 1
#define FILE_AND_LINE_COLUMN 2
#define LEVEL_AND_MESSAGE_COLUMN 3
#define ERROR_COLUMN 4

#define DATE_HEADING "Current File Name"
#define MODULE_HEADING "Pending File Name"
#define FILE_AND_LINE_HEADING "TV Show Name"
#define LEVEL_AND_MESSAGE_HEADING "Episode Title"

#define COLUMN_COUNT 4


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
    void addLogItem(TVShowClass logItem);
    void removeAll();
    void removeTopRow();

private:
    QVector<TVShowClass> _TVShowItemList;
};

#endif // TVSHOWMODEL_H
