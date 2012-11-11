#ifndef CONVERTIONSETTINGSCLASS_H
#define CONVERTIONSETTINGSCLASS_H

#include "ui_ConvertionSettings.h"
#include <QSize>
#include <QPoint>
#include <QDialog>
#include <QSettings>
#include <QFileDialog>
#include <QMessageBox>
#include <QUrl>
#include <QDesktopServices>
#include <QColorDialog>

class ConvertionSettingsClass : public QDialog, QSettings
{
    Q_OBJECT
public:
    ConvertionSettingsClass(QWidget *parent = NULL);
	
	void load();
	void save();
	bool openSettings();

	void accept();
    
public slots:



private:
	Ui::ConvertionSettings ui;
	int _ApplicationMajor;
	int _ApplicationMinor;
	int _ApplicationVersion;
	QString _WindowStyle;
};

#endif // CONVERTIONSETTINGSCLASS_H
