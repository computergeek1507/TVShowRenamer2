#include "tvshowmodel.h"

TVShowModel::TVShowModel(QObject *parent)
    : QAbstractTableModel(parent)
{
}

TVShowModel::TVShowModel(QVector<TVShowClass> TVShowItemList, QObject *parent)
      : QAbstractTableModel(parent)
{
    _TVShowItemList=TVShowItemList;
}

int TVShowModel::rowCount(const QModelIndex &parent) const
 {
     Q_UNUSED(parent);
     return _TVShowItemList.count();
 }

 int TVShowModel::columnCount(const QModelIndex &parent) const
 {
     Q_UNUSED(parent);
     return COLUMN_COUNT;
 }

 QVariant TVShowModel::data(const QModelIndex &index, int role) const
 {
     if (!index.isValid())
         return QVariant();

     if (index.row() >= _TVShowItemList.size() || index.row() < 0)
         return QVariant();

     return QVariant();
 }

 QVariant TVShowModel::headerData(int section, Qt::Orientation orientation, int role) const
 {
     if (role != Qt::DisplayRole)
         return QVariant();

     if (orientation == Qt::Horizontal)
     {
         if (section == NEWFILENAME_COLUMN)
             return tr(NEWFILENAME_HEADING);
         else if (section == FILENAME_COLUMN)
             return tr(FILENAME_HEADING);
         else if (section == FILEFOLDER_COLUMN)
             return tr(FILEFOLDER_HEADING);
         else if (section == FILETITLE_COLUMN)
             return tr(FILETITLE_HEADING);
         else if (section == TVSHOWNAME_COLUMN)
             return tr(TVSHOWNAME_HEADING);
         else if (section == TVSHOWID_COLUMN)
             return tr(TVSHOWID_HEADING);
         else if (section == SEASONNUM_COLUMN)
             return tr(SEASONNUM_HEADING);
         else if (section == EPISODENUM_COLUMN)
             return tr(EPISODENUM_HEADING);
     }
     return QVariant();
 }

 QVector<TVShowClass> TVShowModel::getList()
 {
     return _TVShowItemList;
 }

 void TVShowModel::addTVShowItem(TVShowClass TVShow)
 {
    int row = _TVShowItemList.count();

    // insert 1 row.
    beginInsertRows(QModelIndex(), row, row);

    _TVShowItemList.append(TVShow);

    endInsertRows();
 }

 void TVShowModel::removeAll()
 {
    beginRemoveRows(QModelIndex(), 0, _TVShowItemList.count() - 1);
    _TVShowItemList.clear();
    endRemoveRows();
 }

 void TVShowModel::removeTopRow()
 {
    beginRemoveRows(QModelIndex(), 0, 0);
    _TVShowItemList.remove(0);
    endRemoveRows();
 }
