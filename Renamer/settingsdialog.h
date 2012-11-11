#ifndef SETTINGSDIALOG_H
#define SETTINGSDIALOG_H

#include <QDialog>
#include <QSettings>
#include <ui_settingsdialog.h>

class SettingsDialog : public QDialog, QSettings
{
    Q_OBJECT
    
public:
   SettingsDialog(QWidget *parent = NULL);
    ~SettingsDialog();
    
private slots:
    void on_buttonBox_accepted();

private:
    Ui::SettingsDialog ui;
    int _ApplicationMajor;
    int _ApplicationMinor;
    int _ApplicationVersion;
    QString _WindowStyle;
};

#endif // SETTINGSDIALOG_H
