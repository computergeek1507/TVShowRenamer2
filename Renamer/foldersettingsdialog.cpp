#include "foldersettingsdialog.h"
#include "ui_foldersettingsdialog.h"

FolderSettingsDialog::FolderSettingsDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::FolderSettingsDialog)
{
    ui->setupUi(this);
}

FolderSettingsDialog::~FolderSettingsDialog()
{
    delete ui;
}
