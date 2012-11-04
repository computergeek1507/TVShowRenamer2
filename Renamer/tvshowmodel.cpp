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
         if (section == DATE_COLUMN)
             return tr(DATE_HEADING);
         else if (section == MODULE_COLUMN)
             return tr(MODULE_HEADING);
         else if (section == FILE_AND_LINE_COLUMN)
             return tr(FILE_AND_LINE_HEADING);
         else if (section == LEVEL_AND_MESSAGE_COLUMN)
             return tr(LEVEL_AND_MESSAGE_HEADING);
     }

     return QVariant();
 }

 QVector<TVShowClass> TVShowModel::getList()
 {
     return _TVShowItemList;
 }

 void TVShowModel::addLogItem(TVShowClass TVShow)
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
