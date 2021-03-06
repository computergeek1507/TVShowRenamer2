#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QItemSelection>
#include <QFileDialog>
#include <QDir>
#include <QtNetwork>
#include <QMessageBox>
#include <QSettings>
#include <QHash>
#include <QSortFilterProxyModel>
#include "tvshowmodel.h"
#include "convertiondialog.h"
#include "settingsdialog.h"
#include "tvepisodetitle.h"

namespace Ui
{
	class MainWindow;
}

class MainWindow : public QMainWindow
{
	Q_OBJECT

public:
	explicit MainWindow(QWidget *parent = 0);
	~MainWindow();

private slots:
	void on_actionAdd_Files_triggered();
	void on_actionAdd_Folder_triggered();
	void on_actionRemove_Selected_triggered();
	void on_actionClear_List_triggered();
	void on_actionExit_triggered();
	void on_actionConvertion_Profiles_triggered();
	void on_actionOptions_triggered();
	void on_pushButtonSave_clicked();
	void on_pushButtonMove_clicked();
	void on_pushButtonCopy_clicked();
	void on_pushButtonGetTitle_clicked();

private:
	Ui::MainWindow *ui;
	SettingsDialog *_MainSettings;
	ConvertionDialog *_ConvertionSettings;
	QSortFilterProxyModel *proxyModel;
	TVShowModel *_TVShowModelList;
	QSettings* _ConvertionSettingsQSettings;

	tvepisodetitle *episodeTitle;

	QStringList _UsableEXT;

	bool ConvertFileName();
	void RecurseDirectory(const QString& sDir);
	void LoadSettings();
	void SaveSettings();
	QString TrimExtraChar(QString string);
};

#endif // MAINWINDOW_H
