#ifndef FOLDERSETTINGSDIALOG_H
#define FOLDERSETTINGSDIALOG_H

#include <QDialog>

namespace Ui {
class FolderSettingsDialog;
}

class FolderSettingsDialog : public QDialog
{
    Q_OBJECT
    
public:
    explicit FolderSettingsDialog(QWidget *parent = 0);
    ~FolderSettingsDialog();
    
private:
    Ui::FolderSettingsDialog *ui;
};

#endif // FOLDERSETTINGSDIALOG_H
