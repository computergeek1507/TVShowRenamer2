#include "settingsdialog.h"
#include "ui_settingsdialog.h"

SettingsDialog::SettingsDialog(QWidget *parent) :
    QDialog(parent),
    QSettings( "ScottNation", "TVShowRenamer", parent )
    //ui(new Ui::SettingsDialog)
{
    ui->setupUi(this);
    _ApplicationMajor   = 3;
    _ApplicationMinor   = 0;
    _ApplicationVersion = 0;
}

SettingsDialog::~SettingsDialog()
{
    //delete ui;
}

void SettingsDialog::on_buttonBox_accepted()
{

}
